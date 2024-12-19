using DV.Customization;
using DV.Customization.Paint;
using DV.Logic.Job;
using DV.Simulation.Brake;
using HarmonyLib;
using LocoMeshSplitter.MeshSplitters.DE6S;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace LocoMeshSplitter.MeshLoaders
{
	internal class DE6SMeshLoader : MonoBehaviour
	{
		internal DE6SMeshLoader()
		{
			GameObject splitLocoBodyLOD0 = InitLocoBodyLOD0();
			GameObject splitLocoBodyLOD1 = InitLocoBodyLOD1();

			//This bit does nothing, since the slug doesn't have any instances of TrainCarPaint.
			//This is because in vanilla DV (as of B99.2), the slug only has the default livery.
			TrainCarPaint[] tcps = GetComponents<TrainCarPaint>();
			foreach (TrainCarPaint tcp in tcps)
			{
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD0.transform);
				TrainCarPaintSetup.SetupTrainCarPaintRenderers(tcp, splitLocoBodyLOD1.transform);
				//tcp.UpdateTheme();
			}

			splitLocoBodyLOD0.SetActive(true);
			splitLocoBodyLOD1.SetActive(true);
		}

		private GameObject InitLocoBodyLOD0()
		{
			LODGroup lodGroup = transform.Find("LocoDE6Slug_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD0 = Instantiate(DE6SLOD0MeshSplitter.SplitLocoBodyLOD0, transform.Find("LocoDE6Slug_Body"));
			AddRenderersToLOD(lodGroup, 0, splitLocoBodyLOD0.transform);

			//Set skin to main locomotive mesh's skin
			Transform locoBody = splitLocoBodyLOD0.transform.parent
				.Find("de6_slug_LOD0");
			locoBody.gameObject.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;

			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD0.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
			}

			return splitLocoBodyLOD0;
		}

		private GameObject InitLocoBodyLOD1()
		{
			LODGroup lodGroup = transform.Find("LocoDE6Slug_Body").GetComponent<LODGroup>();

			GameObject splitLocoBodyLOD1 = Instantiate(DE6SLOD1MeshSplitter.SplitLocoBodyLOD1, transform.Find("LocoDE6Slug_Body"));
			AddRenderersToLOD(lodGroup, 1, splitLocoBodyLOD1.transform);

			//Set skin to main locomotive mesh's skin
			Transform locoBody = splitLocoBodyLOD1.transform.parent
					.Find("de6_slug_LOD1");
			locoBody.gameObject.SetActive(false);
			Material locoMaterial = locoBody.GetComponent<MeshRenderer>().material;

			foreach (MeshRenderer meshRenderer in splitLocoBodyLOD1.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material = locoMaterial;
				//MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
				//lmsCPM.AddGadgetMesh(meshFilter);
			}

			return splitLocoBodyLOD1;
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
	}
}
