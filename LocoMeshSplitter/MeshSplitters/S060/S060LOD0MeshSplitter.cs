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

		private static readonly RangeFloat tank2LimitX = new(-3f, -0.7429f);
		private static readonly RangeFloat tank2LimitX2 = new(0.7429f, 3f);
		private static readonly RangeFloat tank2LimitY = new(1.65f, 3f);
		private static readonly RangeFloat tank2LimitZ = new(-1.29f, 2.79f);

		private static readonly RangeFloat tankLowRailLimitX = new(-1.27f, -1.23f);
		private static readonly RangeFloat tankLowRailLimitX2 = new(1.23f, 1.27f);
		private static readonly RangeFloat tankLowRailLimitY = new(1.55f, 1.631f);
		private static readonly RangeFloat tankLowRailLimitZ = new(-1.311f, 2.79f);

		private static readonly RangeFloat tankFloorLimitX = new(-3f, 3f);
		private static readonly RangeFloat tankFloorLimitY = new(1.619f, 1.661f);
		private static readonly RangeFloat tankFloorLimitZ = new(-1.31f, 2.81f);

		private static readonly RangeFloat tankLadderLeftLimitX = new(0.809f, 8f);
		private static readonly RangeFloat tankLadderRightLimitX = new(-8f, -0.809f);
		private static readonly RangeFloat tankLadderLimitY = new(1.6f, 8f);
		private static readonly RangeFloat tankLadderLimitZ = new(0, 2.9158f);

		private static readonly RangeFloat steamDomeLimitX1 = new(0, 0.43f);
		private static readonly RangeFloat steamDomeLimitX2 = new(-4.3f, 0.39f);
		private static readonly RangeFloat steamDomeLimitY = new(2.95f, 8f);
		private static readonly RangeFloat steamDomeLimitZ1 = new(0.53f, 0.94f);
		private static readonly RangeFloat steamDomeLimitZ2 = new(0.26f, 1.13f);

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

		private static readonly RangeFloat bellLimitX = new(-0.8f, 0.3f);
		private static readonly RangeFloat bellLimitY1 = new(2.4f, 8f);
		private static readonly RangeFloat bellLimitY2 = new(3.15f, 8f);
		private static readonly RangeFloat bellLimitZ1 = new(2f, 2.7f);
		private static readonly RangeFloat bellLimitZ2 = new(2f, 2.8f);

		private static readonly RangeFloat headlightLimitX = new(-0.2f, 0.2f);
		private static readonly RangeFloat headlightLimitY = new(3.205f, 8f);
		private static readonly RangeFloat headlightLimitZ = new(3.61f, 8f);

		private static readonly RangeFloat headlightLowerLeftLimitX = new(0.7f, 1.1f);
		private static readonly RangeFloat headlightLowerRightLimitX = new(-1.1f, -0.7f);
		private static readonly RangeFloat headlightLowerLimitY = new(1.19f, 1.55f);
		private static readonly RangeFloat headlightLowerLimitZ = new(3.7f, 8f);

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

		private static readonly RangeFloat pilotLimitX = new(-4, 4);
		private static readonly RangeFloat pilotLimitY = new(0f, 1.211f);
		private static readonly RangeFloat pilotLimitZ = new(3.79f, 8f);

		private static readonly RangeFloat smokeboxBoltsLimitX = new(-0.7f, 0.7f);
		private static readonly RangeFloat smokeboxBoltsLimitY = new(1.7f, 3.1f);
		private static readonly RangeFloat smokeboxBoltsLimitZ = new(3.8f, 3.86f);

		private static readonly RangeFloat airPumpLimitX = new(0.46f, 4);
		private static readonly RangeFloat airPumpLimitY = new(1.64f, 2.9f);
		private static readonly RangeFloat airPumpLimitZ = new(3.394f, 3.91f);
		private static readonly RangeFloat airPumpLimitX2 = new(0.82f, 4);
		private static readonly RangeFloat airPumpLimitZ2 = new(3.38f, 3.91f);

		private static readonly RangeFloat handleFrontLeftLimitX = new(0, 0.74f);
		private static readonly RangeFloat handleFrontLeftLimitY = new(2.572f, 2.621f);
		private static readonly RangeFloat handleFrontLeftLimitZ = new(3f, 3.4f);

		private static readonly RangeFloat tankMountLimitX = new(-1.1f, 1.1f);
		private static readonly RangeFloat tankMountLimitY = new(1.65f, 2.402f);
		private static readonly RangeFloat tankMountLimitZ = new(2.77f, 2.795f);

		private static readonly RangeFloat tankMountUpperMountLimitX = new(-0.89f, 0.89f);
		private static readonly RangeFloat tankMountUpperMountLimitY = new(2.28f, 2.38f);
		private static readonly RangeFloat tankMountUpperMountLimitZ = new(2.79f, 3.01f);

		private static readonly RangeFloat tankMountLowerMountLimitX = new(-0.83f, 0.83f);
		private static readonly RangeFloat tankMountLowerMountLimitX2 = new(-0.65f, 0.65f);
		private static readonly RangeFloat tankMountLowerMountLimitY = new(2.03f, 2.13f);
		private static readonly RangeFloat tankMountLowerMountLimitZ = new(2.70f, 2.842f);
		private static readonly RangeFloat tankMountLowerMountLimitZ2 = new(2.70f, 2.98f);

		private static readonly RangeFloat airPipes1LimitX = new(0.47f, 0.528f);
		private static readonly RangeFloat airPipes1LimitY = new(1.65f, 1.85f);
		private static readonly RangeFloat airPipes1LimitZ = new(2.7f, 3.7f);

		private static readonly RangeFloat guttersFLimitX = new(-1.24f, -0.71f);
		private static readonly RangeFloat guttersFLimitX2 = new(0.71f, 1.24f);
		private static readonly RangeFloat guttersFLimitY = new(3.45f, 3.48f);
		private static readonly RangeFloat guttersFLimitZ = new(-1.31f, -1.2f);

		private static readonly RangeFloat whistleLinkageLimitX = new(-0.73f, -0.67f);
		private static readonly RangeFloat whistleLinkageLimitY = new(3.4f, 3.5f);
		private static readonly RangeFloat whistleLinkageLimitZ = new(-1.31f, -1.2f);
		private static readonly RangeFloat whistleLinkageLimitX2 = new(-0.72f, -0.23f);
		private static readonly RangeFloat whistleLinkageLimitY2 = new(3.1f, 3.8f);
		private static readonly RangeFloat whistleLinkageLimitZ2 = new(-1.25f, 0.1f);

		private static readonly RangeFloat cylinderLLimitX = new(0.77f, 4);
		private static readonly RangeFloat cylinderRLimitX = new(-4, -0.77f);
		private static readonly RangeFloat cylinderLimitY = new(0f, 1.57f);
		private static readonly RangeFloat cylinderLimitZ = new(2.55f, 3.7f);

		private static readonly RangeFloat branchPipeLLimitX = new(0.52f, 1.5f);
		private static readonly RangeFloat branchPipeRLimitX = new(-1.5f, -0.52f);
		private static readonly RangeFloat branchPipeLimitY = new(1.56f, 2.35f);
		private static readonly RangeFloat branchPipeLimitZ = new(2.98f, 3.35f);

		private static readonly RangeFloat dynamoToHeadlightCtrlLimitX = new(0.32f, 0.42f);
		private static readonly RangeFloat dynamoToHeadlightCtrlLimitY = new(3f, 3.1f);
		private static readonly RangeFloat dynamoToHeadlightCtrlLimitZ = new(-1.32f, 3.2f);

		private static readonly RangeFloat dynamoToHeadlightLimitX = new(-0.1f, 0.35f);
		private static readonly RangeFloat dynamoToHeadlightLimitY = new(2.98f, 4f);
		private static readonly RangeFloat dynamoToHeadlightLimitZ = new(3.28f, 3.7f);

		private static readonly RangeFloat boilerJacketLimitX = new(-0.722938f, 0.722938f);
		private static readonly RangeFloat boilerJacketLimitY = new(1.99f, 3.13f);
		private static readonly RangeFloat boilerJacketLimitZ = new(1.4f, 2.73f);
		private static readonly RangeFloat boilerJacketLimitZ2 = new(0.01f, 1.38f);
		private static readonly RangeFloat boilerJacketLimitZ3 = new(-1.33f, 0.01f);

		private static readonly RangeFloat boilerBandLimitX = new(-0.75f, 0.75f);
		private static readonly RangeFloat boilerBandLimitY = new(1.98f, 3.15f);
		private static readonly RangeFloat boilerBandLimitZ = new(1.36f, 1.41f);
		private static readonly RangeFloat boilerBandLimitZ2 = new(0f, 0.1f);

		private static readonly RangeFloat lubricatorLimitX = new(-1.02f, -0.78f);
		private static readonly RangeFloat lubricatorLimitY = new(1.41f, 1.63f);
		private static readonly RangeFloat lubricatorLimitZ = new(2.12f, 2.418f);

		private static readonly RangeFloat equalizerPipeLimitX = new(-2f, 2f);
		private static readonly RangeFloat equalizerPipeLimitY = new(1.29f, 1.63f);
		private static readonly RangeFloat equalizerPipeLimitZ = new(2.43f, 2.66f);

		private static readonly RangeFloat stackLimitX = new(-0.32397f, 0.32397f);
		private static readonly RangeFloat stackLimitY = new(2.8f, 7f);
		//private static readonly RangeFloat stackLimitY = new(2.99f, 7f);
		private static readonly RangeFloat stackLimitZ = new(2.8f, 3.483f);
		private static readonly RangeFloat stackLimitX2 = new(-0.25f, 0.25f);
		private static readonly RangeFloat stackLimitZ2 = new(2.8f, 3.53f);

		private static readonly RangeFloat caliperLLimitX = new(0.41f, 1f);
		private static readonly RangeFloat caliperRLimitX = new(-1f, -0.41f);
		private static readonly RangeFloat caliperLimitY = new(0f, 0.8215f);
		private static readonly RangeFloat caliperLimitZ1 = new(2.16f, 2.59f);
		private static readonly RangeFloat caliperLimitZ2 = new(0.60f, 1.31f);
		private static readonly RangeFloat caliperLimitZ3 = new(-0.91f, -0.57f);

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

			Mesh steamDomeMesh = GetSteamDomeMesh(locoMesh);
			Mesh sandDomeFMesh = GetSandDomeFMesh(locoMesh);
			Mesh sandDomeRMesh = GetSandDomeRMesh(locoMesh);
			Mesh whistleBracketMesh = GetWhistleBracketMesh(locoMesh);
			Mesh rearSubframeLeftMesh = GetRearSubframeLeftMesh(locoMesh);
			Mesh rearSubframeRightMesh = GetRearSubframeRightMesh(locoMesh);
			Mesh smokeboxBoltsMesh = GetSmokeboxBoltsMesh(locoMesh);
			Mesh cylinderLMesh = GetCylinderLMesh(locoMesh);
			Mesh cylinderRMesh = GetCylinderRMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, steamDomeLimitX1, steamDomeLimitY, steamDomeLimitZ1);
			markPartOfMesh(locoMesh.vertices, triangles, steamDomeLimitX2, steamDomeLimitY, steamDomeLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleBracketLimitX, whistleBracketLimitY, whistleBracketLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearSubframeLeftLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearSubframeRightLimitX, rearSubframeLimitY, rearSubframeLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, smokeboxBoltsLimitX, smokeboxBoltsLimitY, smokeboxBoltsLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinderLLimitX, cylinderLimitY, cylinderLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinderRLimitX, cylinderLimitY, cylinderLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh whistleLMesh = GetWhistleLMesh(locoMesh);
			Mesh whistleRMesh = GetWhistleRMesh(locoMesh);
			Mesh bumperMesh = GetBumperMesh(locoMesh);
			Mesh airPumpMesh = GetAirPumpMesh(locoMesh);
			Mesh lubricatorMesh = GetLubricatorMesh(locoMesh);
			Mesh guttersFMesh = GetGuttersFMesh(locoMesh);
			Mesh tankMountUpperMountMesh = GetTankMountUpperMountMesh(locoMesh);
			Mesh tankMountLowerMountMesh = GetTankMountLowerMountMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, whistleLLimitX, whistleLimitY, whistleLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleRLimitX, whistleLimitY, whistleLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bumperLimitX, bumperLimitY, bumperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airPumpLimitX, airPumpLimitY, airPumpLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airPumpLimitX2, airPumpLimitY, airPumpLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, lubricatorLimitX, lubricatorLimitY, lubricatorLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, guttersFLimitX, guttersFLimitY, guttersFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, guttersFLimitX2, guttersFLimitY, guttersFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankMountUpperMountLimitX, tankMountUpperMountLimitY, tankMountUpperMountLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankMountLowerMountLimitX, tankMountLowerMountLimitY, tankMountLowerMountLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankMountLowerMountLimitX2, tankMountLowerMountLimitY, tankMountLowerMountLimitZ2);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh sandPipesFMesh = GetSandPipesFMesh(locoMesh);
			Mesh sandPipesRMesh = GetSandPipesRMesh(locoMesh);
			Mesh handleFrontLeftMesh = GetHandleFrontLeftMesh(locoMesh);
			Mesh tankMountMesh = GetTankMountMesh(locoMesh);
			Mesh airPipes1Mesh = GetAirPipes1Mesh(locoMesh);
			Mesh whistleLinkageMesh = GetWhistleLinkageMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesFLimitY2, sandPipesFLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, sandPipesLimitX, sandPipesLimitY, sandPipesRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handleFrontLeftLimitX, handleFrontLeftLimitY, handleFrontLeftLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankMountLimitX, tankMountLimitY, tankMountLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airPipes1LimitX, airPipes1LimitY, airPipes1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleLinkageLimitX, whistleLinkageLimitY, whistleLinkageLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, whistleLinkageLimitX2, whistleLinkageLimitY2, whistleLinkageLimitZ2);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh tankLMesh = GetTankLMesh(locoMesh);
			Mesh tankRMesh = GetTankRMesh(locoMesh);
			Mesh tankFloorMesh = GetTankFloorMesh(locoMesh);
			Mesh dynamoToHeadlightCtrlMesh = GetDynamoToHeadlightCtrlMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX, tankLimitY, tankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLimitX2, tankLimitY, tankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tank2LimitX, tank2LimitY, tank2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tank2LimitX2, tank2LimitY, tank2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLowRailLimitX, tankLowRailLimitY, tankLowRailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLowRailLimitX2, tankLowRailLimitY, tankLowRailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankFloorLimitX, tankFloorLimitY, tankFloorLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLadderLeftLimitX, tankLadderLimitY, tankLadderLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, tankLadderRightLimitX, tankLadderLimitY, tankLadderLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, dynamoToHeadlightCtrlLimitX, dynamoToHeadlightCtrlLimitY, dynamoToHeadlightCtrlLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh headlightMesh = GetHeadlightMesh(locoMesh);
			Mesh headlightLowerLeftMesh = GetHeadlightLowerLeftMesh(locoMesh);
			Mesh headlightLowerRightMesh = GetHeadlightLowerRightMesh(locoMesh);
			Mesh taillightUpperMesh = GetTaillightUpperMesh(locoMesh);
			Mesh taillightLowerLeftMesh = GetTaillightLowerLeftMesh(locoMesh);
			Mesh taillightLowerRightMesh = GetTaillightLowerRightMesh(locoMesh);
			Mesh bellMesh = GetBellMesh(locoMesh);
			Mesh equalizerPipeMesh = GetEqualizerPipeMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLowerLeftLimitX, headlightLowerLimitY, headlightLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLowerRightLimitX, headlightLowerLimitY, headlightLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightUpperLimitX, taillightUpperLimitY, taillightUpperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightLowerLeftLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, taillightLowerRightLimitX, taillightLowerLimitY, taillightLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bellLimitX, bellLimitY1, bellLimitZ1);
			markPartOfMesh(locoMesh.vertices, triangles, bellLimitX, bellLimitY2, bellLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, equalizerPipeLimitX, equalizerPipeLimitY, equalizerPipeLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh bunkerMesh = GetBunkerMesh(locoMesh);
			Mesh headlightBracketMesh = GetHeadlightBracketMesh(locoMesh);
			Mesh pilotMesh = GetPilotMesh(locoMesh);
			Mesh boilerJacket1Mesh = GetBoilerJacket1Mesh(locoMesh);
			Mesh boilerJacket2Mesh = GetBoilerJacket2Mesh(locoMesh);
			Mesh boilerJacket3Mesh = GetBoilerJacket3Mesh(locoMesh);
			Mesh boilerBand1Mesh = GetBoilerBand1Mesh(locoMesh);
			Mesh boilerBand2Mesh = GetBoilerBand2Mesh(locoMesh);
			Mesh stackMesh = GetStackMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, headlightBracketLimitX, headlightBracketLimitY, headlightBracketLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bunkerUpperLimitX, bunkerUpperLimitY, bunkerUpperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, bunkerLowerLimitX, bunkerLowerLimitY, bunkerLowerLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, rearWallLimitX, rearWallLimitY, rearWallLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, pilotLimitX, pilotLimitY, pilotLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ3);
			markPartOfMesh(locoMesh.vertices, triangles, boilerBandLimitX, boilerBandLimitY, boilerBandLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, boilerBandLimitX, boilerBandLimitY, boilerBandLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, stackLimitX, stackLimitY, stackLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, stackLimitX2, stackLimitY, stackLimitZ2);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh dynamoToHeadlightMesh = GetDynamoToHeadlightMesh(locoMesh);
			Mesh caliperL1Mesh = GetCaliperL1Mesh(locoMesh);
			Mesh caliperL2Mesh = GetCaliperL2Mesh(locoMesh);
			Mesh caliperL3Mesh = GetCaliperL3Mesh(locoMesh);
			Mesh caliperR1Mesh = GetCaliperR1Mesh(locoMesh);
			Mesh caliperR2Mesh = GetCaliperR2Mesh(locoMesh);
			Mesh caliperR3Mesh = GetCaliperR3Mesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, dynamoToHeadlightLimitX, dynamoToHeadlightLimitY, dynamoToHeadlightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ1);
			markPartOfMesh(locoMesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ3);
			markPartOfMesh(locoMesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ1);
			markPartOfMesh(locoMesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ3);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			removeUnusedVertices(locoMesh);

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

			GameObject tankMount = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankMount.GetComponent<MeshFilter>().mesh = tankMountMesh;
			tankMount.name = "s060_tank_mount";

			GameObject tankMountUpperMount = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankMountUpperMount.GetComponent<MeshFilter>().mesh = tankMountUpperMountMesh;
			tankMountUpperMount.name = "s060_tank_mount_upper_mount";

			GameObject tankMountLowerMount = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankMountLowerMount.GetComponent<MeshFilter>().mesh = tankMountLowerMountMesh;
			tankMountLowerMount.name = "s060_tank_mount_lower_mount";

			GameObject tankFloor = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			tankFloor.GetComponent<MeshFilter>().mesh = tankFloorMesh;
			tankFloor.name = "s060_tank_floor";

			GameObject lubricator = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			lubricator.GetComponent<MeshFilter>().mesh = lubricatorMesh;
			lubricator.name = "s060_lubricator";

			GameObject equalizerPipe = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			equalizerPipe.GetComponent<MeshFilter>().mesh = equalizerPipeMesh;
			equalizerPipe.name = "s060_equalizer_pipe";

			GameObject steamDome = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			steamDome.GetComponent<MeshFilter>().mesh = steamDomeMesh;
			steamDome.name = "s060_steam_dome";

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

			GameObject whistleLinkage = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			whistleLinkage.GetComponent<MeshFilter>().mesh = whistleLinkageMesh;
			whistleLinkage.name = "s060_whistle_linkage";

			GameObject bell = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			bell.GetComponent<MeshFilter>().mesh = bellMesh;
			bell.name = "s060_bell";

			GameObject whistleBracket = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			whistleBracket.GetComponent<MeshFilter>().mesh = whistleBracketMesh;
			whistleBracket.name = "s060_whistle_bracket";

			GameObject headlight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlight.GetComponent<MeshFilter>().mesh = headlightMesh;
			headlight.name = "s060_headlight";

			GameObject headlightBracket = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightBracket.GetComponent<MeshFilter>().mesh = headlightBracketMesh;
			headlightBracket.name = "s060_headlight_bracket";

			GameObject dynamoToHeadlight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			dynamoToHeadlight.GetComponent<MeshFilter>().mesh = dynamoToHeadlightMesh;
			dynamoToHeadlight.name = "s060_dynamo_to_headlight";

			GameObject dynamoToHeadlightCtrl = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			dynamoToHeadlightCtrl.GetComponent<MeshFilter>().mesh = dynamoToHeadlightCtrlMesh;
			dynamoToHeadlightCtrl.name = "s060_dynamo_to_headlight_ctrl";

			GameObject headlightLowerLeft = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightLowerLeft.GetComponent<MeshFilter>().mesh = headlightLowerLeftMesh;
			headlightLowerLeft.name = "s060_headlight_lower_left";

			GameObject headlightLowerRight = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightLowerRight.GetComponent<MeshFilter>().mesh = headlightLowerRightMesh;
			headlightLowerRight.name = "s060_headlight_lower_right";

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

			GameObject pilot = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			pilot.GetComponent<MeshFilter>().mesh = pilotMesh;
			pilot.name = "s060_pilot";

			GameObject smokeboxBolts = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			smokeboxBolts.GetComponent<MeshFilter>().mesh = smokeboxBoltsMesh;
			smokeboxBolts.name = "s060_smokebox_bolts";

			GameObject handleFrontLeft = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handleFrontLeft.GetComponent<MeshFilter>().mesh = handleFrontLeftMesh;
			handleFrontLeft.name = "s060_front_left_handle";

			GameObject airPump = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			airPump.GetComponent<MeshFilter>().mesh = airPumpMesh;
			airPump.name = "s060_air_pump";

			GameObject airPipes1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			airPipes1.GetComponent<MeshFilter>().mesh = airPipes1Mesh;
			airPipes1.name = "s060_air_pipes_1";

			GameObject stack = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			stack.GetComponent<MeshFilter>().mesh = stackMesh;
			stack.name = "s060_stack";

			GameObject guttersF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			guttersF.GetComponent<MeshFilter>().mesh = guttersFMesh;
			guttersF.name = "s060_gutters_front";

			GameObject cylinderL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			cylinderL.GetComponent<MeshFilter>().mesh = cylinderLMesh;
			cylinderL.name = "s060_cylinder_left";

			GameObject cylinderR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			cylinderR.GetComponent<MeshFilter>().mesh = cylinderRMesh;
			cylinderR.name = "s060_cylinder_right";

			GameObject boilerJacket1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			boilerJacket1.GetComponent<MeshFilter>().mesh = boilerJacket1Mesh;
			boilerJacket1.name = "s060_boiler_jacket_1";

			GameObject boilerJacket2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			boilerJacket2.GetComponent<MeshFilter>().mesh = boilerJacket2Mesh;
			boilerJacket2.name = "s060_boiler_jacket_2";

			GameObject boilerJacket3 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			boilerJacket3.GetComponent<MeshFilter>().mesh = boilerJacket3Mesh;
			boilerJacket3.name = "s060_boiler_jacket_3";

			GameObject boilerBand1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			boilerBand1.GetComponent<MeshFilter>().mesh = boilerBand1Mesh;
			boilerBand1.name = "s060_boiler_band_1";

			GameObject boilerBand2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			boilerBand2.GetComponent<MeshFilter>().mesh = boilerBand2Mesh;
			boilerBand2.name = "s060_boiler_band_2";

			GameObject caliperR1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperR1.GetComponent<MeshFilter>().mesh = caliperR1Mesh;
			caliperR1.name = "s060_caliper_r_1";

			GameObject caliperR2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperR2.GetComponent<MeshFilter>().mesh = caliperR2Mesh;
			caliperR2.name = "s060_caliper_r_2";

			GameObject caliperR3 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperR3.GetComponent<MeshFilter>().mesh = caliperR3Mesh;
			caliperR3.name = "s060_caliper_r_3";

			GameObject caliperL1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperL1.GetComponent<MeshFilter>().mesh = caliperL1Mesh;
			caliperL1.name = "s060_caliper_l_1";

			GameObject caliperL2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperL2.GetComponent<MeshFilter>().mesh = caliperL2Mesh;
			caliperL2.name = "s060_caliper_l_2";

			GameObject caliperL3 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			caliperL3.GetComponent<MeshFilter>().mesh = caliperL3Mesh;
			caliperL3.name = "s060_caliper_l_3";

			/*GameObject branchPipeL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			branchPipeL.GetComponent<MeshFilter>().mesh = branchPipeLMesh;
			branchPipeL.name = "s060_branch_pipe_left";

			GameObject branchPipeR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			branchPipeR.GetComponent<MeshFilter>().mesh = branchPipeRMesh;
			branchPipeR.name = "s060_branch_pipe_right";*/

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
			markPartOfMesh(tankLMesh.vertices, triangles, tankLadderLeftLimitX, tankLadderLimitY, tankLadderLimitZ);
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
			markPartOfMesh(tankRMesh.vertices, triangles, tankLadderRightLimitX, tankLadderLimitY, tankLadderLimitZ);
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

		private static Mesh GetSteamDomeMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, steamDomeLimitX1, steamDomeLimitY, steamDomeLimitZ1);
			markPartOfMesh(mesh.vertices, triangles, steamDomeLimitX2, steamDomeLimitY, steamDomeLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
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

		private static Mesh GetBellMesh(Mesh s060Mesh)
		{
			Mesh whistleRMesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])whistleRMesh.triangles.Clone();
			markPartOfMesh(whistleRMesh.vertices, triangles, bellLimitX, bellLimitY1, bellLimitZ1);
			markPartOfMesh(whistleRMesh.vertices, triangles, bellLimitX, bellLimitY2, bellLimitZ2);
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

		private static Mesh GetDynamoToHeadlightCtrlMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, dynamoToHeadlightCtrlLimitX, dynamoToHeadlightCtrlLimitY, dynamoToHeadlightCtrlLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetDynamoToHeadlightMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, dynamoToHeadlightLimitX, dynamoToHeadlightLimitY, dynamoToHeadlightLimitZ);
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

		private static Mesh GetHeadlightLowerLeftMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightLowerLeftLimitX, headlightLowerLimitY, headlightLowerLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeadlightLowerRightMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightLowerRightLimitX, headlightLowerLimitY, headlightLowerLimitZ);
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

		private static Mesh GetPilotMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, pilotLimitX, pilotLimitY, pilotLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetSmokeboxBoltsMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, smokeboxBoltsLimitX, smokeboxBoltsLimitY, smokeboxBoltsLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetAirPumpMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, airPumpLimitX, airPumpLimitY, airPumpLimitZ);
			markPartOfMesh(mesh.vertices, triangles, airPumpLimitX2, airPumpLimitY, airPumpLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetAirPipes1Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, airPipes1LimitX, airPipes1LimitY, airPipes1LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandleFrontLeftMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handleFrontLeftLimitX, handleFrontLeftLimitY, handleFrontLeftLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTankMountMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, tankMountLimitX, tankMountLimitY, tankMountLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTankMountUpperMountMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, tankMountUpperMountLimitX, tankMountUpperMountLimitY, tankMountUpperMountLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTankMountLowerMountMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, tankMountLowerMountLimitX, tankMountLowerMountLimitY, tankMountLowerMountLimitZ);
			markPartOfMesh(mesh.vertices, triangles, tankMountLowerMountLimitX2, tankMountLowerMountLimitY, tankMountLowerMountLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetEqualizerPipeMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, equalizerPipeLimitX, equalizerPipeLimitY, equalizerPipeLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetGuttersFMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, guttersFLimitX, guttersFLimitY, guttersFLimitZ);
			markPartOfMesh(mesh.vertices, triangles, guttersFLimitX2, guttersFLimitY, guttersFLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetWhistleLinkageMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, whistleLinkageLimitX, whistleLinkageLimitY, whistleLinkageLimitZ);
			markPartOfMesh(mesh.vertices, triangles, whistleLinkageLimitX2, whistleLinkageLimitY2, whistleLinkageLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCylinderLMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, cylinderLLimitX, cylinderLimitY, cylinderLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCylinderRMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, cylinderRLimitX, cylinderLimitY, cylinderLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBranchPipeLMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, branchPipeLLimitX, branchPipeLimitY, branchPipeLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBranchPipeRMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, branchPipeRLimitX, branchPipeLimitY, branchPipeLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBoilerJacket1Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBoilerJacket2Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBoilerJacket3Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, boilerJacketLimitX, boilerJacketLimitY, boilerJacketLimitZ3);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBoilerBand1Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, boilerBandLimitX, boilerBandLimitY, boilerBandLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetBoilerBand2Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, boilerBandLimitX, boilerBandLimitY, boilerBandLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetStackMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, stackLimitX, stackLimitY, stackLimitZ);
			markPartOfMesh(mesh.vertices, triangles, stackLimitX2, stackLimitY, stackLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetLubricatorMesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, lubricatorLimitX, lubricatorLimitY, lubricatorLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperL1Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ1);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperL2Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperL3Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperLLimitX, caliperLimitY, caliperLimitZ3);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperR1Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ1);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperR2Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCaliperR3Mesh(Mesh s060Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(s060Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, caliperRLimitX, caliperLimitY, caliperLimitZ3);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

	}
}
