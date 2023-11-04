using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;
using UnityEngine;

namespace LocoMeshSplitter.MeshSplitters
{
	internal class S282ALOD1MeshSplitter
	{
		public static GameObject SplitLocoBodyLOD1
		{ get; private set; }

		private static readonly Range[] verticesToMoveRight =
				{
			//Left side:
			//side
			new Range(1861, 1881),
			//top chamfer
			new Range(1935, 1955),
			//bottom chamfer
			new Range(1957, 1977),
			//bottom
			new Range(1834, 1843),
			new Range(1844, 1845),
			//top
			new Range(2274, 2281),
			//small triangles on top
			new Range(2282, 2285),
		};

		private static readonly Range[] verticesToMoveLeft =
		{
			//Right side:
			//side
			new Range(1915, 1935),
			//top chamfer
			//new Range(5733, 5735) // these points are already being shifted, don't need to shift twice
			new Range(5739, 5759),
			//bottom chamfer
			new Range(1892, 1893),
			new Range(1894, 1913),
			//bottom
			new Range(1845, 1848),
			new Range(1849, 1854),
			new Range(1856, 1857),
			new Range(1858, 1859),
			//top
			new Range(5728, 5735),
			//small triangles on top
			new Range(5736, 5739),
		};

		private static readonly int[] trisToHideOnBody =
		{
			/*//hide reach rod support
			11463,
			12106, 12107, 12108, 12109,
			12110, 12111,
			12113, 12114, 12115, 12116, 12117, 12118, 12119,
			12120, 12121, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129,
			12130, 12131, 12132, 12133, 12134, 12135, 12136, 12137, 12138, 12139,
			12140, 12141, 12142, 12143, 12144, 12145, 12146, 12147, 12148, 12149,
			12150, 12151, 12152, 12153, 12154, 12155,*/
			/*//right side #2 brake caliper
			31637,
			31353, 31354, 31355, 31356, 31357, 31358, 31359, 31360, 31361, 31362, 31363, 31364,
			31719, 31720, 31721, 31722, 31723, 31724, 31725, 31726, 31727, 31728,
			31729, 31730, 31731, 31732,
			44266, 44267,
			44270, 44271, 44272, 44273,
			//left side #2 brake caliper
			33593, 33594, 33595, 33596, 33597, 33598, 33599, 33600,
			33959, 33960, 33961, 33962, 33963, 33964, 33965, 33966, 33967, 33968,
			45528, 45529,
			45538, 45539,  45540, 45541,*/
		};

		//brake caliper limits:
		private static readonly RangeFloat brakeCaliperLimitX = new(-.81f, -.30f);
		private static readonly RangeFloat brakeCaliperLimitY = new(0, .66f);
		private static readonly RangeFloat brakeCaliperLimitZ = new(.31f, .74f);
		private static readonly RangeFloat brakeCaliperSandLimitX = new(-.81f, -.31f);
		private static readonly RangeFloat brakeCaliperSandLimitY = new(0, .66f);
		private static readonly RangeFloat brakeCaliperSandLimitZ = new(3.56f, 4);

		//other brake caliper limits, this is just to know what to hide
		private static readonly RangeFloat brakeCaliperLimitXLeft = new(.31f, .81f);
		private static readonly RangeFloat brakeCaliperLimitZ4 = new(-1.24f, -.81f);
		private static readonly RangeFloat brakeCaliperSandLimitZ2 = new(1.87f, 2.22f);
		private static readonly RangeFloat brakeCaliperSandLimitY2a = new(0.26f, .66f);
		private static readonly RangeFloat brakeCaliperSandLimitZ2a = new(1.87f, 2.3f);

		//limits of part of bracket that clips with 4-coupled and 6-coupled
		private static readonly RangeFloat bracketLimitX = new(-1.17f, -0.2f);
		private static readonly RangeFloat bracketLimitX2 = new(0.2f, 1.17f);
		private static readonly RangeFloat bracketLimitY = new(1.44f, 1.57f);
		private static readonly RangeFloat bracketLimitZ = new(0.61f, 0.69f);

		private static readonly RangeFloat bracket2LimitX = new(-1.28f, -0.98f);
		private static readonly RangeFloat bracket2LimitX2 = new(0.98f, 1.28f);
		private static readonly RangeFloat bracket2LimitY = new(1.4f, 1.7f);
		private static readonly RangeFloat bracket2LimitZ = new(0.61f, 2.24f);

		private static readonly RangeFloat bracket3LimitX = new(-0.99f, -0.2f);
		private static readonly RangeFloat bracket3LimitX2 = new(0.2f, 0.99f);
		private static readonly RangeFloat bracket3LimitY = new(1.62f, 1.7f);
		private static readonly RangeFloat bracket3LimitZ = new(1.77f, 1.85f);

		//4, 5, and 6 are the vertical part of the bracket
		private static readonly RangeFloat bracket4LimitX = new(-1.35f, -0.134f);
		private static readonly RangeFloat bracket4LimitX2 = new(0.134f, 1.35f);
		private static readonly RangeFloat bracket4LimitY = new(1.09f, 1.59f);
		private static readonly RangeFloat bracket4LimitZ = new(2.22f, 2.34f);

		private static readonly RangeFloat bracket5LimitX = new(-1.35f, -1.03f);
		private static readonly RangeFloat bracket5LimitX2 = new(1.03f, 1.35f);
		private static readonly RangeFloat bracket5LimitY = new(0.11f, 1.59f);
		private static readonly RangeFloat bracket5LimitZ = new(2.22f, 2.34f);

		private static readonly RangeFloat bracket6LimitX = new(-1.1f, 1.1f);
		private static readonly RangeFloat bracket6LimitY = new(0.1f, 0.3f);
		private static readonly RangeFloat bracket6LimitZ = new(2.22f, 2.34f);

		private static readonly RangeFloat crossheadGuideLimitX = new(-1.11f, -1.07f);
		private static readonly RangeFloat crossheadGuideLimitX2 = new(1.07f, 1.11f);
		private static readonly RangeFloat crossheadGuideLimitY = new(0.39f, 1.03f);
		private static readonly RangeFloat crossheadGuideLimitZ = new(2.19f, 4.04f);

		private static readonly RangeFloat cylinderLimitX = new(-1.5f, -0.74f);
		private static readonly RangeFloat cylinderLimitX2 = new(0.74f, 1.5f);
		private static readonly RangeFloat cylinderLimitY = new(0, 1.7f);
		private static readonly RangeFloat cylinderLimitZ = new(4.15f, 5.35f);

		private static readonly RangeFloat cylinder2LimitX = new(-1.5f, -0.74f);
		private static readonly RangeFloat cylinder2LimitX2 = new(0.74f, 1.5f);
		private static readonly RangeFloat cylinder2LimitY = new(0, 1.63f);
		private static readonly RangeFloat cylinder2LimitZ = new(3.92f, 4.18f);

		private static readonly RangeFloat cylinder3LimitX = new(-1.15f, -1.07f);
		private static readonly RangeFloat cylinder3LimitX2 = new(1.07f, 1.15f);
		private static readonly RangeFloat cylinder3LimitY = new(0.32f, 0.36f);
		private static readonly RangeFloat cylinder3LimitZ = new(4.1f, 5.25f);

		private static readonly RangeFloat stackLimitX = new(-0.5f, 0.5f);
		private static readonly RangeFloat stackLimitY = new(3.15f, 4.2f);
		private static readonly RangeFloat stackLimitZ = new(4.4f, 5.4f);

		private static readonly RangeFloat whistleLimitX = new(-0.5f, -0.2f);
		private static readonly RangeFloat whistleLimitY = new(3.3f, 4f);
		private static readonly RangeFloat whistleLimitZ = new(-0.51f, -0.2f);

