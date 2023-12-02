using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.ThingTypes;
using HarmonyLib;
using LocoMeshSplitter.MeshLoaders;
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
					break;
				case "LocoS060":
					__instance.GetOrAddComponent<S060MeshLoader>();
					break;
			}
		}
	}
}
