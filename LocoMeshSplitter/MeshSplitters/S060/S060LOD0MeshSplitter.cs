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

		private static readonly RangeFloat headlightLimitX = new(-0.2f, 0.2f);
		private static readonly RangeFloat headlightLimitY = new(3.2f, 8f);
		private static readonly RangeFloat headlightLimitZ = new(3.61f, 8f);

		private static readonly RangeFloat headlightBracketLimitX = new(-0.15f, 0.15f);
		private static readonly RangeFloat headlightBracketLimitY = new(3.06f, 3.25f);
		private static readonly RangeFloat headlightBracketLimitZ = new(3.651f, 8f);

		private static readonly RangeFloat taillightUpperLimitX = new(-0.22f, 0.22f);
		private static readonly RangeFloat taillightUpperLimitY = new(2.9f, 4.17f);
		private static readonly RangeFloat taillightUpperLimitZ = new(-8f, -4.1f);

		private static readonly RangeFloat taillightLowerLeftLimitX = new(0.8f, 1.25f);
		private static readonly RangeFloat taillightLowerRightLimitX = new(-1.25f, -0.8f);
		private static readonly RangeFloat taillightLowerLimitY = new(1.25f, 1.79f);
		private static readonly RangeFloat taillightLowerLimitZ = new(-8f, -3.9f);

		private static readonly RangeFloat bunkerUpperLimitX = new(-0.78f, 0.78f);
		private static readonly RangeFloat bunkerUpperLimitY = new(2.49f, 3.75f);
		private static readonly RangeFloat bunkerUpperLimitZ = new(-8f, -3.49f);

		private static readonly RangeFloat bunkerLowerLimitX = new(-4, 4);
		private static readonly RangeFloat bunkerLowerLimitY = new(1.209f, 2.5f);
		private static readonly RangeFloat bunkerLowerLimitZ = new(-8f, -3.49f);

		private static readonly RangeFloat rearWallLimitX = new(-4, 4);
		private static readonly RangeFloat rearWallLimitY = new(2.4f, 8f);
		private static readonly RangeFloat rearWallLimitZ = new(-3.501f, -3.499f);

		private static readonly RangeFloat bumperLimitX = new(-4, 4);
		private static readonly RangeFloat bumperLimitY = new(0f, 1.211f);
		private static readonly RangeFloat bumperLimitZ = new(-8f, -3.799f);

		private static readonly RangeFloat rearSubframeLeftLimitX = new(0.431f, 0.545f);
		private static readonly RangeFloat rearSubframeRightLimitX = new(-0.545f, -0.431f);
		private static readonly RangeFloat rearSubframeLimitY = new(0.55f, 1.211f);
		private static readonly RangeFloat rearSubframeLimitZ = new(-3.801f, -0.836f);


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
			Mesh rearSubframeLeftMesh = GetRearSubframeLeftMesh(locoMesh);
			Mesh rearSubframeRightMesh = GetRearSubframeRightMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleBracketLimitX, whistleBracketLimitY, whistleBracketLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearSubframeLeftLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearSubframeRightLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh whistleLMesh = GetWhistleLMesh(locoMesh);
			Mesh whistleRMesh = GetWhistleRMesh(locoMesh);
			Mesh bumperMesh = GetBumperMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, whistleLLimitX, whistleLimitY, whistleLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleRLimitX, whistleLimitY, whistleLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bumperLimitX, bumperLimitY, bumperLimitZ);
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

			Mesh headlightMesh = GetHeadlightMesh(locoMesh);
			Mesh headlightBracketMesh = GetHeadlightBracketMesh(locoMesh);
			Mesh taillightUpperMesh = GetTaillightUpperMesh(locoMesh);
			Mesh taillightLowerLeftMesh = GetTaillightLowerLeftMesh(locoMesh);
			Mesh taillightLowerRightMesh = GetTaillightLowerRightMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, headlightBracketLimitX, headlightBracketLimitY, headlightBracketLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightUpperLimitX, taillightUpperLimitY, taillightUpperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightLowerLeftLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightLowerRightLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh bunkerMesh = GetBunkerMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, bunkerUpperLimitX, bunkerUpperLimitY, bunkerUpperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bunkerLowerLimitX, bunkerLowerLimitY, bunkerLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearWallLimitX, rearWallLimitY, rearWallLimitZ);
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

			GameObject headlight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlight.GetComponent<MeshFilter>().mesh = headlightMesh;
			headlight.name = "s060_headlight";

			GameObject headlightBracket = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightBracket.GetComponent<MeshFilter>().mesh = headlightBracketMesh;
			headlightBracket.name = "s060_headlight_bracket";

			GameObject taillightUpper = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			taillightUpper.GetComponent<MeshFilter>().mesh = taillightUpperMesh;
			taillightUpper.name = "s060_taillight_upper";

			GameObject taillightLowerLeft = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			taillightLowerLeft.GetComponent<MeshFilter>().mesh = taillightLowerLeftMesh;
			taillightLowerLeft.name = "s060_taillight_lower_left";

			GameObject taillightLowerRight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			taillightLowerRight.GetComponent<MeshFilter>().mesh = taillightLowerRightMesh;
			taillightLowerRight.name = "s060_taillight_lower_right";

			GameObject bunker = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			bunker.GetComponent<MeshFilter>().mesh = bunkerMesh;
			bunker.name = "s060_bunker";

			GameObject rearSubframeLeft = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			rearSubframeLeft.GetComponent<MeshFilter>().mesh = rearSubframeLeftMesh;
			rearSubframeLeft.name = "s060_rear_subframe_left";

			GameObject rearSubframeRight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			rearSubframeRight.GetComponent<MeshFilter>().mesh = rearSubframeRightMesh;
			rearSubframeRight.name = "s060_rear_subframe_right";

			GameObject bumper = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			bumper.GetComponent<MeshFilter>().mesh = bumperMesh;
			bumper.name = "s060_bumper";

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

		private static Mesh GetHeadlightMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeadlightBracketMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightBracketLimitX, headlightBracketLimitY, headlightBracketLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTaillightUpperMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, taillightUpperLimitX, taillightUpperLimitY, taillightUpperLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTaillightLowerLeftMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, taillightLowerLeftLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTaillightLowerRightMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, taillightLowerRightLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBunkerMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, bunkerUpperLimitX, bunkerUpperLimitY, bunkerUpperLimitZ);
			markPartOfMesh(mesh.vertices, triangles, bunkerLowerLimitX, bunkerLowerLimitY, bunkerLowerLimitZ);
			markPartOfMesh(mesh.vertices, triangles, rearWallLimitX, rearWallLimitY, rearWallLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetRearSubframeLeftMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, rearSubframeLeftLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetRearSubframeRightMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, rearSubframeRightLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBumperMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, bumperLimitX, bumperLimitY, bumperLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

	}
}
