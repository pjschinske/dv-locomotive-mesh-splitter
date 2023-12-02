using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters.S060
{
	internal class S060LOD1MeshSplitter
	{
		public static GameObject SplitLocoBodyLOD1
		{ get; private set; }

		private static readonly RangeFloat tankLimitX = new(-2f, -0.74f);
		private static readonly RangeFloat tankLimitX2 = new(0.74f, 2f);
		private static readonly RangeFloat tankLimitY = new(1.65f, 3f);
		private static readonly RangeFloat tankLimitZ = new(-1.311f, 2.79f);

		internal static void Init()
		{
			SplitLocoBodyLOD1 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoBodyLOD1);
		}

		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh("s060_body_LOD1");
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the s060_body_LOD1 mesh");
				throw new System.IO.FileNotFoundException("MeshSplitter can't find the s060_body_LOD1 mesh");
			}
			Main.Logger.Log($"Splitting s060_body_LOD1 mesh...");

			GameObject splitLocoBodyLOD1 = new("s060_split_body_LOD1");
			splitLocoBodyLOD1.SetActive(false);

			Mesh tankLMesh = GetTankLMesh(locoMesh);
			Mesh tankRMesh = GetTankRMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX, tankLimitY, tankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX2, tankLimitY, tankLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			locoMesh.RecalculateNormals();
			locoMesh.RecalculateTangents();
			locoMesh.RecalculateBounds();

			GameObject splitLoco = new GameObject("s060_body");
			splitLoco.transform.SetParent(splitLocoBodyLOD1.transform);
			splitLoco.transform.localScale = new Vector3(-1, 1, 1);
			splitLoco.AddComponent<MeshFilter>().mesh = locoMesh;
			splitLoco.AddComponent<MeshRenderer>();

			GameObject tankL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD1.transform);
			tankL.GetComponent<MeshFilter>().mesh = tankLMesh;
			tankL.name = "s060_tank_l";

			GameObject tankR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD1.transform);
			tankR.GetComponent<MeshFilter>().mesh = tankRMesh;
			tankR.name = "s060_tank_r";

			Main.Logger.Log("Split s060_body_LOD1 mesh.");
			return splitLocoBodyLOD1;
		}

		private static Mesh GetTankLMesh(Mesh s060Mesh)
		{
			Mesh tankLMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])tankLMesh.triangles.Clone();
			markPartOfMesh(tankLMesh.vertices, triangles, tankLimitX2, tankLimitY, tankLimitZ);
			deleteUnmarkedPartOfMesh(tankLMesh, triangles);

			tankLMesh.RecalculateNormals();
			tankLMesh.RecalculateTangents();
			tankLMesh.RecalculateBounds();
			return tankLMesh;
		}

		private static Mesh GetTankRMesh(Mesh s060Mesh)
		{
			Mesh tankRMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])tankRMesh.triangles.Clone();
			markPartOfMesh(tankRMesh.vertices, triangles, tankLimitX, tankLimitY, tankLimitZ);
			deleteUnmarkedPartOfMesh(tankRMesh, triangles);

			tankRMesh.RecalculateNormals();
			tankRMesh.RecalculateTangents();
			tankRMesh.RecalculateBounds();
			return tankRMesh;
		}
	}
}