		private static readonly RangeFloat bellLimitX = new(-0.251f, 0.251f);
		private static readonly RangeFloat bellLimitY = new(3.3f, 4f);
		private static readonly RangeFloat bellLimitZ = new(0.3f, 0.7f);

		private static readonly RangeFloat bellAirLine1LimitX = new(0.23f, 1f);
		private static readonly RangeFloat bellAirLine1LimitY = new(2.1f, 4f);
		private static readonly RangeFloat bellAirLine1LimitZ = new(0.2f, 0.5f);

		private static readonly RangeFloat bellAirLine2LimitX = new(0.7f, 0.88f);
		private static readonly RangeFloat bellAirLine2LimitY = new(2.19f, 2.3f);
		private static readonly RangeFloat bellAirLine2LimitZ = new(-4.1f, 0.21f);

		private static readonly RangeFloat sandDomeLimitX = new(-0.71f, 0.71f);
		private static readonly RangeFloat sandDomeLimitY = new(3.06f, 4f);
		private static readonly RangeFloat sandDomeLimitZ = new(1.54f, 3.06f);

		private static readonly RangeFloat runningBoardsLimitX = new(-1.2f, 1.2f);
		private static readonly RangeFloat runningBoardsLimitY = new(2.18f, 2.24f);
		private static readonly RangeFloat runningBoardsLimitZ = new(-4.02f, 5.7f);

		private static readonly RangeFloat miscAirFittingsLLimitX = new(0, 1);
		private static readonly RangeFloat miscAirFittingsLLimitY = new(2.1f, 2.3f);
		private static readonly RangeFloat miscAirFittingsLLimitZ = new(4.79f, 5.28f);

		private static readonly RangeFloat runningBoardSupportLimitX = new(-1.2f, 1.2f);
		/*private static readonly RangeFloat runningBoardSupportLLimitX = new(-1.2f, -0.6f);
		private static readonly RangeFloat runningBoardSupportRLimitX = new(0.6f, 1.2f);*/
		private static readonly RangeFloat runningBoardSupportLimitY = new(1.9f, 2.2f);
		private static readonly RangeFloat runningBoardSupport1LimitZ = new(5.2f, 5.4f);
		private static readonly RangeFloat runningBoardSupport2LimitZ = new(3.0f, 3.2f);
		private static readonly RangeFloat runningBoardSupport3LimitZ = new(1.3f, 1.5f);
		private static readonly RangeFloat runningBoardSupport4LimitZ = new(-0.3f, 0.02f);
		private static readonly RangeFloat runningBoardSupport5LimitZ = new(-1.8f, -1.6f);
		private static readonly RangeFloat runningBoardSupport6LimitZ = new(-3.9f, -3.7f);

		private static readonly RangeFloat dryPipeLLimitX = new(0.6f, 1.2f);
		private static readonly RangeFloat dryPipeRLimitX = new(-1.2f, -0.6f);
		private static readonly RangeFloat dryPipeLimitY = new(1.5f, 2.5f);
		private static readonly RangeFloat dryPipeLimitZ = new(4.45f, 4.78f);

		private static readonly RangeFloat stepsLLimitX = new(0.8f, 1.2f);
		private static readonly RangeFloat stepsRLimitX = new(-1.2f, -0.8f);
		private static readonly RangeFloat stepsLimitY = new(1, 2.2f);
		private static readonly RangeFloat stepsLimitZ = new(5.4f, 6);

		private static readonly RangeFloat handrailLLimitX = new(0.66f, 1.2f);
		private static readonly RangeFloat handrailRLimitX = new(-1.2f, -0.66f);
		private static readonly RangeFloat handrailLimitY = new(3.2f, 3.4f);
		private static readonly RangeFloat handrailLimitZ = new(-3.9f, 5.6f);
		private static readonly RangeFloat handrailSupportLLimitX = new(0.58f, 1.2f);
		private static readonly RangeFloat handrailSupportRLimitX = new(-1.2f, -0.58f);
		private static readonly RangeFloat handrailSupportLimitY = new(3.1f, 3.4f);
		private static readonly RangeFloat handrailSupport1LimitZ = new(3.2f, 5.6f);
		private static readonly RangeFloat handrailSupport2LimitZ = new(0.8f, 1.0f);
		private static readonly RangeFloat handrailSupport3LimitZ = new(-1.5f, -1.4f);
		private static readonly RangeFloat handrailSupport4LimitZ = new(-3.9f, -3.8f);

		private static readonly RangeFloat frontBoilerSupportLimit1X = new(-1.1f, -0.3f);
		private static readonly RangeFloat frontBoilerSupportLimit2X = new(-1.1f, 1.1f);
		private static readonly RangeFloat frontBoilerSupportLimit3X = new(0.3f, 1.1f);
		private static readonly RangeFloat frontBoilerSupportLimit1Y = new(1f, 2f);
		private static readonly RangeFloat frontBoilerSupportLimit2Y = new(1.11f, 2f);
		private static readonly RangeFloat frontBoilerSupportLimitZ = new(5.6f, 5.675f);

		private static readonly RangeFloat pilotLimit1X = new(-1.2f, -0.4f);
		private static readonly RangeFloat pilotLimit2X = new(0.4f, 1.2f);
		private static readonly RangeFloat pilotLimit3X = new(-1.2f, 1.2f);
		private static readonly RangeFloat pilotLimit1Y = new(0, 1.04f);
		private static readonly RangeFloat pilotLimit3Y = new(0, 1.2f);
		private static readonly RangeFloat pilotLimit1Z = new(4.9f, 8f);
		private static readonly RangeFloat pilotLimit3Z = new(6.2f, 8f);

		private static readonly RangeFloat toolboxLimitX = new(-0.38f, 0.38f);
		private static readonly RangeFloat toolboxLimitY = new(1.005f, 1.4f);
		private static readonly RangeFloat toolboxLimitZ = new(5.66f, 6.28f);
		private static readonly RangeFloat toolbox2LimitY = new(0.99f, 1.02f);
		private static readonly RangeFloat toolbox2LimitZ = new(6.17f, 6.28f);

		private static readonly RangeFloat frontHandrailLimitX = new(-1f, 1f);
		private static readonly RangeFloat frontHandrailLimitY = new(1.02f, 2.7f);
		private static readonly RangeFloat frontHandrailLimitZ = new(6.0f, 6.2f);
		private static readonly RangeFloat frontHandrailLimit2Y = new(2.5f, 2.7f);
		private static readonly RangeFloat frontHandrailLimit2Z = new(5.0f, 6.2f);

		private static readonly RangeFloat airPumpLimitX = new(-0.77f, 0f);
		private static readonly RangeFloat airPumpLimitY = new(1f, 2f);
		private static readonly RangeFloat airPumpLimitZ = new(5f, 5.6f);

		private static readonly RangeFloat cabLimitX = new(-1.6f, 1.6f);
		private static readonly RangeFloat cabLimitY = new(1.25f, 4.73f);
		private static readonly RangeFloat cabLimitZ = new(-7f, -4.02f);
		private static readonly RangeFloat cabRoofLimitX = new(-1.6f, 1.6f);
		private static readonly RangeFloat cabRoofLimitY = new(3.3f, 5f);
		private static readonly RangeFloat cabRoofLimitZ = new(-7f, -3.9f);

		private static readonly RangeFloat reachRodSupportLimitX = new(-0.8f, -0.1f);
		private static readonly RangeFloat reachRodSupportLimitY = new(1.6f, 2.0f);
		private static readonly RangeFloat reachRodSupportLimitZ = new(0.9f, 1.2f);

		private static readonly RangeFloat lubricatorLimitX = new(-1.2f, -1.02f);
		private static readonly RangeFloat lubricatorLimitY = new(1.79f, 2.01f);
		private static readonly RangeFloat lubricatorLimitZ = new(3.78f, 4.07f);

