using DV;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LocoMeshSplitter.MeshSplitters
{
	internal class CustomizationMeshGenerator
	{
		internal static void GenerateCustomizationMeshes(Transform meshParent, Transform oldGadgetMeshHolder)
		{
			foreach (MeshFilter meshFilter in meshParent.GetComponentsInChildren<MeshFilter>(true))
			{
				Mesh mesh = meshFilter.sharedMesh;
				GameObject colliderObj = new GameObject("[LocoMeshSplitter_GadgetMeshCollider][" + meshFilter.name + "]");
				colliderObj.transform.parent = oldGadgetMeshHolder;
				colliderObj.layer = Layers.DVLayer.Gadget_Mesh_Placing.ToInt();
				MeshCollider meshCollider = colliderObj.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = mesh;
			}
		}
	}
}
