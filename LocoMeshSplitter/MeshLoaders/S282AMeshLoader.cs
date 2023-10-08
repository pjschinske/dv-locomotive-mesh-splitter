using DV.Simulation.Brake;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using LocoMeshSplitter.MeshSplitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LocoMeshSplitter.MeshLoaders
{
	public class S282AMeshLoader : MonoBehaviour
	{
		internal S282AMeshLoader()
		{
			GameObject splitLocoBodyLOD0 = Instantiate(S282AMeshSplitter.SplitLocoBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			splitLocoBodyLOD0.SetActive(true);

			//Set skin to main locomotive mesh's skin
			Transform locoBody = splitLocoBodyLOD0.transform.parent
					.Find("s282_locomotive_body");
			locoBody.gameObject.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;
			splitLocoBodyLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);

			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}

			//Smokebox door
			GameObject splitSmokeboxDoorLOD0 = Instantiate(S282ASmokeboxDoorMeshSplitter.SplitSmokeboxDoorBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			splitSmokeboxDoorLOD0.SetActive(true);
			splitSmokeboxDoorLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);
			foreach (MeshRenderer meshRenderer in splitSmokeboxDoorLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			Transform oldSmokeboxDoor = splitLocoBodyLOD0.transform.parent
					.Find("s282_locomotive_smokebox_door");
			oldSmokeboxDoor.gameObject.SetActive(false);

			//brake shoes
			GameObject leftBrakeShoes = Instantiate(S282ABrakeShoeMeshSplitter.BrakeShoes, transform.Find("LocoS282A_Body/MovingParts_LOD0/DriveMechanism L"));
			GameObject rightBrakeShoes = Instantiate(S282ABrakeShoeMeshSplitter.BrakeShoes, transform.Find("LocoS282A_Body/MovingParts_LOD0/DriveMechanism R"));
			//hide the existing brake shoes; we've added new ones
			Transform oldBrakeShoes = splitLocoBodyLOD0.transform.parent
					.Find("s282_brake_shoes");
			oldBrakeShoes.gameObject.SetActive(false);
			leftBrakeShoes.gameObject.SetActive(true);
			rightBrakeShoes.gameObject.SetActive(true);
			//assign a texture to the new brake shoes
			foreach (MeshRenderer meshRenderer in leftBrakeShoes.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in rightBrakeShoes.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}

			//Add brake shoes to BrakesOverheatingController to make them glow
			BrakesOverheatingController brakesOverheatingController = GetComponent<BrakesOverheatingController>();
			var brakeRenderers = brakesOverheatingController.brakeRenderers.ToList();
			brakeRenderers.AddRange(leftBrakeShoes.GetComponentsInChildren<MeshRenderer>());
			brakeRenderers.AddRange(rightBrakeShoes.GetComponentsInChildren<MeshRenderer>());
			brakesOverheatingController.brakeRenderers = brakeRenderers.ToArray();

			/*FixExplodedModel(S282AMeshSplitter.SplitLocoBodyLOD0, splitLocoBodyLOD0);
			FixExplodedModel(S282ABrakeShoeMeshSplitter.BrakeShoes, leftBrakeShoes);
			FixExplodedModel(S282ABrakeShoeMeshSplitter.BrakeShoes, rightBrakeShoes);*/
		}

		// Exploded locos are handled separately from regular locos, so we need to alter
		// the model to look right when it gets exploded
		private void FixExplodedModel(GameObject prefab, GameObject spawnedGO)
		{
			ExplosionModelHandler explosionModelHandler = GetComponent<ExplosionModelHandler>();

			explosionModelHandler.gameObjectsToReplace = explosionModelHandler.gameObjectsToReplace
				.Append(spawnedGO)
				.ToArray();

			GameObject explodedFrontAxle = explosionModelHandler.replacePrefabsToSpawn[2];
			explosionModelHandler.replacePrefabsToSpawn = explosionModelHandler.replacePrefabsToSpawn
				.Append(prefab)
				.ToArray();

			explosionModelHandler.nonExplodedModelGOs.Add(spawnedGO);
		}
	}
}
