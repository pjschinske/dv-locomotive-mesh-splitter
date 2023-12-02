using MeshXtensions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters.S060
{
	internal class S060LOD0MeshSplitter
	{
		public static GameObject SplitLocoBodyLOD0
		{ get; private set; }

		private static readonly RangeFloat tankLimitX = new(-1.29f, -0.7428f);
		private static readonly RangeFloat tankLimitX2 = new(0.7428f, 1.29f);
		private static readonly RangeFloat tankLimitY = new(1.65f, 2.81f);
		private static readonly RangeFloat tankLimitZ = new(-1.311f, 2.79f);

		private static readonly RangeFloat tank2LimitX = new(-3f, -0.74f);
		private static readonly RangeFloat tank2LimitX2 = new(0.74f, 3f);
		private static readonly RangeFloat tank2LimitY = new(1.65f, 3f);
		private static readonly RangeFloat tank2LimitZ = new(-1.29f, 2.79f);

		private static readonly RangeFloat tankLowRailLimitX = new(-1.27f, -1.23f);
		private static readonly RangeFloat tankLowRailLimitX2 = new(1.23f, 1.27f);
		private static readonly RangeFloat tankLowRailLimitY = new(1.55f, 1.631f);
		private static readonly RangeFloat tankLowRailLimitZ = new(-1.311f, 2.79f);

		private static readonly RangeFloat tankFloorLimitX = new(-3f, 3f);
		private static readonly RangeFloat tankFloorLimitY = new(1.619f, 1.661f);
		private static readonly RangeFloat tankFloorLimitZ = new(-1.31f, 2.81f);

		private static readonly RangeFloat sandDomeLimitX = new(-0.318f, 0.318f);
		private static readonly RangeFloat sandDomeLimitY = new(3f, 4f);
		private static readonly RangeFloat sandDomeFLimitZ = new(1.75f, 2.37f);
		private static readonly RangeFloat sandDomeRLimitZ = new(-0.97f, -0.35f);

		private static readonly RangeFloat sandPipesLimitX = new(-0.8f, 0.27f);
		private static readonly RangeFloat sandPipesLimitY = new(1.65f, 3.2f);
		private static readonly RangeFloat sandPipesFLimitZ = new(1.9f, 2.09f);
		private static readonly RangeFloat sandPipesRLimitZ = new(-0.74f, -0.55f);
		private static readonly RangeFloat sandPipesFLimitY2 = new(2.6f, 4f);
		private static readonly RangeFloat sandPipesFLimitZ2 = new(1.9f, 2.2f);

		private static readonly RangeFloat whistleBracketLimitX = new(-0.5f, -0.19f);
		private static readonly RangeFloat whistleBracketLimitY = new(2.9f, 3.12f);
		private static readonly RangeFloat whistleBracketLimitZ = new(-0.1f, 0f);

		private static readonly RangeFloat whistleLLimitX = new(-0.3f, -0.196f);
		private static readonly RangeFloat whistleRLimitX = new(-0.4f, -0.3f);
		private static readonly RangeFloat whistleLimitY = new(3.09f, 3.4f);
		private static readonly RangeFloat whistleLimitZ = new(-0.1f, -0.02f);

		internal static void Init()
		{
			SplitLocoBodyLOD0 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoBodyLOD0);
		}

		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh("s060_body");
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the s060_body mesh");
				throw new System.IO.FileNotFoundException("MeshSplitter can't find the s060_body mesh");
			}
			Main.Logger.Log($"Splitting s060_body mesh...");

			GameObject splitLocoBodyLOD0 = new("s060_split_body");
			splitLocoBodyLOD0.SetActive(false);

			Mesh sandDomeFMesh = GetSandDomeFMesh(locoMesh);
			Mesh sandDomeRMesh = GetSandDomeRMesh(locoMesh);
			Mesh whistleBracketMesh = GetWhistleBracketMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleBracketLimitX, whistleBracketLimitY, whistleBracketLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh whistleLMesh = GetWhistleLMesh(locoMesh);
			Mesh whistleRMesh = GetWhistleRMesh(locoMesh);
			markPartOfMesh(locoMesh.vertices, triangles, whistleLLimitX, whistleLimitY, whistleLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleRLimitX, whistleLimitY, whistleLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh sandPipesFMesh = GetSandPipesFMesh(locoMesh);
			Mesh sandPipesRMesh = GetSandPipesRMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesFLimitY2, sandPipesFLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesRLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh tankLMesh = GetTankLMesh(locoMesh);
			Mesh tankRMesh = GetTankRMesh(locoMesh);
			Mesh tankFloorMesh = GetTankFloorMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX, tankLimitY, tankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX2, tankLimitY, tankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tank2LimitX, tank2LimitY, tank2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tank2LimitX2, tank2LimitY, tank2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLowRailLimitX, tankLowRailLimitY, tankLowRailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLowRailLimitX2, tankLowRailLimitY, tankLowRailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankFloorLimitX, tankFloorLimitY, tankFloorLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			locoMesh.RecalculateNormals();
			locoMesh.RecalculateTangents();
			locoMesh.RecalculateBounds();

			GameObject splitLoco = new GameObject("s060_body");
			splitLoco.transform.SetParent(splitLocoBodyLOD0.transform);
			splitLoco.transform.localScale = new Vector3(-1, 1, 1);
			splitLoco.AddComponent<MeshFilter>().mesh = locoMesh;
			splitLoco.AddComponent<MeshRenderer>();

			GameObject tankL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankL.GetComponent<MeshFilter>().mesh = tankLMesh;
			tankL.name = "s060_tank_l";

			GameObject tankR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankR.GetComponent<MeshFilter>().mesh = tankRMesh;
			tankR.name = "s060_tank_r";

			GameObject tankFloor = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankFloor.GetComponent<MeshFilter>().mesh = tankFloorMesh;
			tankFloor.name = "s060_tank_floor";

			GameObject sandDomeF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			sandDomeF.GetComponent<MeshFilter>().mesh = sandDomeFMesh;
			sandDomeF.name = "s060_sand_dome_f";

			GameObject sandDomeR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			sandDomeR.GetComponent<MeshFilter>().mesh = sandDomeRMesh;
			sandDomeR.name = "s060_sand_dome_r";

			GameObject sandPipesF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			sandPipesF.GetComponent<MeshFilter>().mesh = sandPipesFMesh;
			sandPipesF.name = "s060_sand_pipes_f";

			GameObject sandPipesR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			sandPipesR.GetComponent<MeshFilter>().mesh = sandPipesRMesh;
			sandPipesR.name = "s060_sand_pipes_r";

			GameObject whistleL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			whistleL.GetComponent<MeshFilter>().mesh = whistleLMesh;
			whistleL.name = "s060_whistle_l";

			GameObject whistleR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			whistleR.GetComponent<MeshFilter>().mesh = whistleRMesh;
			whistleR.name = "s060_whistle_r";

			GameObject whistleBracket = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			whistleBracket.GetComponent<MeshFilter>().mesh = whistleBracketMesh;
			whistleBracket.name = "s060_whistle_bracket";

			Main.Logger.Log("Split S060 mesh.");
			return splitLocoBodyLOD0;
		}

		private static Mesh GetTankLMesh(Mesh s060Mesh)
		{
			Mesh tankLMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])tankLMesh.triangles.Clone();
			markPartOfMesh(tankLMesh.vertices, triangles, tankLimitX2, tankLimitY, tankLimitZ);
			markPartOfMesh(tankLMesh.vertices, triangles, tank2LimitX2, tank2LimitY, tank2LimitZ);
			markPartOfMesh(tankLMesh.vertices, triangles, tankLowRailLimitX2, tankLowRailLimitY, tankLowRailLimitZ);
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
			markPartOfMesh(tankRMesh.vertices, triangles, tank2LimitX, tank2LimitY, tank2LimitZ);
			markPartOfMesh(tankRMesh.vertices, triangles, tankLowRailLimitX, tankLowRailLimitY, tankLowRailLimitZ);
			deleteUnmarkedPartOfMesh(tankRMesh, triangles);

			tankRMesh.RecalculateNormals();
			tankRMesh.RecalculateTangents();
			tankRMesh.RecalculateBounds();
			return tankRMesh;
		}

		private static Mesh GetTankFloorMesh(Mesh s060Mesh)
		{
			Mesh tankFloorMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])tankFloorMesh.triangles.Clone();
			markPartOfMesh(tankFloorMesh.vertices, triangles, tankFloorLimitX, tankFloorLimitY, tankFloorLimitZ);
			deleteUnmarkedPartOfMesh(tankFloorMesh, triangles);

			tankFloorMesh.RecalculateNormals();
			tankFloorMesh.RecalculateTangents();
			tankFloorMesh.RecalculateBounds();
			return tankFloorMesh;
		}

		private static Mesh GetSandDomeFMesh(Mesh s060Mesh)
		{
			Mesh sandDomeFMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])sandDomeFMesh.triangles.Clone();
			markPartOfMesh(sandDomeFMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeFLimitZ);
			deleteUnmarkedPartOfMesh(sandDomeFMesh, triangles);

			sandDomeFMesh.RecalculateNormals();
			sandDomeFMesh.RecalculateTangents();
			sandDomeFMesh.RecalculateBounds();
			return sandDomeFMesh;
		}

		private static Mesh GetSandDomeRMesh(Mesh s060Mesh)
		{
			Mesh sandDomeRMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])sandDomeRMesh.triangles.Clone();
			markPartOfMesh(sandDomeRMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeRLimitZ);
			deleteUnmarkedPartOfMesh(sandDomeRMesh, triangles);

			sandDomeRMesh.RecalculateNormals();
			sandDomeRMesh.RecalculateTangents();
			sandDomeRMesh.RecalculateBounds();
			return sandDomeRMesh;
		}

		private static Mesh GetSandPipesFMesh(Mesh s060Mesh)
		{
			Mesh sandPipesFMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])sandPipesFMesh.triangles.Clone();
			markPartOfMesh(sandPipesFMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesFLimitZ);
			markPartOfMesh(sandPipesFMesh.vertices, triangles, sandPipesLimitX, sandPipesFLimitY2, sandPipesFLimitZ2);
			deleteUnmarkedPartOfMesh(sandPipesFMesh, triangles);

			sandPipesFMesh.RecalculateNormals();
			sandPipesFMesh.RecalculateTangents();
			sandPipesFMesh.RecalculateBounds();
			return sandPipesFMesh;
		}

		private static Mesh GetSandPipesRMesh(Mesh s060Mesh)
		{
			Mesh sandPipesRMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])sandPipesRMesh.triangles.Clone();
			markPartOfMesh(sandPipesRMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesRLimitZ);
			deleteUnmarkedPartOfMesh(sandPipesRMesh, triangles);

			sandPipesRMesh.RecalculateNormals();
			sandPipesRMesh.RecalculateTangents();
			sandPipesRMesh.RecalculateBounds();
			return sandPipesRMesh;
		}

		private static Mesh GetWhistleLMesh(Mesh s060Mesh)
		{
			Mesh whistleLMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])whistleLMesh.triangles.Clone();
			markPartOfMesh(whistleLMesh.vertices, triangles, whistleLLimitX, whistleLimitY, whistleLimitZ);
			deleteUnmarkedPartOfMesh(whistleLMesh, triangles);

			whistleLMesh.RecalculateNormals();
			whistleLMesh.RecalculateTangents();
			whistleLMesh.RecalculateBounds();
			return whistleLMesh;
		}

		private static Mesh GetWhistleRMesh(Mesh s060Mesh)
		{
			Mesh whistleRMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])whistleRMesh.triangles.Clone();
			markPartOfMesh(whistleRMesh.vertices, triangles, whistleRLimitX, whistleLimitY, whistleLimitZ);
			deleteUnmarkedPartOfMesh(whistleRMesh, triangles);

			whistleRMesh.RecalculateNormals();
			whistleRMesh.RecalculateTangents();
			whistleRMesh.RecalculateBounds();
			return whistleRMesh;
		}

		private static Mesh GetWhistleBracketMesh(Mesh s060Mesh)
		{
			Mesh whistleBracketMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])whistleBracketMesh.triangles.Clone();
			markPartOfMesh(whistleBracketMesh.vertices, triangles, whistleBracketLimitX, whistleBracketLimitY, whistleBracketLimitZ);
			deleteUnmarkedPartOfMesh(whistleBracketMesh, triangles);

			whistleBracketMesh.RecalculateNormals();
			whistleBracketMesh.RecalculateTangents();
			whistleBracketMesh.RecalculateBounds();
			return whistleBracketMesh;
		}

	}
}
