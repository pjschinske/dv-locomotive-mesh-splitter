using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DV;
using DV.Customization;
using HarmonyLib;
using LocoMeshSplitter.MeshSplitters.DE6;
using UnityEngine;

namespace LocoMeshSplitter.Patches
{
	//We remove the default locomotive gadget collision meshes so that
	//we can add back in the split versions.
	[HarmonyPatch(
		typeof(CarSpawner),
		nameof(CarSpawner.Awake))]
	internal class RemoveUnsplitGadgetMeshesPatches
	{
		internal static void Prefix()
		{
			Globals.G.types.TryGetLivery("LocoDE6", out var de6Livery);
			Globals.G.types.TryGetLivery("LocoS060", out var s060Livery);
			Globals.G.types.TryGetLivery("LocoS282A", out var s282aLivery);
			if (de6Livery is null)
			{
				Main.Logger.Error("Can't find DE6 livery");
				return;
			}
			if (s060Livery is null)
			{
				Main.Logger.Error("Can't find S060 livery");
				return;
			}
			if (s282aLivery is null)
			{
				Main.Logger.Error("Can't find S282A livery");
				return;
			}
			RemoveMesh(de6Livery.prefab, "diesel_body_LOD1");
			RemoveMesh(de6Livery.interiorPrefab, "Cab");
			//RemoveMesh(de6SlugLivery.prefab, "de6_slug_LOD1");
			RemoveMesh(s060Livery.prefab, "s060_body_LOD1");
			//RemoveMesh(s060Livery.interiorPrefab, "s060_cab");
			RemoveMesh(s282aLivery.prefab, "s282_locomotive_body_LOD1");
			RemoveMesh(s282aLivery.prefab, "s282_locomotive_smokebox_door_LOD1");
			//RemoveMesh(s282aLivery.prefab, "Cab");
		}

		private static void RemoveMesh(GameObject prefab, string meshName)
		{
			CustomizationPlacementMeshes cpm = prefab.GetComponent<CustomizationPlacementMeshes>();
			if (cpm is null)
			{
				Main.Logger.Error($"Could not find CustomizationPlacementMeshes on '{prefab.name}'");
				return;
			}
			for (int i = 0; i < cpm.collisionMeshes.Length; i++)
			{
				MeshFilter currentMesh = cpm.collisionMeshes[i];
				if (currentMesh.name == meshName)
				{
					List<MeshFilter> newCollisionMeshes = cpm.collisionMeshes.ToList();
					newCollisionMeshes.RemoveAt(i);
					cpm.collisionMeshes = newCollisionMeshes.ToArray();
					return;
				}
			}

			Main.Logger.Warning($"Couldn't find mesh '{meshName}' in CustomizationPlacementMeshes");
		}
	}

	/*[HarmonyPatch(
		typeof(CustomizationPlacementMeshes),
		nameof(CustomizationPlacementMeshes.GenerateCustomizationMeshes))]
	internal class RemoveUnsplitGadgetMeshesPatches
	{
		internal static void Prefix(TrainCar car, CustomizationPlacementMeshes __instance)
		{
			bool isInterior = (__instance.GetComponentInParent<TrainCarInteriorObject>() is not null);
			Main.Logger.Log("GenerateCustomizationMeshes prefix:\n" +
				$"isInterior:\t'{isInterior}'\n" +
				$"car livery prefab name:\t'{car.carLivery.prefab.name}'\n" +
				$"CPM name:\t'{__instance.name}'\n" +
				$"first mesh name:\t'{(__instance.collisionMeshes.Length > 0 ? __instance.collisionMeshes[0] : "empty")}'");

			switch (__instance.name)
			{
				case "LocoDE6(Clone)":
					RemoveMesh(__instance, "diesel_body_LOD1");
					break;
				case "LocoDE6_Interior":
					RemoveMesh(__instance, "Cab");
					break;
				*//*case "LocoDE6Slug(Clone)":
					RemoveMesh(__instance, "LocoDE6Slug_Body/de6_slug_LOD1");
					break;*//*
				case "LocoS060(Clone)":
					RemoveMesh(__instance, "s060_body_LOD1");
					break;
				*//*case "LocoS060_Interior":
					//Uncomment if we ever split the s060 interior mesh
					RemoveMesh(__instance, "Static/s060_cab");
					break;*//*
				case "LocoS282A(Clone)":
					RemoveMesh(__instance, "s282_locomotive_body_LOD1");
					RemoveMesh(__instance, "s282_locomotive_smokebox_door_LOD1");
					break;
				*//*case "LocoS282A_Interior":
					//Uncomment if we ever split the s282a cab mesh
					RemoveMesh(__instance, "Static/Cab");
					break;*//*
			}
		}

		private static void RemoveMesh(CustomizationPlacementMeshes cpm, string meshName)
		{
			for (int i = 0; i < cpm.collisionMeshes.Length; i++)
			{
				MeshFilter currentMesh = cpm.collisionMeshes[i];
				if (currentMesh.name == meshName)
				{
					List<MeshFilter> newCollisionMeshes = cpm.collisionMeshes.ToList();
					newCollisionMeshes.RemoveAt(i);
					cpm.collisionMeshes = newCollisionMeshes.ToArray();
					return;
				}
			}

			Main.Logger.Warning($"Couldn't find mesh '{meshName}' in CustomizationPlacementMeshes");
		}
	}*/
}