		private static readonly RangeFloat lubricatorSupportLimitX = new(-1.2f, -1.005f);
		private static readonly RangeFloat lubricatorSupportLimitY = new(1.6f, 1.8f);
		private static readonly RangeFloat lubricatorSupportLimitZ = new(3.82f, 4.07f);

		private static readonly RangeFloat oilLinesRLimitX = new(-1.2f, -0.2f);
		private static readonly RangeFloat oilLinesRLimitY = new(1.6f, 1.86f);
		private static readonly RangeFloat oilLinesRLimitZ = new(3.74f, 3.86f);

		private static readonly RangeFloat oilLinesFLimitX = new(-1.2f, -0.82f);
		private static readonly RangeFloat oilLinesFLimitY = new(1.4f, 1.86f);
		private static readonly RangeFloat oilLinesFLimitZ = new(4.06f, 4.14f);

		private static readonly RangeFloat airTankLLimitX = new(0.59f, 1f);
		private static readonly RangeFloat airTankRLimitX = new(-1f, -0.59f);
		private static readonly RangeFloat airTankLimitY = new(1.63f, 2.04f);
		private static readonly RangeFloat airTankLimitZ = new(2.05f, 4.07f);

		private static readonly RangeFloat airPumpOutputPipeLimitX = new(-0.82f, -0.54f);
		private static readonly RangeFloat airPumpOutputPipeLimitY = new(1f, 1.9f);
		private static readonly RangeFloat airPumpOutputPipeLimitZ = new(4.04f, 5.13f);

		private static readonly RangeFloat airPumpInputPipeLimitX = new(-0.6f, -0.51f);
		private static readonly RangeFloat airPumpInputPipeLimitY = new(1.9f, 2.0f);
		private static readonly RangeFloat airPumpInputPipeLimitZ = new(3.6f, 4.798f);
		private static readonly RangeFloat airPumpInputPipeLimit2X = new(-1f, -0.51f);
		private static readonly RangeFloat airPumpInputPipeLimit2Y = new(1.72f, 2.44f);
		private static readonly RangeFloat airPumpInputPipeLimit2Z = new(4.7f, 5.27f);

		private static readonly RangeFloat airTankSupportLLimitX = new(0.57f, 1.02f);
		private static readonly RangeFloat airTankSupportRLimitX = new(-1.02f, -0.57f);
		private static readonly RangeFloat airTankSupportLimitY = new(1.61f, 2.2f);
		private static readonly RangeFloat airTankSupportFLimitZ = new(3.811f, 3.88f);
		private static readonly RangeFloat airTankSupportRLimitZ = new(2.24f, 2.31f);

		internal static void Init()
		{
			SplitLocoBodyLOD1 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoBodyLOD1);
		}

