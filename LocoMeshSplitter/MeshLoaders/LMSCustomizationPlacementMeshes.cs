using DV;
using DV.Customization;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LocoMeshSplitter.MeshLoaders
{
	public class LMSCustomizationPlacementMeshes : MonoBehaviour
	{
		private Dictionary<MeshFilter, MeshCollider> gadgetMeshes;

		//in B99, this is "[GadgetMeshColliders]"
		private const string GADGET_ROOT = CustomizationPlacementMeshes.ROOT;
		//In B99, this is 29
		private readonly int LAYER = DV.Layers.DVLayer.Gadget_Mesh_Placing.ToInt();

		private Transform gadgetColliderHolder;

		public LMSCustomizationPlacementMeshes()
		{
			gadgetMeshes = new();
			gadgetColliderHolder = FindRoot();
		}

		private Transform FindRoot()
		{
			var interior = transform.GetComponentInParent<TrainCarInteriorObject>();
			TrainCar trainCar;
			if (interior is null)
			{
				trainCar = transform.GetComponent<TrainCar>();
			}
			else
			{
				trainCar = interior.actualTrainCar;
			}

			Transform gadgetMeshColliders = trainCar.interior.Find(GADGET_ROOT);
			if (gadgetMeshColliders == null)
			{
				gadgetMeshColliders = new GameObject(GADGET_ROOT).transform;
				gadgetMeshColliders.SetParent(trainCar.interior, worldPositionStays: false);
				return gadgetMeshColliders.gameObject
					.AddComponent<GadgetColliderHolder>()
					.holder.transform;
			}
			return gadgetMeshColliders
				.GetComponent<GadgetColliderHolder>()
				?.holder.transform;
		}

		public void AddGadgetMesh(MeshFilter meshFilter)
		{
			if (gadgetMeshes.ContainsKey(meshFilter))
			{
				Main.Logger.Warning($"Tried to generate gadget mesh for `{meshFilter.name}`," +
					$"but it existed already");
				return;
			}

			GameObject gadgetMeshGO = new GameObject($"[LMSGadgetMeshCollider][{meshFilter.name}]");
			gadgetMeshGO.transform.SetParent(gadgetColliderHolder, worldPositionStays: false);
			Vector3 position = transform.InverseTransformPoint(meshFilter.transform.position);
			Quaternion rotation = Quaternion.Inverse(transform.rotation) * meshFilter.transform.rotation;
			gadgetMeshGO.transform.position = gadgetColliderHolder.TransformPoint(position);
			gadgetMeshGO.transform.localScale = meshFilter.transform.localScale;
			gadgetMeshGO.transform.rotation = gadgetColliderHolder.rotation * rotation;
			gadgetMeshGO.layer = LAYER;
			MeshCollider gadgetMeshCollider = gadgetMeshGO.AddComponent<MeshCollider>();
			gadgetMeshCollider.sharedMesh = meshFilter.sharedMesh;
			gadgetMeshes.Add(meshFilter, gadgetMeshCollider);
		}

		public void ReloadGadgetMeshTransform(MeshFilter mf)
		{
			MeshCollider mc = gadgetMeshes[mf];
			if (mc is null)
			{
				Main.Logger.Error($"Couldn't find mesh filter '{mf.name}' in LMSCPM '{name}', failed to update transform.");
				return;
			}
			Vector3 position = transform.InverseTransformPoint(mf.transform.position);
			Quaternion rotation = Quaternion.Inverse(transform.rotation) * mf.transform.rotation;
			mc.transform.position = gadgetColliderHolder.TransformPoint(position);
			mc.transform.localScale = mf.transform.localScale;
			mc.transform.rotation = gadgetColliderHolder.rotation * rotation;
		}

		public void ReloadGadgetMesh(MeshFilter mf)
		{
			MeshCollider mc = gadgetMeshes[mf];
			if (mc is null)
			{
				Main.Logger.Error($"Couldn't find mesh filter '{mf.name}' in LMSCPM '{name}', failed to update transform.");
				return;
			}
			mc.sharedMesh = mf.sharedMesh;
		}

		public void ReloadAllGadgetMeshTransforms()
		{
			//for each meshfilter
			//make sure it's transform matches its gadgetmesh's transform
			foreach (KeyValuePair<MeshFilter, MeshCollider> kvp in gadgetMeshes)
			{
				Vector3 position = transform.InverseTransformPoint(kvp.Key.transform.position);
				Quaternion rotation = Quaternion.Inverse(transform.rotation) * kvp.Key.transform.rotation;
				kvp.Value.transform.position = gadgetColliderHolder.TransformPoint(position);
				kvp.Value.transform.localScale = kvp.Key.transform.localScale;
				kvp.Value.transform.rotation = gadgetColliderHolder.rotation * rotation;
			}
		}

		public void ReloadAllGadgetMeshes()
		{
			//for each meshfilter
			//make sure each mesh matches its gadgetmesh's mesh
			//for each meshfilter
			//make sure it's transform matches its gadgetmesh's transform
			foreach (KeyValuePair<MeshFilter, MeshCollider> kvp in gadgetMeshes)
			{
				kvp.Value.sharedMesh = kvp.Key.sharedMesh;
			}
		}

	}
}
