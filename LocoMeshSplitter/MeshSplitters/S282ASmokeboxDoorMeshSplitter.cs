using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters
{

	//this class separates the S282A's smokebox door from the front headlight
	public class S282ASmokeboxDoorMeshSplitter
	{
		public static GameObject SplitSmokeboxDoorBodyLOD0
		{ get; private set; }

		private static readonly RangeFloat headlightLimitX = new(-0.162f, 0.162f);
		private static readonly RangeFloat headlightLimitY = new(3.2f, 6f);
		private static readonly RangeFloat headlightLimitZ = new(5.75f, 7f);

		internal static void Init()
		{
			SplitSmokeboxDoorBodyLOD0 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitSmokeboxDoorBodyLOD0);
		}

		private static GameObject SplitMesh()
		{
			Mesh smokeboxDoorMesh = MeshFinder.FindMesh("s282_locomotive_smokebox_door");
			if (smokeboxDoorMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the s282_locomotive_smokebox_door mesh");
				return null;
			}

			GameObject splitSmokeboxDoorBodyLOD0 = new GameObject("s282a_split_smokebox_door");
			splitSmokeboxDoorBodyLOD0.SetActive(false);

			Mesh headlightMesh = UnityEngine.Object.Instantiate(smokeboxDoorMesh);
			splitSmokeboxDoorMesh(smokeboxDoorMesh, headlightMesh);

			GameObject smokeboxDoor = new GameObject("s282a_smokebox_door");
			smokeboxDoor.transform.SetParent(splitSmokeboxDoorBodyLOD0.transform);
			smokeboxDoor.transform.localScale = new Vector3(-1, 1, 1);
			smokeboxDoor.AddComponent<MeshFilter>().mesh = smokeboxDoorMesh;
			smokeboxDoor.AddComponent<MeshRenderer>();

			GameObject headlight = UnityEngine.Object.Instantiate(smokeboxDoor, splitSmokeboxDoorBodyLOD0.transform);
			headlight.GetComponent<MeshFilter>().mesh = headlightMesh;
			headlight.name = "s282a_headlight";

			return splitSmokeboxDoorBodyLOD0;
		}

		private static void splitSmokeboxDoorMesh(Mesh smokeboxMesh, Mesh headlightMesh)
		{
			int[] triangles = (int[])smokeboxMesh.triangles.Clone();
			markPartOfMesh(smokeboxMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			deleteUnmarkedPartOfMesh(headlightMesh, triangles);
			deleteMarkedPartOfMesh(smokeboxMesh, triangles);
		}
	}
}
