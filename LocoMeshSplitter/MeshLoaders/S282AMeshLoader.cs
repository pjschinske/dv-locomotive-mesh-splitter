using DV.Simulation.Brake;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using HarmonyLib;
using LocoMeshSplitter.MeshSplitters;
using LocoMeshSplitter.MeshSplitters.S282A;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;
using static Oni;

namespace LocoMeshSplitter.MeshLoaders
{
	public class S282AMeshLoader : MonoBehaviour
	{
		private TrainCar Car;

		private Transform SplitLocoBodyLOD0;
		private Transform SplitLocoBodyLOD1;

		private static readonly int[] frameFrontVertices =
		{
			649, 650, 691, 692, 696,
			1045, 1046, 1047, 1864, 1865,
			6678, 6679, 6684, 6685, 6689, 6690,
			23120, 23121, 23237, 23238, 23243, 23244, 23251, 23252,
			23343, 23344, 23347, 23348, 23398,
			23401, 23515, 23516, 23521, 23522,
			23529, 23532, 23621, 23624, 23625, 23628,
			24076, 24077, 24079, 24080, 24084, 24085, 24087, 24088,
		};

		internal S282AMeshLoader()
		{
			Car = transform.GetComponent<TrainCar>();

			LODGroup lodGroup = transform.Find("LocoS282A_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD0 = Instantiate(S282ALOD0MeshSplitter.SplitLocoBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			SplitLocoBodyLOD0 = splitLocoBodyLOD0.transform;
			AddRenderersToLOD(lodGroup, 0, splitLocoBodyLOD0.transform);

			GameObject splitLocoBodyLOD1 = Instantiate(S282ALOD1MeshSplitter.SplitLocoBodyLOD1, transform.Find("LocoS282A_Body/Static_LOD1"));
			SplitLocoBodyLOD1 = splitLocoBodyLOD1.transform;
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

		//Move cylinders to a certain location in Y or Z. (0,0) is the default location
		public void MoveCylinders(Vector3 position)
		{
			SplitLocoBodyLOD0.Find("s282a_cylinder_l").localPosition = position;
			SplitLocoBodyLOD0.Find("s282a_cylinder_r").localPosition = position;
			SplitLocoBodyLOD0.Find("s282a_cylinder_cocks").localPosition = position;

			SplitLocoBodyLOD1.Find("s282a_cylinder_l").localPosition = position;
			SplitLocoBodyLOD1.Find("s282a_cylinder_r").localPosition = position;
			SplitLocoBodyLOD1.Find("s282a_cylinder_cocks").localPosition = position;

			//move particles related to the cylinders as well
			Transform particles = Car.transform.Find("LocoS282A_Particles");
			particles.Find("CylCockWaterDrip").localPosition = position;
			particles.Find("CylCockSteam").localPosition = position;
			particles.Find("CylinderCrack").localPosition = position + new Vector3(1.42f, 1.04f, 9.08f);
		}

		//Move pilot by a certain amount in Y or Z. (0,0) means it won't move.
		//Also moves all the other crap in the front like the coupler.
		public void MovePilot(Vector2 position)
		{
			SplitLocoBodyLOD0.Find("s282a_pilot").localPosition
				+= new Vector3(0, position.x, position.y);
			SplitLocoBodyLOD0.parent.Find("s282_buffer_stems").localPosition
				+= new Vector3(0, position.x, position.y);
			transform.Find("[coupler front]").localPosition
				+= new Vector3(0, position.x, position.y);

			transform.Find("[buffers]").localPosition
				+= new Vector3(0, position.x, position.y);
			Car.interior.Find("ChainCoupler").localPosition
				+= new Vector3(0, position.x, position.y);
			Car.interior.Find("hoses").localPosition
				+= new Vector3(0, position.x, position.y);

			Mesh mesh = SplitLocoBodyLOD0.Find("s282a_body").GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;

			foreach (int index in frameFrontVertices)
			{
				vertices[index] += new Vector3(0, 0, position.y);
			}
			mesh.vertices = vertices;
		}
	}
}
