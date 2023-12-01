using DV.Simulation.Brake;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using HarmonyLib;
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
			LODGroup lodGroup = transform.Find("LocoS282A_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD0 = Instantiate(S282ALOD0MeshSplitter.SplitLocoBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			AddRenderersToLOD(lodGroup, 0, splitLocoBodyLOD0.transform);

			GameObject splitLocoBodyLOD1 = Instantiate(S282ALOD1MeshSplitter.SplitLocoBodyLOD1, transform.Find("LocoS282A_Body/Static_LOD1"));
			AddRenderersToLOD(lodGroup, 1, splitLocoBodyLOD1.transform);

			//Set skin to main locomotive mesh's skin
			Transform locoBody = splitLocoBodyLOD0.transform.parent
					.Find("s282_locomotive_body");
			locoBody.gameObject.SetActive(false);
			splitLocoBodyLOD1.transform.parent
					.Find("s282_locomotive_body_LOD1").gameObject
					.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;
			splitLocoBodyLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);
			splitLocoBodyLOD1.transform.localPosition = new Vector3(0, 0, 4.885f);

			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD1.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}

			splitLocoBodyLOD0.SetActive(true);
			splitLocoBodyLOD1.SetActive(true);

			//Smokebox door
			GameObject splitSmokeboxDoorLOD0 = Instantiate(S282ASmokeboxDoorMeshSplitter.SplitSmokeboxDoorBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			AddRenderersToLOD(lodGroup, 0, splitSmokeboxDoorLOD0.transform);
			splitSmokeboxDoorLOD0.SetActive(true);
			splitSmokeboxDoorLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);
			GameObject splitSmokeboxDoorLOD1 = Instantiate(S282ASmokeboxDoorMeshSplitter.SplitSmokeboxDoorBodyLOD1, transform.Find("LocoS282A_Body/Static_LOD1"));
			AddRenderersToLOD(lodGroup, 1, splitSmokeboxDoorLOD1.transform);
			splitSmokeboxDoorLOD1.SetActive(true);
			splitSmokeboxDoorLOD1.transform.localPosition = new Vector3(0, 0, 4.88f);

			foreach (MeshRenderer meshRenderer in splitSmokeboxDoorLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in splitSmokeboxDoorLOD1.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			splitLocoBodyLOD0.transform.parent
					.Find("s282_locomotive_smokebox_door")
					.gameObject.SetActive(false);
			splitLocoBodyLOD1.transform.parent
					.Find("s282_locomotive_smokebox_door_LOD1")
					.gameObject.SetActive(false);

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

		private void AddRenderersToLOD(LODGroup lodGroup, int lodIndex, Transform renderers)
		{
			LOD[] lods = lodGroup.GetLODs();
			if (lodIndex < 0)
			{
				Main.Logger.Error("lodIndex less than 0");
			}
			else if (lodIndex >= lods.Length)
			{
				Main.Logger.Error("lodIndex greater than # of lods");
			}
			LOD lod = lods[lodIndex];
			Main.Logger.Log("[DEBUG] lodIndex: " + lodIndex.ToString());
			Main.Logger.Log("[DEBUG] Mesh renderers: " + renderers.GetComponentsInChildren<MeshRenderer>().Length.ToString());
			lod = new LOD(lod.screenRelativeTransitionHeight, lod.renderers.AddRangeToArray(renderers.GetComponentsInChildren<MeshRenderer>()));
			lods[lodIndex] = lod;
			lodGroup.SetLODs(lods);
			lodGroup.RecalculateBounds();
		}
	}
}
