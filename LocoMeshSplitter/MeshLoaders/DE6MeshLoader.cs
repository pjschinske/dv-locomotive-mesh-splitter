using DV.Customization;
using DV.Customization.Paint;
using DV.Logic.Job;
using DV.Simulation.Brake;
using HarmonyLib;
using LocoMeshSplitter.MeshSplitters;
using LocoMeshSplitter.MeshSplitters.DE6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace LocoMeshSplitter.MeshLoaders
{
	internal class DE6MeshLoader : MonoBehaviour
	{
		internal DE6MeshLoader()
		{
			CustomizationPlacementMeshes cpm = GetComponent<CustomizationPlacementMeshes>();
			if (cpm is null)
			{
				Main.Logger.Error("Can't find CustomizationPlacementMeshes on DE6. Not splitting mesh");
				return;
			}

			GameObject splitLocoBodyLOD0 = InitLocoBodyLOD0();
			GameObject splitLocoBodyLOD1 = InitLocoBodyLOD1(cpm);
			GameObject splitLocoInteriorLOD1 = InitInteriorLOD1();
			InitInteriorLOD0();

			TrainCarPaint[] tcps = GetComponents<TrainCarPaint>();
			foreach (TrainCarPaint tcp in tcps)
			{
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD0.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD1.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoInteriorLOD1.transform);
				//tcp.UpdateTheme();
			}

			splitLocoBodyLOD0.SetActive(true);
			splitLocoBodyLOD1.SetActive(true);
			splitLocoInteriorLOD1?.SetActive(true);

			//The customization placement meshes need to be split. So we take the LOD1 meshes
			//and turn them into customization placement meshes.
			//CustomizationMeshGenerator.GenerateCustomizationMeshes(splitLocoBodyLOD1);
			TrainCar trainCar = GetComponent<TrainCar>();
			Transform oldGadgetMeshColliders = trainCar.interior.Find("[GadgetMeshColliders]");
			if (oldGadgetMeshColliders is not null)
			{
				Destroy(oldGadgetMeshColliders.gameObject);
			}
		}

		private GameObject InitLocoBodyLOD0()
		{
			LODGroup lodGroup = transform.Find("LocoDE6_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD0 = Instantiate(DE6LOD0MeshSplitter.SplitLocoBodyLOD0, transform.Find("LocoDE6_Body/Body"));
			AddRenderersToLOD(lodGroup, 0, splitLocoBodyLOD0.transform);

			//Set skin to main locomotive mesh's skin
			//While we're add it, add all meshes to the CustomizationPlacementMeshes
			Transform locoBody = splitLocoBodyLOD0.transform.parent
					.Find("diesel_body_LOD0");
			locoBody.gameObject.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;
			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}
			return splitLocoBodyLOD0;
		}

		private GameObject InitLocoBodyLOD1(CustomizationPlacementMeshes cpm)
		{
			LODGroup lodGroup = transform.Find("LocoDE6_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD1 = Instantiate(DE6LOD1MeshSplitter.SplitLocoBodyLOD1, transform.Find("LocoDE6_Body/Body"));
			AddRenderersToLOD(lodGroup, 1, splitLocoBodyLOD1.transform);

			//Set skin to main locomotive mesh's skin
			Transform locoBody = splitLocoBodyLOD1.transform.parent
					.Find("diesel_body_LOD1");
			locoBody.gameObject.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;
			List<MeshFilter> collisionMeshes = new();
			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD1.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
				MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
				collisionMeshes.Add(meshFilter);
			}
			cpm.collisionMeshes = collisionMeshes.ToArray();

			return splitLocoBodyLOD1;
		}

		private void InitInteriorLOD0()
		{
			TrainCar loco = GetComponent<TrainCar>();
			if (loco is null)
			{
				Main.Logger.Error("Couldn't find TrainCar from DE6MeshLoader");
				return;
			}
			loco.InteriorLoaded += SplitInteriorLOD0;
		}

		private GameObject InitInteriorLOD1()
		{
			LODGroup lodGroup = transform.Find("[interior LOD]/LocoDE6_InteriorLOD").GetComponent<LODGroup>();

			Transform cab = lodGroup.transform.Find("cab_LOD1");
			if (cab is null)
			{
				Main.Logger.Error("Can't find DE6 cab interior LOD1");
				return null;
			}
			cab.gameObject.SetActive(false);

			GameObject splitInteriorLOD1 = Instantiate(DE6InteriorLOD1MeshSplitter.SplitLocoInteriorLOD1, lodGroup.transform);

			Material locoMaterial = cab.GetComponent<MeshRenderer>().material;
			foreach (MeshRenderer meshRenderer in splitInteriorLOD1.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}

			AddRenderersToLOD(lodGroup, 0, splitInteriorLOD1.transform);

			return splitInteriorLOD1;
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
			//Main.Logger.Log("[DEBUG] Mesh renderers: " + renderers.GetComponentsInChildren<MeshRenderer>().Length.ToString());
			lod = new LOD(lod.screenRelativeTransitionHeight, lod.renderers.AddRangeToArray(renderers.GetComponentsInChildren<MeshRenderer>()));
			lods[lodIndex] = lod;
			lodGroup.SetLODs(lods);
			lodGroup.RecalculateBounds();
		}

		private void SplitInteriorLOD0(GameObject interior)
		{
			if (interior is null)
			{
				return;
			}

			Transform cab = interior.transform.Find("Cab");

			if (cab is null)
			{
				Main.Logger.Error("Can't find DE6 cab interior");
				return;
			}

			CustomizationPlacementMeshes cpm = GetComponent<CustomizationPlacementMeshes>();
			if (cpm is null)
			{
				Main.Logger.Error("Can't find CustomizationPlacementMeshes on DE6 interior. Not splitting interior mesh");
				return;
			}

			cab.gameObject.SetActive(false);

			GameObject splitInteriorLOD0 = Instantiate(DE6InteriorLOD0MeshSplitter.SplitLocoInteriorLOD0, interior.transform);

			//We need to set the texture for each part of the split interior.
			//We can't do this beforehand, because we want to stay compatible
			//with SkinManager, which swaps the textures at TrainCar.Start().
			//While we're at it, we can fill in CustomizationPlacementMeshes.
			Material locoMaterial = cab.GetComponent<MeshRenderer>().material;
			List<MeshFilter> collisionMeshes = cpm.collisionMeshes.ToList();
			foreach (MeshRenderer meshRenderer in splitInteriorLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
				MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
				collisionMeshes.Add(meshFilter);
			}
			cpm.collisionMeshes = collisionMeshes.ToArray();

			TrainCarPaint[] tcps = interior.GetComponents<TrainCarPaint>();
			foreach (TrainCarPaint tcp in tcps)
			{
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitInteriorLOD0.transform);
				//tcp.UpdateTheme();
			}

			splitInteriorLOD0.SetActive(true);
		}
	}
}
