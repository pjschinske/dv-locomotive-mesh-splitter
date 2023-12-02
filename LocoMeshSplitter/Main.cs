using System;
using System.Reflection;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using HarmonyLib;
using LocoMeshSplitter.MeshLoaders;
using LocoMeshSplitter.MeshSplitters.S060;
using LocoMeshSplitter.MeshSplitters.S282A;
using UnityEngine;
using UnityModManagerNet;

namespace LocoMeshSplitter
{
	public static class Main
	{
		internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

		// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
		private static bool Load(UnityModManager.ModEntry modEntry)
		{
			Logger = modEntry.Logger;
			Harmony? harmony = null;

			try
			{
				harmony = new Harmony(modEntry.Info.Id);
				harmony.PatchAll(Assembly.GetExecutingAssembly());

				// Other plugin startup logic

				//The idea here is to spawn a temporary S282 which gets the split meshes applied to it.
				//Then, whenever an S282 is spawned in game, the altered meshes have already been loaded
				//into the game.
				/*Logger.Log("Splitting up S282A...");
				GameObject s282aPrefab = TrainCarType.LocoSteamHeavy.ToV2().prefab;
				if (s282aPrefab is null)
				{
					Logger.Error("S282A prefab is null");
				}
				else
				{
					GameObject tempS282a = UnityEngine.Object.Instantiate(s282aPrefab);
					//tempS282a.SetActive(false);
					//tempS282a.AddComponent<S282AMeshLoader>();
					UnityEngine.Object.Destroy(tempS282a);
				}
				Logger.Log("Done splitting up S282A.");*/
				S282ALOD0MeshSplitter.Init();
				S282ALOD1MeshSplitter.Init();
				S282ASmokeboxDoorMeshSplitter.Init();
				S282ABrakeShoeMeshSplitter.Init();

				S060LOD0MeshSplitter.Init();
				S060LOD1MeshSplitter.Init();
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
