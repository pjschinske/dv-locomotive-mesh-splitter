using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.ThingTypes;
using HarmonyLib;
using LocoMeshSplitter.MeshLoaders;
using UnityEngine;
using VLB;

namespace LocoMeshSplitter.Patches
{

	[HarmonyPatch(typeof(TrainCar), nameof(TrainCar.Start))]
	class TrainCarStartPatch
	{
		//TODO: Does this function need to run after the Gauge mod's patch?
		static void Postfix(ref TrainCar __instance)
		{
			if (__instance is null)
			{
				return;
			}
			switch (__instance.carLivery.parentType.id)
			{
				case "LocoS282A":
					__instance.GetOrAddComponent<S282AMeshLoader>();
					FixExplosionsForS282A(__instance);
					break;
				case "LocoS060":
					__instance.GetOrAddComponent<S060MeshLoader>();
					break;
				case "LocoDE6":
					__instance.GetOrAddComponent<DE6MeshLoader>();
					break;
				case "LocoDE6Slug":
					__instance.GetOrAddComponent<DE6SMeshLoader>();
					break;
			}
		}

		//We need the new smokebox door meshes to be hidden when the train is exploded,
		//just like the default smokebox door mesh does.
		private static void FixExplosionsForS282A(TrainCar s282a)
		{
			ExplosionModelHandler explosionHandler = s282a.GetComponent<ExplosionModelHandler>();
			if (explosionHandler is null)
			{
				Main.Logger.Warning("ExplosionModelHandler null on an S282A.");
				return;
			}
			Transform smokeboxDoor
				= s282a.transform.Find("LocoS282A_Body/Static_LOD0/s282a_split_smokebox_door(Clone)");
			if (smokeboxDoor is null)
			{
				Main.Logger.Warning("Could not find split smokebox door assembly.");
				return;
			}
			explosionHandler.gameObjectsToDisable
				= explosionHandler.gameObjectsToDisable.AddItem(smokeboxDoor.gameObject).ToArray();

			explosionHandler.nonExplodedModelGOs.Add(smokeboxDoor.gameObject);
		}
	}
}
