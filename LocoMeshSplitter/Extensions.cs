using DV.Customization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DV.Customization.CustomizationPlacementMeshes;

namespace LocoMeshSplitter
{
	public static class Extensions
	{
		public static void RegenerateGadgetMeshes(this CustomizationPlacementMeshes cpm)
		{
			//Whether we're on an interior or not, find the trainCar
			TrainCar trainCar = TrainCar.Resolve(cpm.gameObject);

			//Destroy all existing gadget collision meshes
			GadgetColliderHolder gch = cpm.FindRoot(trainCar);
			foreach (Transform gadgetCollisionMesh in gch.holder)
			{
				UnityEngine.Object.Destroy(gadgetCollisionMesh.gameObject);
			}

			//Now generate new ones, based on the meshes in cpm.collisionMeshes and cpm.drillDisableMeshes
			IEnumerator coroutine = GenerateNewGadgetMeshes(cpm, trainCar);
			cpm.StartCoroutine(coroutine);
		}

		//Waits a frame, to make sure that all old gadgetCollisionMeshes are destroyed, then generates new ones
		private static IEnumerator GenerateNewGadgetMeshes(CustomizationPlacementMeshes cpm, TrainCar trainCar)
		{
			yield return 0;
			cpm.GenerateCustomizationMeshes(trainCar);
		}
	}
}
