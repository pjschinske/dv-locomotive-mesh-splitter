using DV.Customization;
using DV.Customization.Paint;
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
		LMSCustomizationPlacementMeshes cpm;

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

			Car.InteriorLoaded += LoadInterior;

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
			//Main.Logger.Log($"[DEBUG] loco material texture name = '{locoMaterial.mainTexture.name}'");
			//Main.Logger.Log($"[DEBUG] real loco material texture name = '{locoMaterial.GetTexture("_MainTex").name}'");
			splitLocoBodyLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);
			splitLocoBodyLOD1.transform.localPosition = new Vector3(0, 0, 4.885f);

			//TODO: To make skin manager work, I think I need to rename the new materials.
			//check out what makes skin manager only replace the default texture

			cpm = gameObject.AddComponent<LMSCustomizationPlacementMeshes>();
			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD0.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD1.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
				MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
				cpm.AddGadgetMesh(meshFilter);
			}

			//Smokebox door
			GameObject splitSmokeboxDoorLOD0 = Instantiate(S282ASmokeboxDoorMeshSplitter.SplitSmokeboxDoorBodyLOD0, transform.Find("LocoS282A_Body/Static_LOD0"));
			AddRenderersToLOD(lodGroup, 0, splitSmokeboxDoorLOD0.transform);
			splitSmokeboxDoorLOD0.transform.localPosition = new Vector3(0, 0, 4.88f);
			GameObject splitSmokeboxDoorLOD1 = Instantiate(S282ASmokeboxDoorMeshSplitter.SplitSmokeboxDoorBodyLOD1, transform.Find("LocoS282A_Body/Static_LOD1"));
			AddRenderersToLOD(lodGroup, 1, splitSmokeboxDoorLOD1.transform);
			splitSmokeboxDoorLOD1.transform.localPosition = new Vector3(0, 0, 4.88f);

			foreach (MeshRenderer meshRenderer in splitSmokeboxDoorLOD0.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in splitSmokeboxDoorLOD1.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
				MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
				cpm.AddGadgetMesh(meshFilter);
			}
			splitLocoBodyLOD0.transform.parent
					.Find("s282_locomotive_smokebox_door")
					.gameObject.SetActive(false);
			splitLocoBodyLOD1.transform.parent
					.Find("s282_locomotive_smokebox_door_LOD1")
					.gameObject.SetActive(false);

			//brake shoes
			GameObject leftBrakeShoes = Instantiate(S282ABrakeShoeMeshSplitter.BrakeShoes, transform.Find("LocoS282A_Body/MovingParts_LOD0/LocoS282A_Drivetrain L"));
			GameObject rightBrakeShoes = Instantiate(S282ABrakeShoeMeshSplitter.BrakeShoes, transform.Find("LocoS282A_Body/MovingParts_LOD0/LocoS282A_Drivetrain R"));
			//hide the existing brake shoes; we've added new ones
			Transform oldBrakeShoes = splitLocoBodyLOD0.transform.parent
					.Find("s282_brake_shoes");	
			oldBrakeShoes.gameObject.SetActive(false);
			//assign a texture to the new brake shoes
			foreach (MeshRenderer meshRenderer in leftBrakeShoes.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
			}
			foreach (MeshRenderer meshRenderer in rightBrakeShoes.GetComponentsInChildren<MeshRenderer>(true))
			{
				meshRenderer.material = locoMaterial;
			}

			leftBrakeShoes.gameObject.SetActive(true);
			rightBrakeShoes.gameObject.SetActive(true);

			//Add brake shoes to BrakesOverheatingController to make them glow
			BrakesOverheatingController brakesOverheatingController = GetComponent<BrakesOverheatingController>();
			var brakeRenderers = brakesOverheatingController.brakeRenderers.ToList();
			brakeRenderers.AddRange(leftBrakeShoes.GetComponentsInChildren<MeshRenderer>(true));
			brakeRenderers.AddRange(rightBrakeShoes.GetComponentsInChildren<MeshRenderer>(true));
			brakesOverheatingController.brakeRenderers = brakeRenderers.ToArray();

			//Now that we've added a bunch of extra GameObjects, we need to make sure they get repainted
			TrainCarPaint[] tcps = GetComponents<TrainCarPaint>();

			Transform bellHammer = splitLocoBodyLOD0.transform.parent.Find("s282_bell_hammer");
			Transform bufferStems = splitLocoBodyLOD0.transform.parent.Find("s282_buffer_stems");
			Transform wheelsFrontSupport = splitLocoBodyLOD0.transform.parent.Find("s282_wheels_front_support");

			foreach (TrainCarPaint tcp in tcps)
			{
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD0.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD1.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitSmokeboxDoorLOD0.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitSmokeboxDoorLOD0.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, leftBrakeShoes.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, rightBrakeShoes.transform);
				if (bellHammer is not null)
					TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, bellHammer);
				if (bufferStems is not null)
					TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, bufferStems);
				if (wheelsFrontSupport is not null)
					TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, wheelsFrontSupport);

				//tcp.UpdateTheme();
			}

			splitLocoBodyLOD0.SetActive(true);
			splitLocoBodyLOD1.SetActive(true);
			splitSmokeboxDoorLOD0.SetActive(true);
			splitSmokeboxDoorLOD1.SetActive(true);
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
			//Main.Logger.Log("[DEBUG] lodIndex: " + lodIndex.ToString());
			//Logger.Log("[DEBUG] Mesh renderers: " + renderers.GetComponentsInChildren<MeshRenderer>(true).Length.ToString());
			lod = new LOD(lod.screenRelativeTransitionHeight, lod.renderers.AddRangeToArray(renderers.GetComponentsInChildren<MeshRenderer>(true)));
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
			cpm.ReloadAllGadgetMeshTransforms();
		}

		private Vector2 chainCouplerPosition = new Vector2(0.97f, 11.64f);
		private Vector2 hosesPosition = new Vector2(1.05f, 11.98f);

		//Move pilot by a certain amount in Y or Z. (0,0) means it won't move.
		//Also moves all the other crap in the front like the coupler.
		public void MovePilot(Vector2 position)
		{
			chainCouplerPosition = new Vector2(chainCouplerPosition.x + position.x, chainCouplerPosition.y + position.y);
			hosesPosition = new Vector2(hosesPosition.x + position.x, hosesPosition.y + position.y);

			SplitLocoBodyLOD0.Find("s282a_pilot").localPosition
				+= new Vector3(0, position.x, position.y);
			SplitLocoBodyLOD0.Find("s282a_frame_left").localPosition
				+= new Vector3(0, position.x, position.y);
			SplitLocoBodyLOD0.Find("s282a_frame_right").localPosition
				+= new Vector3(0, position.x, position.y);
			SplitLocoBodyLOD0.Find("s282a_frame_box_front").localPosition
				+= new Vector3(0, position.x, position.y);
			SplitLocoBodyLOD0.parent.Find("s282_buffer_stems").localPosition
				+= new Vector3(0, position.x, position.y);
			transform.Find("[coupler front]").localPosition
				+= new Vector3(0, position.x, position.y);

			transform.Find("[buffers]").localPosition
				+= new Vector3(0, position.x, position.y);
			if (Car.interior != null)
			{
				Car.interior.Find("ChainCoupler").localPosition
					+= new Vector3(0, position.x, position.y);
				Car.interior.Find("hoses").localPosition
					+= new Vector3(0, position.x, position.y);
			}

			cpm.ReloadAllGadgetMeshTransforms();

			//The rest of this crap was for moving vertices inside the frame. I split a few more things apart
			//so that we don't have to do that anymore.

			//Mesh bodyMesh = SplitLocoBodyLOD0.Find("s282a_body").GetComponent<MeshFilter>().mesh;
			//Vector3[] vertices = bodyMesh.vertices;

			/*Main.Logger.Log("Front frame vertices:");
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vertex = vertices[i];
				if (vertex.z > 6.44 && vertex.y < 1.001) {
					Main.Logger.Log("\t" + i + " " + vertex.ToString("F5"));
				}
			}*/

			/*foreach (int index in frameFrontVertices)
			{
				vertices[index] += new Vector3(0, 0, position.y);
			}*/
			//bodyMesh.vertices = vertices;
		}


		//TODO: also paint the ExternalInteractables
		//TODO: figure out why I had to do this lmao. I think I did it because at one point I wanted to
		//change stuff in the interior. I don't need to any more, but I may as well leave the structure in place.
		private void LoadInterior(GameObject loadedInterior)
		{
			return;
			/*Car.interior.Find("ChainCoupler").localPosition
					= new Vector3(0, chainCouplerPosition.x, chainCouplerPosition.y);
			Car.interior.Find("hoses").localPosition
				= new Vector3(0, hosesPosition.x, hosesPosition.y);*/
			if (loadedInterior is null)
			{
				return;
			}

			TrainCarPaint[] tcps = loadedInterior.GetComponents<TrainCarPaint>();
			/*Material locoMaterial = Car.loadedInterior.transform.Find("Static/Cab")
				.GetComponent<Renderer>().sharedMaterial;*/
			foreach (TrainCarPaint tcp in tcps)
			{
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, null);
				tcp.UpdateTheme();
			}
		}
	}
}