		//Splits the S282 mesh into its parts.
		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh("s282_locomotive_body_LOD1");
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the s282_locomotive_body_LOD1 mesh");
				return null;
			}
			Main.Logger.Log($"Splitting s282_locomotive_body_LOD1 mesh...");

			GameObject SplitLocoBodyLOD1 = new GameObject("s282a_split_body_LOD1");
			SplitLocoBodyLOD1.SetActive(false);

			//Alter mesh so that the firebox doesn't clip through the drivers
			Vector3[] s282Vertices = locoMesh.vertices;
			int[] s282Tris = locoMesh.triangles;
			foreach (Range range in verticesToMoveRight)
			{
				for (int i = range.start; i < range.end; i++)
				{
					s282Vertices[i].x -= 0.2f;
				}
			}
			foreach (Range range in verticesToMoveLeft)
			{
				for (int i = range.start; i < range.end; i++)
				{
					s282Vertices[i].x += 0.2f;
				}
			}
			/*foreach (int tri in trisToHideOnBody)
			{
				s282Tris[tri * 3] = -1;
				s282Tris[tri * 3 + 1] = -1;
				s282Tris[tri * 3 + 2] = -1;
			}*/
			s282Tris = s282Tris.Where((source, index) => source != -1).ToArray();
			locoMesh.vertices = s282Vertices;
			locoMesh.triangles = s282Tris;

			//TODO: convert hidePartOfMesh() calls into markPartOfMesh() calls

			Mesh caliperMesh = getCaliperMesh(locoMesh);
			Mesh caliperSandMesh = getCaliperSandMesh(locoMesh);
			Mesh stackMesh = getStackMesh(locoMesh);
			Mesh whistleMesh = getWhistleMesh(locoMesh);
			Mesh bellMesh = getBellMesh(locoMesh);
			Mesh bellAirLineMesh = getBellAirLineMesh(locoMesh);
			Mesh sandDomeMesh = getSandDomeMesh(locoMesh);

			hidePartOfMesh(locoMesh, stackLimitX, stackLimitY, stackLimitZ);
			hidePartOfMesh(locoMesh, whistleLimitX, whistleLimitY, whistleLimitZ);
			hidePartOfMesh(locoMesh, sandDomeLimitX, sandDomeLimitY, sandDomeLimitZ);
			hidePartOfMesh(locoMesh, bellLimitX, bellLimitY, bellLimitZ);
			hidePartOfMesh(locoMesh, bellAirLine1LimitX, bellAirLine1LimitY, bellAirLine1LimitZ);
			hidePartOfMesh(locoMesh, bellAirLine2LimitX, bellAirLine2LimitY, bellAirLine2LimitZ);

			Mesh runningBoardsMesh = getRunningBoardsMesh(locoMesh);
			hidePartOfMesh(locoMesh, runningBoardsLimitX, runningBoardsLimitY, runningBoardsLimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport1LimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport2LimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport3LimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport4LimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport5LimitZ);
			hidePartOfMesh(locoMesh, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport6LimitZ);
			hidePartOfMesh(locoMesh, miscAirFittingsLLimitX, miscAirFittingsLLimitY, miscAirFittingsLLimitZ);

			Mesh dryPipeLMesh = getDryPipeLMesh(locoMesh);
			Mesh dryPipeRMesh = getDryPipeRMesh(locoMesh);
			hidePartOfMesh(locoMesh, dryPipeLLimitX, dryPipeLimitY, dryPipeLimitZ);
			hidePartOfMesh(locoMesh, dryPipeRLimitX, dryPipeLimitY, dryPipeLimitZ);

			Mesh stepsLMesh = getStepsLMesh(locoMesh);
			Mesh stepsRMesh = getStepsRMesh(locoMesh);
			hidePartOfMesh(locoMesh, stepsLLimitX, stepsLimitY, stepsLimitZ);
			hidePartOfMesh(locoMesh, stepsRLimitX, stepsLimitY, stepsLimitZ);

			Mesh handrailLMesh = getHandrailLMesh(locoMesh);
			Mesh handrailRMesh = getHandrailRMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, handrailLLimitX, handrailLimitY, handrailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailRLimitX, handrailLimitY, handrailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport3LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport4LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport3LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport4LimitZ);

			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitX, brakeCaliperLimitY, brakeCaliperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitX, brakeCaliperLimitY, brakeCaliperLimitZ4);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitX, brakeCaliperLimitY, brakeCaliperSandLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitX, brakeCaliperLimitY, brakeCaliperSandLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitX, brakeCaliperSandLimitY2a, brakeCaliperSandLimitZ2a);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitXLeft, brakeCaliperLimitY, brakeCaliperLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitXLeft, brakeCaliperLimitY, brakeCaliperLimitZ4);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitXLeft, brakeCaliperLimitY, brakeCaliperSandLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitXLeft, brakeCaliperLimitY, brakeCaliperSandLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, brakeCaliperLimitXLeft, brakeCaliperSandLimitY2a, brakeCaliperSandLimitZ2a);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh lubricatorMesh = getLubricatorMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, lubricatorLimitX, lubricatorLimitY, lubricatorLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh lubricatorSupportMesh = getLubricatorSupportMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, lubricatorSupportLimitX, lubricatorSupportLimitY, lubricatorSupportLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh airTankMesh = getAirTankMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, airTankSupportRLimitX, airTankSupportLimitY, airTankSupportFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airTankSupportRLimitX, airTankSupportLimitY, airTankSupportRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airTankSupportLLimitX, airTankSupportLimitY, airTankSupportFLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airTankSupportLLimitX, airTankSupportLimitY, airTankSupportRLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airTankRLimitX, airTankLimitY, airTankLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airTankLLimitX, airTankLimitY, airTankLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh oilLinesRMesh = getOilLinesRMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, oilLinesRLimitX, oilLinesRLimitY, oilLinesRLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh oilLinesFMesh = getOilLinesFMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, oilLinesFLimitX, oilLinesFLimitY, oilLinesFLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh cylinderLMesh = getCylinderLMesh(locoMesh);
			Mesh cylinderRMesh = getCylinderRMesh(locoMesh);

			Mesh crossheadSupportLMesh = getCrossheadSupportLMesh(locoMesh);
			Mesh crossheadSupportRMesh = getCrossheadSupportRMesh(locoMesh);

			Mesh liftingArmSupportLMesh = getLiftingArmSupportLMesh(locoMesh);
			Mesh liftingArmSupportRMesh = getLiftingArmSupportRMesh(locoMesh);

			hidePartOfMesh(locoMesh, bracketLimitX, bracketLimitY, bracketLimitZ);
			hidePartOfMesh(locoMesh, bracketLimitX2, bracketLimitY, bracketLimitZ);
			hidePartOfMesh(locoMesh, bracket2LimitX, bracket2LimitY, bracket2LimitZ);
			hidePartOfMesh(locoMesh, bracket2LimitX2, bracket2LimitY, bracket2LimitZ);
			hidePartOfMesh(locoMesh, bracket3LimitX, bracket3LimitY, bracket3LimitZ);
			hidePartOfMesh(locoMesh, bracket3LimitX2, bracket3LimitY, bracket3LimitZ);

			hidePartOfMesh(locoMesh, bracket4LimitX, bracket4LimitY, bracket4LimitZ);
			hidePartOfMesh(locoMesh, bracket4LimitX2, bracket4LimitY, bracket4LimitZ);
			hidePartOfMesh(locoMesh, bracket5LimitX, bracket5LimitY, bracket5LimitZ);
			hidePartOfMesh(locoMesh, bracket5LimitX2, bracket5LimitY, bracket5LimitZ);
			hidePartOfMesh(locoMesh, bracket6LimitX, bracket6LimitY, bracket6LimitZ);

			hidePartOfMesh(locoMesh, crossheadGuideLimitX, crossheadGuideLimitY, crossheadGuideLimitZ);
			hidePartOfMesh(locoMesh, crossheadGuideLimitX2, crossheadGuideLimitY, crossheadGuideLimitZ);

			//hide cylinders
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, cylinderLimitX, cylinderLimitY, cylinderLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinderLimitX2, cylinderLimitY, cylinderLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinder2LimitX, cylinder2LimitY, cylinder2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinder2LimitX2, cylinder2LimitY, cylinder2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinder3LimitX, cylinder3LimitY, cylinder3LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cylinder3LimitX2, cylinder3LimitY, cylinder3LimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh frontBoilerSupportMesh = getFrontBoilerSupportMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, frontBoilerSupportLimit1X, frontBoilerSupportLimit1Y, frontBoilerSupportLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, frontBoilerSupportLimit2X, frontBoilerSupportLimit2Y, frontBoilerSupportLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, frontBoilerSupportLimit3X, frontBoilerSupportLimit1Y, frontBoilerSupportLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh pilotMesh = getPilotMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, pilotLimit1X, pilotLimit1Y, pilotLimit1Z);
			markPartOfMesh(locoMesh.vertices, triangles, pilotLimit2X, pilotLimit1Y, pilotLimit1Z);
			markPartOfMesh(locoMesh.vertices, triangles, pilotLimit3X, pilotLimit3Y, pilotLimit3Z);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh toolboxMesh = getToolboxMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, toolboxLimitX, toolboxLimitY, toolboxLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, toolboxLimitX, toolbox2LimitY, toolbox2LimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh frontHandrailMesh = getFrontHandrailMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, frontHandrailLimitX, frontHandrailLimitY, frontHandrailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, frontHandrailLimitX, frontHandrailLimit2Y, frontHandrailLimit2Z);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh airPumpMesh = getAirPumpMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, airPumpLimitX, airPumpLimitY, airPumpLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh airPumpOutputPipeMesh = getAirPumpOutputPipeMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, airPumpOutputPipeLimitX, airPumpOutputPipeLimitY, airPumpOutputPipeLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh airPumpInputPipeMesh = getAirPumpInputPipeMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, airPumpInputPipeLimitX, airPumpInputPipeLimitY, airPumpInputPipeLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, airPumpInputPipeLimit2X, airPumpInputPipeLimit2Y, airPumpInputPipeLimit2Z);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh cabMesh = getCabMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, cabLimitX, cabLimitY, cabLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cabRoofLimitX, cabRoofLimitY, cabRoofLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh reachRodSupportMesh = getReachRodSupportMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, reachRodSupportLimitX, reachRodSupportLimitY, reachRodSupportLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			locoMesh.RecalculateNormals();
			locoMesh.RecalculateTangents();
			locoMesh.RecalculateBounds();

			GameObject splitLoco = new GameObject("s282a_body");
			splitLoco.transform.SetParent(SplitLocoBodyLOD1.transform);
			splitLoco.transform.localScale = new Vector3(-1, 1, 1);
			splitLoco.AddComponent<MeshFilter>().mesh = locoMesh;
			splitLoco.AddComponent<MeshRenderer>();

			GameObject cylinderL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			cylinderL.GetComponent<MeshFilter>().mesh = cylinderLMesh;
			cylinderL.name = "s282a_cylinder_l";

			GameObject cylinderR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			cylinderR.GetComponent<MeshFilter>().mesh = cylinderRMesh;
			cylinderR.name = "s282a_cylinder_r";

			GameObject dryPipeL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			dryPipeL.GetComponent<MeshFilter>().mesh = dryPipeLMesh;
			dryPipeL.name = "s282a_dry_pipe_l";

			GameObject dryPipeR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			dryPipeR.GetComponent<MeshFilter>().mesh = dryPipeRMesh;
			dryPipeR.name = "s282a_dry_pipe_r";

			GameObject crossheadSupportL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			crossheadSupportL.GetComponent<MeshFilter>().mesh = crossheadSupportLMesh;
			crossheadSupportL.name = "s282a_crosshead_guide_l";

			GameObject crossheadSupportR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			crossheadSupportR.GetComponent<MeshFilter>().mesh = crossheadSupportRMesh;
			crossheadSupportR.name = "s282a_crosshead_guide_r";

			GameObject liftingArmSupportL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			liftingArmSupportL.GetComponent<MeshFilter>().mesh = liftingArmSupportLMesh;
			liftingArmSupportL.name = "s282a_lifting_arm_support_l";

			GameObject liftingArmSupportR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			liftingArmSupportR.GetComponent<MeshFilter>().mesh = liftingArmSupportRMesh;
			liftingArmSupportR.name = "s282a_lifting_arm_support_r";

			GameObject stepsL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			stepsL.GetComponent<MeshFilter>().mesh = stepsLMesh;
			stepsL.name = "s282a_steps_l";

			GameObject stepsR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			stepsR.GetComponent<MeshFilter>().mesh = stepsRMesh;
			stepsR.name = "s282a_steps_r";

			GameObject frontHandrail = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			frontHandrail.GetComponent<MeshFilter>().mesh = frontHandrailMesh;
			frontHandrail.name = "s282a_handrail_f";

			GameObject handrailL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			handrailL.GetComponent<MeshFilter>().mesh = handrailLMesh;
			handrailL.name = "s282a_handrail_l";

			GameObject handrailR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			handrailR.GetComponent<MeshFilter>().mesh = handrailRMesh;
			handrailR.name = "s282a_handrail_r";

			GameObject runningBoards = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			runningBoards.GetComponent<MeshFilter>().mesh = runningBoardsMesh;
			runningBoards.name = "s282a_running_boards";

			GameObject frontBoilerSupport = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			frontBoilerSupport.GetComponent<MeshFilter>().mesh = frontBoilerSupportMesh;
			frontBoilerSupport.name = "s282a_front_boiler_support";

			GameObject pilot = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			pilot.GetComponent<MeshFilter>().mesh = pilotMesh;
			pilot.name = "s282a_pilot";

			GameObject toolbox = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			toolbox.GetComponent<MeshFilter>().mesh = toolboxMesh;
			toolbox.name = "s282a_toolbox";

			GameObject airPump = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			airPump.GetComponent<MeshFilter>().mesh = airPumpMesh;
			airPump.name = "s282a_air_pump";

			GameObject stack = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			stack.GetComponent<MeshFilter>().mesh = stackMesh;
			stack.name = "s282a_stack";

			GameObject whistle = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			whistle.GetComponent<MeshFilter>().mesh = whistleMesh;
			whistle.name = "s282a_whistle";

			GameObject bell = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			bell.GetComponent<MeshFilter>().mesh = bellMesh;
			bell.name = "s282a_bell";

			GameObject bellAirLine = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			bellAirLine.GetComponent<MeshFilter>().mesh = bellAirLineMesh;
			bellAirLine.name = "s282a_bell_air_line";

			GameObject sandDome = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			sandDome.GetComponent<MeshFilter>().mesh = sandDomeMesh;
			sandDome.name = "s282a_sand_dome";

			GameObject cab = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			cab.GetComponent<MeshFilter>().mesh = cabMesh;
			cab.name = "s282a_cab";

			GameObject reachRodSupport = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			reachRodSupport.GetComponent<MeshFilter>().mesh = reachRodSupportMesh;
			reachRodSupport.name = "s282a_reach_rod_support";

			GameObject lubricator = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			lubricator.GetComponent<MeshFilter>().mesh = lubricatorMesh;
			lubricator.name = "s282a_lubricator";

			GameObject lubricatorSupport = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			lubricatorSupport.GetComponent<MeshFilter>().mesh = lubricatorSupportMesh;
			lubricatorSupport.name = "s282a_lubricator_support";

			GameObject oilLinesR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			oilLinesR.GetComponent<MeshFilter>().mesh = oilLinesRMesh;
			oilLinesR.name = "s282a_oil_lines_r";

			GameObject oilLinesF = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			oilLinesF.GetComponent<MeshFilter>().mesh = oilLinesFMesh;
			oilLinesF.name = "s282a_oil_lines_f";

			GameObject airTankL = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			airTankL.GetComponent<MeshFilter>().mesh = airTankMesh;
			airTankL.name = "s282a_air_tank_l";
			airTankL.transform.localScale = Vector3.one;

			GameObject airTankR = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			airTankR.GetComponent<MeshFilter>().mesh = airTankMesh;
			airTankR.name = "s282a_air_tank_r";

			GameObject airPumpInputPipe = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			airPumpInputPipe.GetComponent<MeshFilter>().mesh = airPumpInputPipeMesh;
			airPumpInputPipe.name = "s282a_air_pump_input_pipe";

			GameObject airPumpOutputPipe = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			airPumpOutputPipe.GetComponent<MeshFilter>().mesh = airPumpOutputPipeMesh;
			airPumpOutputPipe.name = "s282a_air_pump_output_pipe";

			GameObject caliperR1 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperR1.GetComponent<MeshFilter>().mesh = caliperSandMesh;
			caliperR1.name = "s282a_caliper_r_1";
			caliperR1.transform.localPosition = new Vector3(0, 0.06f, 0.04f);

			GameObject caliperR2 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperR2.GetComponent<MeshFilter>().mesh = caliperSandMesh;
			caliperR2.name = "s282a_caliper_r_2";
			caliperR2.transform.localPosition = new Vector3(0, 0.06f, -1.67f);

			GameObject caliperR3 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperR3.GetComponent<MeshFilter>().mesh = caliperMesh;
			caliperR3.name = "s282a_caliper_r_3";
			caliperR3.transform.localPosition = new Vector3(0, 0.07f, 0.03f);

			GameObject caliperR4 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperR4.GetComponent<MeshFilter>().mesh = caliperMesh;
			caliperR4.name = "s282a_caliper_r_4";
			caliperR4.transform.localPosition = new Vector3(0, 0.07f, -1.52f);

			GameObject caliperL1 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperL1.GetComponent<MeshFilter>().mesh = caliperSandMesh;
			caliperL1.name = "s282a_caliper_l_1";
			caliperL1.transform.localScale = Vector3.one;
			caliperL1.transform.localPosition = new Vector3(0, 0.06f, 0.04f);

			GameObject caliperL2 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperL2.GetComponent<MeshFilter>().mesh = caliperSandMesh;
			caliperL2.name = "s282a_caliper_l_2";
			caliperL2.transform.localScale = Vector3.one;
			caliperL2.transform.localPosition = new Vector3(0, 0.06f, -1.67f);

			GameObject caliperL3 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperL3.GetComponent<MeshFilter>().mesh = caliperMesh;
			caliperL3.name = "s282a_caliper_l_3";
			caliperL3.transform.localScale = Vector3.one;
			caliperL3.transform.localPosition = new Vector3(0, 0.07f, 0.03f);

			GameObject caliperL4 = UnityEngine.Object.Instantiate(splitLoco, SplitLocoBodyLOD1.transform);
			caliperL4.GetComponent<MeshFilter>().mesh = caliperMesh;
			caliperL4.name = "s282a_caliper_l_4";
			caliperL4.transform.localScale = Vector3.one;
			caliperL4.transform.localPosition = new Vector3(0, 0.07f, -1.52f);

			Main.Logger.Log("Split S282A mesh.");
			return SplitLocoBodyLOD1;
		}

		private static Mesh getCylinderLMesh(Mesh s282Mesh)
		{
			Mesh cylinderMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])cylinderMesh.triangles.Clone();
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinderLimitX2, cylinderLimitY, cylinderLimitZ);
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinder2LimitX2, cylinder2LimitY, cylinder2LimitZ);
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinder3LimitX2, cylinder3LimitY, cylinder3LimitZ);
			deleteUnmarkedPartOfMesh(cylinderMesh, triangles);
			return cylinderMesh;
		}

		private static Mesh getCylinderRMesh(Mesh s282Mesh)
		{
			Mesh cylinderMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])cylinderMesh.triangles.Clone();
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinderLimitX, cylinderLimitY, cylinderLimitZ);
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinder2LimitX, cylinder2LimitY, cylinder2LimitZ);
			markPartOfMesh(cylinderMesh.vertices, triangles, cylinder3LimitX, cylinder3LimitY, cylinder3LimitZ);
			deleteUnmarkedPartOfMesh(cylinderMesh, triangles);
			return cylinderMesh;
		}

		private static Mesh getCrossheadSupportLMesh(Mesh s282Mesh)
		{
			Mesh crossheadBracketMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])crossheadBracketMesh.triangles.Clone();
			//markPartOfMesh(CrossheadBracketMesh.vertices, triangles, bracket2LimitX, bracket2LimitY, bracket2LimitZ);
			//markPartOfMesh(CrossheadBracketMesh.vertices, triangles, bracket3LimitX, bracket3LimitY, bracket3LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, bracket4LimitX2, bracket4LimitY, bracket4LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, bracket5LimitX2, bracket5LimitY, bracket5LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, new(-0.14f, 1.1f), bracket6LimitY, bracket6LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, crossheadGuideLimitX2, crossheadGuideLimitY, crossheadGuideLimitZ);
			deleteUnmarkedPartOfMesh(crossheadBracketMesh, triangles);
			return crossheadBracketMesh;
		}

		private static Mesh getCrossheadSupportRMesh(Mesh s282Mesh)
		{
			Mesh crossheadBracketMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])crossheadBracketMesh.triangles.Clone();
			//markPartOfMesh(CrossheadBracketMesh.vertices, triangles, bracket2LimitX, bracket2LimitY, bracket2LimitZ);
			//markPartOfMesh(CrossheadBracketMesh.vertices, triangles, bracket3LimitX, bracket3LimitY, bracket3LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, bracket4LimitX, bracket4LimitY, bracket4LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, bracket5LimitX, bracket5LimitY, bracket5LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, new(-1.1f, 0.14f), bracket6LimitY, bracket6LimitZ);
			markPartOfMesh(crossheadBracketMesh.vertices, triangles, crossheadGuideLimitX, crossheadGuideLimitY, crossheadGuideLimitZ);
			deleteUnmarkedPartOfMesh(crossheadBracketMesh, triangles);
			return crossheadBracketMesh;
		}

		private static Mesh getLiftingArmSupportLMesh(Mesh s282Mesh)
		{
			Mesh liftingArmSupportMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])liftingArmSupportMesh.triangles.Clone();
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracketLimitX2, bracketLimitY, bracketLimitZ);
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracket2LimitX2, bracket2LimitY, bracket2LimitZ);
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracket3LimitX2, bracket3LimitY, bracket3LimitZ);
			deleteUnmarkedPartOfMesh(liftingArmSupportMesh, triangles);
			return liftingArmSupportMesh;
		}

		private static Mesh getLiftingArmSupportRMesh(Mesh s282Mesh)
		{
			Mesh liftingArmSupportMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])liftingArmSupportMesh.triangles.Clone();
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracketLimitX, bracketLimitY, bracketLimitZ);
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracket2LimitX, bracket2LimitY, bracket2LimitZ);
			markPartOfMesh(liftingArmSupportMesh.vertices, triangles, bracket3LimitX, bracket3LimitY, bracket3LimitZ);
			deleteUnmarkedPartOfMesh(liftingArmSupportMesh, triangles);
			return liftingArmSupportMesh;
		}

		private static Mesh getCaliperMesh(Mesh s282Mesh)
		{
			Mesh brakeCaliperMesh = UnityEngine.Object.Instantiate(s282Mesh);
			//Vector3[] caliperVertices = brakeCaliperMesh.vertices;
			int[] caliperTriangles = (int[])brakeCaliperMesh.triangles.Clone();
			markPartOfMesh(brakeCaliperMesh.vertices, caliperTriangles, brakeCaliperLimitX, brakeCaliperLimitY, brakeCaliperLimitZ);
			deleteUnmarkedPartOfMesh(brakeCaliperMesh, caliperTriangles);

			brakeCaliperMesh.RecalculateNormals();
			brakeCaliperMesh.RecalculateTangents();
			brakeCaliperMesh.RecalculateBounds();
			return brakeCaliperMesh;
		}

		private static Mesh getCaliperSandMesh(Mesh s282Mesh)
		{
			Mesh brakeCaliperSandMesh = UnityEngine.Object.Instantiate(s282Mesh);
			Vector3[] caliperVertices = brakeCaliperSandMesh.vertices;
			int[] caliperTriangles = brakeCaliperSandMesh.triangles;
			//if the vertex is out of range, set it to zero
			for (int i = 0; i < caliperVertices.Length; i++)
			{
				if (!brakeCaliperLimitX.contains(caliperVertices[i].x)
					|| !brakeCaliperLimitY.contains(caliperVertices[i].y)
					|| !brakeCaliperSandLimitZ.contains(caliperVertices[i].z))
				{
					caliperVertices[i] = Vector3.zero;
				}
			}
			//triangles that contain out-of-range vertices get removed
			for (int i = 0; i < caliperTriangles.Length; i += 3)
			{
				if (caliperVertices[caliperTriangles[i]].Equals(Vector3.zero)
					|| caliperVertices[caliperTriangles[i + 1]].Equals(Vector3.zero)
					|| caliperVertices[caliperTriangles[i + 2]].Equals(Vector3.zero))
				{
					caliperTriangles[i] = -1;
					caliperTriangles[i + 1] = -1;
					caliperTriangles[i + 2] = -1;
				}
			}
			caliperVertices = caliperVertices.Where((source, index) => source != Vector3.zero).ToArray();
			brakeCaliperSandMesh.vertices = caliperVertices;
			caliperTriangles = caliperTriangles.Where((source, index) => source != -1).ToArray();
			brakeCaliperSandMesh.triangles = caliperTriangles;

			brakeCaliperSandMesh.RecalculateNormals();
			brakeCaliperSandMesh.RecalculateTangents();
			brakeCaliperSandMesh.RecalculateBounds();
			return brakeCaliperSandMesh;
		}

		private static Mesh getStackMesh(Mesh s282Mesh)
		{
			Mesh stackMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] stackTriangles = (int[])stackMesh.triangles.Clone();
			markPartOfMesh(stackMesh.vertices, stackTriangles, stackLimitX, stackLimitY, stackLimitZ);
			deleteUnmarkedPartOfMesh(stackMesh, stackTriangles);

			stackMesh.RecalculateNormals();
			stackMesh.RecalculateTangents();
			stackMesh.RecalculateBounds();
			return stackMesh;
		}

		private static Mesh getWhistleMesh(Mesh s282Mesh)
		{
			Mesh whistleMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] whistleTriangles = (int[])whistleMesh.triangles.Clone();
			markPartOfMesh(whistleMesh.vertices, whistleTriangles, whistleLimitX, whistleLimitY, whistleLimitZ);
			deleteUnmarkedPartOfMesh(whistleMesh, whistleTriangles);

			whistleMesh.RecalculateNormals();
			whistleMesh.RecalculateTangents();
			whistleMesh.RecalculateBounds();
			return whistleMesh;
		}

		private static Mesh getBellMesh(Mesh s282Mesh)
		{
			Mesh bellMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])bellMesh.triangles.Clone();
			markPartOfMesh(bellMesh.vertices, triangles, bellLimitX, bellLimitY, bellLimitZ);
			deleteUnmarkedPartOfMesh(bellMesh, triangles);

			bellMesh.RecalculateNormals();
			bellMesh.RecalculateTangents();
			bellMesh.RecalculateBounds();
			return bellMesh;
		}


		private static Mesh getBellAirLineMesh(Mesh s282Mesh)
		{
			Mesh bellAirLineMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])bellAirLineMesh.triangles.Clone();
			markPartOfMesh(bellAirLineMesh.vertices, triangles, bellAirLine1LimitX, bellAirLine1LimitY, bellAirLine1LimitZ);
			markPartOfMesh(bellAirLineMesh.vertices, triangles, bellAirLine2LimitX, bellAirLine2LimitY, bellAirLine2LimitZ);
			deleteUnmarkedPartOfMesh(bellAirLineMesh, triangles);

			bellAirLineMesh.RecalculateNormals();
			bellAirLineMesh.RecalculateTangents();
			bellAirLineMesh.RecalculateBounds();
			return bellAirLineMesh;
		}

		private static Mesh getSandDomeMesh(Mesh s282Mesh)
		{
			Mesh sandDomeMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])sandDomeMesh.triangles.Clone();
			markPartOfMesh(sandDomeMesh.vertices, triangles, sandDomeLimitX, sandDomeLimitY, sandDomeLimitZ);
			deleteUnmarkedPartOfMesh(sandDomeMesh, triangles);

			sandDomeMesh.RecalculateNormals();
			sandDomeMesh.RecalculateTangents();
			sandDomeMesh.RecalculateBounds();
			return sandDomeMesh;
		}

		private static Mesh getRunningBoardsMesh(Mesh s282Mesh)
		{
			Mesh runningBoardsMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])runningBoardsMesh.triangles.Clone();
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardsLimitX, runningBoardsLimitY, runningBoardsLimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport1LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport2LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport3LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport4LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport5LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, runningBoardSupportLimitX, runningBoardSupportLimitY, runningBoardSupport6LimitZ);
			markPartOfMesh(runningBoardsMesh.vertices, triangles, miscAirFittingsLLimitX, miscAirFittingsLLimitY, miscAirFittingsLLimitZ);
			deleteUnmarkedPartOfMesh(runningBoardsMesh, triangles);

			runningBoardsMesh.RecalculateNormals();
			runningBoardsMesh.RecalculateTangents();
			runningBoardsMesh.RecalculateBounds();
			return runningBoardsMesh;
		}

		private static Mesh getDryPipeLMesh(Mesh s282Mesh)
		{
			Mesh dryPipeMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])dryPipeMesh.triangles.Clone();
			markPartOfMesh(dryPipeMesh.vertices, triangles, dryPipeLLimitX, dryPipeLimitY, dryPipeLimitZ);
			deleteUnmarkedPartOfMesh(dryPipeMesh, triangles);

			dryPipeMesh.RecalculateNormals();
			dryPipeMesh.RecalculateTangents();
			dryPipeMesh.RecalculateBounds();
			return dryPipeMesh;
		}

		private static Mesh getDryPipeRMesh(Mesh s282Mesh)
		{
			Mesh dryPipeMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])dryPipeMesh.triangles.Clone();
			markPartOfMesh(dryPipeMesh.vertices, triangles, dryPipeRLimitX, dryPipeLimitY, dryPipeLimitZ);
			deleteUnmarkedPartOfMesh(dryPipeMesh, triangles);

			dryPipeMesh.RecalculateNormals();
			dryPipeMesh.RecalculateTangents();
			dryPipeMesh.RecalculateBounds();
			return dryPipeMesh;
		}

		private static Mesh getHandrailLMesh(Mesh s282Mesh)
		{
			Mesh handrailLMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])handrailLMesh.triangles.Clone();
			markPartOfMesh(handrailLMesh.vertices, triangles, handrailLLimitX, handrailLimitY, handrailLimitZ);
			markPartOfMesh(handrailLMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport1LimitZ);
			markPartOfMesh(handrailLMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport2LimitZ);
			markPartOfMesh(handrailLMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport3LimitZ);
			markPartOfMesh(handrailLMesh.vertices, triangles, handrailSupportLLimitX, handrailSupportLimitY, handrailSupport4LimitZ);
			deleteUnmarkedPartOfMesh(handrailLMesh, triangles);

			handrailLMesh.RecalculateNormals();
			handrailLMesh.RecalculateTangents();
			handrailLMesh.RecalculateBounds();
			return handrailLMesh;
		}

		private static Mesh getHandrailRMesh(Mesh s282Mesh)
		{
			Mesh handrailRMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])handrailRMesh.triangles.Clone();
			markPartOfMesh(handrailRMesh.vertices, triangles, handrailRLimitX, handrailLimitY, handrailLimitZ);
			markPartOfMesh(handrailRMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport1LimitZ);
			markPartOfMesh(handrailRMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport2LimitZ);
			markPartOfMesh(handrailRMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport3LimitZ);
			markPartOfMesh(handrailRMesh.vertices, triangles, handrailSupportRLimitX, handrailSupportLimitY, handrailSupport4LimitZ);
			deleteUnmarkedPartOfMesh(handrailRMesh, triangles);

			handrailRMesh.RecalculateNormals();
			handrailRMesh.RecalculateTangents();
			handrailRMesh.RecalculateBounds();
			return handrailRMesh;
		}

		private static Mesh getStepsLMesh(Mesh s282Mesh)
		{
			Mesh stepsLMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])stepsLMesh.triangles.Clone();
			markPartOfMesh(stepsLMesh.vertices, triangles, stepsLLimitX, stepsLimitY, stepsLimitZ);
			deleteUnmarkedPartOfMesh(stepsLMesh, triangles);

			stepsLMesh.RecalculateNormals();
			stepsLMesh.RecalculateTangents();
			stepsLMesh.RecalculateBounds();
			return stepsLMesh;
		}

		private static Mesh getStepsRMesh(Mesh s282Mesh)
		{
			Mesh stepsLMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])stepsLMesh.triangles.Clone();
			markPartOfMesh(stepsLMesh.vertices, triangles, stepsRLimitX, stepsLimitY, stepsLimitZ);
			deleteUnmarkedPartOfMesh(stepsLMesh, triangles);

			stepsLMesh.RecalculateNormals();
			stepsLMesh.RecalculateTangents();
			stepsLMesh.RecalculateBounds();
			return stepsLMesh;
		}

		private static Mesh getFrontBoilerSupportMesh(Mesh s282Mesh)
		{
			Mesh frontBoilerSupportMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])frontBoilerSupportMesh.triangles.Clone();
			markPartOfMesh(frontBoilerSupportMesh.vertices, triangles,
						   frontBoilerSupportLimit1X, frontBoilerSupportLimit1Y, frontBoilerSupportLimitZ);
			markPartOfMesh(frontBoilerSupportMesh.vertices, triangles,
						   frontBoilerSupportLimit2X, frontBoilerSupportLimit2Y, frontBoilerSupportLimitZ);
			markPartOfMesh(frontBoilerSupportMesh.vertices, triangles,
						   frontBoilerSupportLimit3X, frontBoilerSupportLimit1Y, frontBoilerSupportLimitZ);
			deleteUnmarkedPartOfMesh(frontBoilerSupportMesh, triangles);

			frontBoilerSupportMesh.RecalculateNormals();
			frontBoilerSupportMesh.RecalculateTangents();
			frontBoilerSupportMesh.RecalculateBounds();
			return frontBoilerSupportMesh;
		}

		private static Mesh getPilotMesh(Mesh s282Mesh)
		{
			Mesh pilotMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])pilotMesh.triangles.Clone();
			markPartOfMesh(pilotMesh.vertices, triangles, pilotLimit1X, pilotLimit1Y, pilotLimit1Z);
			markPartOfMesh(pilotMesh.vertices, triangles, pilotLimit2X, pilotLimit1Y, pilotLimit1Z);
			markPartOfMesh(pilotMesh.vertices, triangles, pilotLimit3X, pilotLimit3Y, pilotLimit3Z);
			deleteUnmarkedPartOfMesh(pilotMesh, triangles);

			pilotMesh.RecalculateNormals();
			pilotMesh.RecalculateTangents();
			pilotMesh.RecalculateBounds();
			return pilotMesh;
		}

		private static Mesh getToolboxMesh(Mesh s282Mesh)
		{
			Mesh toolboxMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])toolboxMesh.triangles.Clone();
			markPartOfMesh(toolboxMesh.vertices, triangles, toolboxLimitX, toolboxLimitY, toolboxLimitZ);
			markPartOfMesh(toolboxMesh.vertices, triangles, toolboxLimitX, toolbox2LimitY, toolbox2LimitZ);
			deleteUnmarkedPartOfMesh(toolboxMesh, triangles);

			toolboxMesh.RecalculateNormals();
			toolboxMesh.RecalculateTangents();
			toolboxMesh.RecalculateBounds();
			return toolboxMesh;
		}

		private static Mesh getFrontHandrailMesh(Mesh s282Mesh)
		{
			Mesh frontHandrailMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])frontHandrailMesh.triangles.Clone();
			markPartOfMesh(frontHandrailMesh.vertices, triangles, frontHandrailLimitX, frontHandrailLimitY, frontHandrailLimitZ);
			markPartOfMesh(frontHandrailMesh.vertices, triangles, frontHandrailLimitX, frontHandrailLimit2Y, frontHandrailLimit2Z);
			deleteUnmarkedPartOfMesh(frontHandrailMesh, triangles);

			frontHandrailMesh.RecalculateNormals();
			frontHandrailMesh.RecalculateTangents();
			frontHandrailMesh.RecalculateBounds();
			return frontHandrailMesh;
		}

		private static Mesh getAirPumpMesh(Mesh s282Mesh)
		{
			Mesh airPumpMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])airPumpMesh.triangles.Clone();
			markPartOfMesh(airPumpMesh.vertices, triangles, airPumpLimitX, airPumpLimitY, airPumpLimitZ);
			deleteUnmarkedPartOfMesh(airPumpMesh, triangles);

			airPumpMesh.RecalculateNormals();
			airPumpMesh.RecalculateTangents();
			airPumpMesh.RecalculateBounds();
			return airPumpMesh;
		}

		private static Mesh getCabMesh(Mesh s282Mesh)
		{
			Mesh cabMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])cabMesh.triangles.Clone();
			markPartOfMesh(cabMesh.vertices, triangles, cabLimitX, cabLimitY, cabLimitZ);
			markPartOfMesh(cabMesh.vertices, triangles, cabRoofLimitX, cabRoofLimitY, cabRoofLimitZ);
			deleteUnmarkedPartOfMesh(cabMesh, triangles);

			cabMesh.RecalculateNormals();
			cabMesh.RecalculateTangents();
			cabMesh.RecalculateBounds();
			return cabMesh;
		}

		private static Mesh getReachRodSupportMesh(Mesh s282Mesh)
		{
			Mesh reachRodSupportMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])reachRodSupportMesh.triangles.Clone();
			markPartOfMesh(reachRodSupportMesh.vertices, triangles, reachRodSupportLimitX, reachRodSupportLimitY, reachRodSupportLimitZ);
			deleteUnmarkedPartOfMesh(reachRodSupportMesh, triangles);

			reachRodSupportMesh.RecalculateNormals();
			reachRodSupportMesh.RecalculateTangents();
			reachRodSupportMesh.RecalculateBounds();
			return reachRodSupportMesh;
		}

		private static Mesh getLubricatorMesh(Mesh s282Mesh)
		{
			Mesh lubricatorMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])lubricatorMesh.triangles.Clone();
			markPartOfMesh(lubricatorMesh.vertices, triangles, lubricatorLimitX, lubricatorLimitY, lubricatorLimitZ);
			deleteUnmarkedPartOfMesh(lubricatorMesh, triangles);

			lubricatorMesh.RecalculateNormals();
			lubricatorMesh.RecalculateTangents();
			lubricatorMesh.RecalculateBounds();
			return lubricatorMesh;
		}

		private static Mesh getLubricatorSupportMesh(Mesh s282Mesh)
		{
			Mesh lubricatorSupportMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])lubricatorSupportMesh.triangles.Clone();
			markPartOfMesh(lubricatorSupportMesh.vertices, triangles, lubricatorSupportLimitX, lubricatorSupportLimitY, lubricatorSupportLimitZ);
			deleteUnmarkedPartOfMesh(lubricatorSupportMesh, triangles);

			lubricatorSupportMesh.RecalculateNormals();
			lubricatorSupportMesh.RecalculateTangents();
			lubricatorSupportMesh.RecalculateBounds();
			return lubricatorSupportMesh;
		}

		private static Mesh getOilLinesRMesh(Mesh s282Mesh)
		{
			Mesh oilLinesRMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])oilLinesRMesh.triangles.Clone();
			markPartOfMesh(oilLinesRMesh.vertices, triangles, oilLinesRLimitX, oilLinesRLimitY, oilLinesRLimitZ);
			deleteUnmarkedPartOfMesh(oilLinesRMesh, triangles);

			oilLinesRMesh.RecalculateNormals();
			oilLinesRMesh.RecalculateTangents();
			oilLinesRMesh.RecalculateBounds();
			return oilLinesRMesh;
		}

		private static Mesh getOilLinesFMesh(Mesh s282Mesh)
		{
			Mesh oilLinesFMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])oilLinesFMesh.triangles.Clone();
			markPartOfMesh(oilLinesFMesh.vertices, triangles, oilLinesFLimitX, oilLinesFLimitY, oilLinesFLimitZ);
			deleteUnmarkedPartOfMesh(oilLinesFMesh, triangles);

			oilLinesFMesh.RecalculateNormals();
			oilLinesFMesh.RecalculateTangents();
			oilLinesFMesh.RecalculateBounds();
			return oilLinesFMesh;
		}

		private static Mesh getAirTankMesh(Mesh s282Mesh)
		{
			Mesh airTankMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])airTankMesh.triangles.Clone();
			markPartOfMesh(airTankMesh.vertices, triangles, airTankSupportRLimitX, airTankSupportLimitY, airTankSupportFLimitZ);
			markPartOfMesh(airTankMesh.vertices, triangles, airTankSupportRLimitX, airTankSupportLimitY, airTankSupportRLimitZ);
			markPartOfMesh(airTankMesh.vertices, triangles, airTankRLimitX, airTankLimitY, airTankLimitZ);
			deleteUnmarkedPartOfMesh(airTankMesh, triangles);

			airTankMesh.RecalculateNormals();
			airTankMesh.RecalculateTangents();
			airTankMesh.RecalculateBounds();
			return airTankMesh;
		}

		private static Mesh getAirPumpInputPipeMesh(Mesh s282Mesh)
		{
			Mesh airPumpInputPipeMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])airPumpInputPipeMesh.triangles.Clone();
			markPartOfMesh(airPumpInputPipeMesh.vertices, triangles, airPumpInputPipeLimitX, airPumpInputPipeLimitY, airPumpInputPipeLimitZ);
			markPartOfMesh(airPumpInputPipeMesh.vertices, triangles, airPumpInputPipeLimit2X, airPumpInputPipeLimit2Y, airPumpInputPipeLimit2Z);
			deleteUnmarkedPartOfMesh(airPumpInputPipeMesh, triangles);

			airPumpInputPipeMesh.RecalculateNormals();
			airPumpInputPipeMesh.RecalculateTangents();
			airPumpInputPipeMesh.RecalculateBounds();
			return airPumpInputPipeMesh;
		}

		private static Mesh getAirPumpOutputPipeMesh(Mesh s282Mesh)
		{
			Mesh airPumpOutputPipeMesh = UnityEngine.Object.Instantiate(s282Mesh);
			int[] triangles = (int[])airPumpOutputPipeMesh.triangles.Clone();
			markPartOfMesh(airPumpOutputPipeMesh.vertices, triangles, airPumpOutputPipeLimitX, airPumpOutputPipeLimitY, airPumpOutputPipeLimitZ);
			deleteUnmarkedPartOfMesh(airPumpOutputPipeMesh, triangles);

			airPumpOutputPipeMesh.RecalculateNormals();
			airPumpOutputPipeMesh.RecalculateTangents();
			airPumpOutputPipeMesh.RecalculateBounds();
			return airPumpOutputPipeMesh;
		}
	}
}
