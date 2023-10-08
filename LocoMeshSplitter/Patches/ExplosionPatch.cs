using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using LocoMeshSplitter.MeshSplitters;
using UnityEngine;

namespace LocoMeshSplitter.Patches
{
	[HarmonyPatch(typeof(ExplosionModelHandler), nameof(ExplosionModelHandler.HandleExplosionModelChange))]
	class ExplosionPatch
	{
		static void Postfix(ref ExplosionModelHandler __instance)
		{ 
			string carID = __instance.GetComponent<TrainCar>().carLivery.parentType.id;
			switch (carID)
			{
				case "LocoS282A":
					Transform splitBody = __instance.transform.Find("LocoS282AExploded_Body(Clone)/Static_LOD0/s282a_split_body(Clone)");
					if (splitBody is not null)
					{
						UnityEngine.Object.Destroy(splitBody.gameObject);
					}
					__instance.transform.Find("LocoS282AExploded_Body(Clone)/Static_LOD0/s282_locomotive_body")
							.gameObject.SetActive(false);
					__instance.transform.Find("LocoS282AExploded_Body(Clone)/Static_LOD0/s282_brake_shoes")
						.gameObject.SetActive(false);
					__instance.transform.Find("LocoS282AExploded_Body(Clone)/MovingParts_LOD0")
						.gameObject.SetActive(false);

					GameObject explodedSplitBody =
						UnityEngine.Object.Instantiate(
							__instance.transform.Find("LocoS282A_Body/Static_LOD0/s282a_split_body(Clone)").gameObject,
						__instance.transform.Find("LocoS282AExploded_Body(Clone)/Static_LOD0"));
					explodedSplitBody.transform.localPosition = new Vector3(0, 0, 4.88f);
					GameObject explodedMovingParts =
						UnityEngine.Object.Instantiate(
							__instance.transform.Find("LocoS282A_Body/MovingParts_LOD0").gameObject,
							__instance.transform.Find("LocoS282AExploded_Body(Clone)"));

					//get exploded material. Doesn't matter from what part, they all have the same texture
					var explodedMaterial = __instance.transform
						.Find("LocoS282AExploded_Body(Clone)/")
						.GetComponentInChildren<MeshRenderer>()
						.material;

					//apply exploded texture to all the stuff we spawned in
					foreach (var renderer in explodedSplitBody.GetComponentsInChildren<MeshRenderer>(true))
					{
						if (renderer.material is null)
							continue;

						renderer.material = explodedMaterial;
					}
					foreach (var renderer in explodedMovingParts.GetComponentsInChildren<MeshRenderer>(true))
					{
						if (renderer.material is null)
							continue;

						renderer.material = explodedMaterial;
					}
					break;
			}
		}
	}
}
