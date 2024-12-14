using System;
using System.Linq;
using System.Reflection;
using DV;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using HarmonyLib;
using LocoMeshSplitter.MeshLoaders;
using LocoMeshSplitter.MeshSplitters.DE6;
using LocoMeshSplitter.MeshSplitters.DE6S;
using LocoMeshSplitter.MeshSplitters.S060;
using LocoMeshSplitter.MeshSplitters.S282A;
using UnityEngine;
using UnityModManagerNet;

namespace LocoMeshSplitter
{	
	public static class Main
	{
		internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }
		public static string ModPath { get; private set; }

		// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Logger = modEntry.Logger;
			ModPath = modEntry.Path;
			Harmony? harmony = null;

			try
			{
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// Other plugin startup logic

				//The idea here is to split up the locomotives based on the meshes obtained at
				//runtime with AssetStudio. Then when spawning in locomotives, we just need to
				//spawn in the respective split locomotive meshes.

				Logger.Log("Splitting up locomotives...");
				S282ALOD0MeshSplitter.Init();
				S282ALOD1MeshSplitter.Init();
				S282ASmokeboxDoorMeshSplitter.Init();
				S282ABrakeShoeMeshSplitter.Init();

				S060LOD0MeshSplitter.Init();
				S060LOD1MeshSplitter.Init();

				DE6LOD0MeshSplitter.Init();
				DE6LOD1MeshSplitter.Init();
				DE6InteriorLOD0MeshSplitter.Init();
				DE6InteriorLOD1MeshSplitter.Init();

				DE6SLOD0MeshSplitter.Init();
				DE6SLOD1MeshSplitter.Init();
				Logger.Log("Done splitting up locomotives.");
			}
			catch (Exception ex)
			{
				modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
				harmony?.UnpatchAll(modEntry.Info.Id);
				return false;
			}

			return true;
		}
	}
}
