using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters.DE6
{
	internal class DE6LOD0MeshSplitter
	{
		public static GameObject SplitLocoBodyLOD0
		{ get; private set; }

		//not a cowcatcher smh
		private static readonly RangeFloat plowLimitX = new(-1.5f, 1.5f);
		private static readonly RangeFloat plowLimitY = new(0, 1);
		private static readonly RangeFloat plowLimitZ = new(8.75f, 10);
		private static readonly RangeFloat plowLimitZ2 = new(-10f, -8.75f);

		//front and back handrails
		private static readonly RangeFloat handrail1LimitX = new(-1.4f, 1.4f);
		private static readonly RangeFloat handrail1LimitY = new(1.61f, 2.6f);
		private static readonly RangeFloat handrail1LimitZ = new(8.7f, 9);
		private static readonly RangeFloat handrail1LimitZ2 = new(-9, -8.7f);

		private static readonly RangeFloat headlightLimitX = new(-0.21f, 0.21f);
		private static readonly RangeFloat headlightLimitY = new(2.6f, 3.7f);
		private static readonly RangeFloat headlightLimitZ = new(8.1f, 8.6f);
		private static readonly RangeFloat headlightLimitZ2 = new(-8.4f, -8.2f);

		private static readonly RangeFloat upperHeadlightLimitX = new(-0.15f, 0.15f);
		private static readonly RangeFloat upperHeadlightLimitY = new(3.61f, 3.9f);
		private static readonly RangeFloat upperHeadlightLimitZ = new (7.05f, 7.2f);

		private static readonly RangeFloat ditchLightLimitX = new(-0.73f, -0.49f);
		private static readonly RangeFloat ditchLightLimitX2 = new(0.49f, 0.73f);
		private static readonly RangeFloat ditchLightLimitY = new(1.8f, 2.2f);
		private static readonly RangeFloat ditchLightLimitZ = new(8.2f, 8.4f);
		private static readonly RangeFloat ditchLightLimitZ2 = new(-8.3f, -8.1f);

		private static readonly RangeFloat shortHoodBaseLimitX = new(-0.92f, 0.92f);
		private static readonly RangeFloat shortHoodBaseLimitY = new(1.64f, 1.68f);
		private static readonly RangeFloat shortHoodBaseLimitZ = new(6.88f, 8.5f);

		private static readonly RangeFloat shortHoodLimitX = new(-0.92f, 0.92f);
		private static readonly RangeFloat shortHoodLimitY = new(1.67f, 3f);
		private static readonly RangeFloat shortHoodLimitZ = new(6.88f, 8.5f);

		private static readonly RangeFloat numberBoardsLimitX = new(-0.6983f, 0.6983f);
		private static readonly RangeFloat numberBoardsLimitY = new(3.6f, 3.91f);
		private static readonly RangeFloat numberBoardsLimitZ = new(6.8f, 7.13f);

		private static readonly RangeFloat hornLimitX = new(-0.67f, -0.406f);
		private static readonly RangeFloat hornLimitY = new(3.87f, 5f);
		private static readonly RangeFloat hornLimitZ = new(6.6f, 7.3f);

		//splitting these because they get in the way of getting the middle handrails
		private static readonly RangeFloat doorHingeLimitX = new(-1.47f, -1.44f);
		private static readonly RangeFloat doorHingeLimitX2 = new(1.44f, 1.47f);
		private static readonly RangeFloat doorHingeLimitY = new(1.8f, 3.0f);
		private static readonly RangeFloat doorHingeLimitZ = new(6.85f, 6.95f);
		private static readonly RangeFloat doorHingeLimitZ2 = new(4.8f, 4.9f);

		//side handrails in front of cab
		private static readonly RangeFloat handrail2LimitX = new(-1.6f, -1.43f);
		private static readonly RangeFloat handrail2LimitX2 = new(1.43f, 1.6f);
		private static readonly RangeFloat handrail2LimitY = new(1.526f, 2.7f);
		private static readonly RangeFloat handrail2LimitZ = new(6.8f, 8.1f);

		//side handrails behind cab
		private static readonly RangeFloat handrail3LimitX = new(-1.6f, -1.43f);
		private static readonly RangeFloat handrail3LimitX2 = new(1.43f, 1.6f);
		private static readonly RangeFloat handrail3LimitY = new(1.526f, 2.7f);
		private static readonly RangeFloat handrail3LimitZ = new(3.1f, 4.95f);

		//side handrails at rear
		private static readonly RangeFloat handrail4LimitX = new(-1.6f, -1.43f);
		private static readonly RangeFloat handrail4LimitX2 = new(1.43f, 1.6f);
		private static readonly RangeFloat handrail4LimitY = new(1.526f, 2.7f);
		private static readonly RangeFloat handrail4LimitZ = new(-8.05f, -5.31f);

		private static readonly RangeFloat cabLimitX = new(-1.6f, 1.6f);
		private static readonly RangeFloat cabLimitY = new(1.5f, 4f);
		private static readonly RangeFloat cabLimitZ = new(4.55f, 7.2f);

		private static readonly RangeFloat handrailTopLimitX = new(-1.1f, -0.8f);
		private static readonly RangeFloat handrailTopLimitX2 = new(0.8f, 1.1f);
		private static readonly RangeFloat handrailTopLimitY = new(3.36f, 3.6f);
		private static readonly RangeFloat handrailTopLimitZ = new(-8.1f, 4.9f);

		private static readonly RangeFloat fanLimitX = new(-0.5f, 0.5f);
		private static readonly RangeFloat fanLimitY = new(3.8f, 4f);
		private static readonly RangeFloat fanLimitZ = new(3.1f, 4.1f);
		private static readonly RangeFloat fanLimitZ2 = new(1.6f, 2.6f);
		private static readonly RangeFloat fanLimitZ3 = new(-5.8f, -4.8f);
		private static readonly RangeFloat fanLimitZ4 = new(-7.3f, -6.3f);

		private static readonly RangeFloat exhaustLimitX = new(-0.38f, 0.38f);
		private static readonly RangeFloat exhaustLimitY = new(3.8f, 4.2f);
		private static readonly RangeFloat exhaustLimitZ = new(-2.8f, -2.3f);

		private static readonly RangeFloat longHoodLimitX = new(-1.1f, 1.1f);
		private static readonly RangeFloat longHoodLimitY = new(1.66f, 4f);
		private static readonly RangeFloat longHoodLimitZ = new(-8.5f, 5f);

		//TODO: rear side handrails
		//LODs, interior

		internal static void Init()
		{
			SplitLocoBodyLOD0 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoBodyLOD0);
		}

		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh("diesel_body");
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the diesel_body_LOD0 mesh");
				return null;
			}
			Main.Logger.Log($"Splitting diesel_body_LOD0 mesh...");

			GameObject splitLocoBodyLOD0 = new GameObject("diesel_body_LOD0");
			splitLocoBodyLOD0.SetActive(false);

			Mesh plowFMesh = GetPlowFMesh(locoMesh);
			Mesh plowRMesh = GetPlowRMesh(locoMesh);
			Mesh doorHingesMesh = GetCabDoorHingesMesh(locoMesh);
			Mesh handrailFMesh = GetHandrailFMesh(locoMesh);
			Mesh handrailRMesh = GetHandrailRMesh(locoMesh);
			Mesh headlightFMesh = GetHeadlightFMesh(locoMesh);
			Mesh headlightRMesh = GetHeadlightRMesh(locoMesh);
			Mesh ditchLightRFMesh = GetDitchLightRFMesh(locoMesh);
			Mesh ditchLightLFMesh = GetDitchLightLFMesh(locoMesh);
			Mesh ditchLightRRMesh = GetDitchLightRRMesh(locoMesh);
			Mesh ditchLightLRMesh = GetDitchLightLRMesh(locoMesh);
			Mesh shortHoodBaseMesh = GetShortHoodBaseMesh(locoMesh);
			Mesh upperHeadlightMesh = GetUpperHeadlightFMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, doorHingeLimitX2, doorHingeLimitY, doorHingeLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, doorHingeLimitX, doorHingeLimitY, doorHingeLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, shortHoodBaseLimitX, shortHoodBaseLimitY, shortHoodBaseLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, upperHeadlightLimitX, upperHeadlightLimitY, upperHeadlightLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh shortHoodMesh = GetShortHoodMesh(locoMesh);
			Mesh numberBoardsMesh = GetNumberBoardsMesh(locoMesh);
			Mesh hornMesh = GetHornMesh(locoMesh);
			Mesh handrailLFMesh = GetHandrailLFMesh(locoMesh);
			Mesh handrailRFMesh = GetHandrailRFMesh(locoMesh);
			Mesh handrailLMMesh = GetHandrailLMMesh(locoMesh);
			Mesh handrailRMMesh = GetHandrailRMMesh(locoMesh);
			Mesh handrailLRMesh = GetHandrailLRMesh(locoMesh);
			Mesh handrailRRMesh = GetHandrailRRMesh(locoMesh);
			Mesh handrailTopLMesh = GetHandrailTopLMesh(locoMesh);
			Mesh handrailTopRMesh = GetHandrailTopRMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, shortHoodLimitX, shortHoodLimitY, shortHoodLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, numberBoardsLimitX, numberBoardsLimitY, numberBoardsLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, hornLimitX, hornLimitY, hornLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail2LimitX, handrail2LimitY, handrail2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail2LimitX2, handrail2LimitY, handrail2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail3LimitX, handrail3LimitY, handrail3LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail3LimitX2, handrail3LimitY, handrail3LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail4LimitX, handrail4LimitY, handrail4LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail4LimitX2, handrail4LimitY, handrail4LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailTopLimitX, handrailTopLimitY, handrailTopLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailTopLimitX2, handrailTopLimitY, handrailTopLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh cabMesh = GetCabMesh(locoMesh);
			Mesh fan1Mesh = GetFan1Mesh(locoMesh);
			Mesh fan2Mesh = GetFan2Mesh(locoMesh);
			Mesh fan3Mesh = GetFan3Mesh(locoMesh);
			Mesh fan4Mesh = GetFan4Mesh(locoMesh);
			Mesh exhaustMesh = GetExhaustMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, cabLimitX, cabLimitY, cabLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ3);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ4);
			markPartOfMesh(locoMesh.vertices, triangles, exhaustLimitX, exhaustLimitY, exhaustLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh longHoodMesh = GetLongHoodMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, longHoodLimitX, longHoodLimitY, longHoodLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			GameObject splitLoco = new GameObject("de6_body");
			splitLoco.transform.SetParent(splitLocoBodyLOD0.transform);
			splitLoco.transform.localScale = new Vector3(-1, 1, 1);
			splitLoco.AddComponent<MeshFilter>().mesh = locoMesh;
			splitLoco.AddComponent<MeshRenderer>();

			GameObject plowF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			plowF.GetComponent<MeshFilter>().mesh = plowFMesh;
			plowF.name = "de6_plow_f";

			GameObject plowR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			plowR.GetComponent<MeshFilter>().mesh = plowRMesh;
			plowR.name = "de6_plow_r";

			GameObject doorHinges = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			doorHinges.GetComponent<MeshFilter>().mesh = doorHingesMesh;
			doorHinges.name = "de6_door_hinges";

			GameObject handrailF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailF.GetComponent<MeshFilter>().mesh = handrailFMesh;
			handrailF.name = "de6_handrail_f";

			GameObject handrailLF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailLF.GetComponent<MeshFilter>().mesh = handrailLFMesh;
			handrailLF.name = "de6_handrail_lf";

			GameObject handrailRF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailRF.GetComponent<MeshFilter>().mesh = handrailRFMesh;
			handrailRF.name = "de6_handrail_rf";

			GameObject handrailLM = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailLM.GetComponent<MeshFilter>().mesh = handrailLMMesh;
			handrailLM.name = "de6_handrail_lm";

			GameObject handrailRM = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailRM.GetComponent<MeshFilter>().mesh = handrailRMMesh;
			handrailRM.name = "de6_handrail_rm";

			GameObject handrailLR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailLR.GetComponent<MeshFilter>().mesh = handrailLRMesh;
			handrailLR.name = "de6_handrail_lr";

			GameObject handrailRR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailRR.GetComponent<MeshFilter>().mesh = handrailRRMesh;
			handrailRR.name = "de6_handrail_rr";

			GameObject handrailTopL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailTopL.GetComponent<MeshFilter>().mesh = handrailTopLMesh;
			handrailTopL.name = "de6_handrail_top_l";

			GameObject handrailTopR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailTopR.GetComponent<MeshFilter>().mesh = handrailTopRMesh;
			handrailTopR.name = "de6_handrail_top_r";

			GameObject handrailR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailR.GetComponent<MeshFilter>().mesh = handrailRMesh;
			handrailR.name = "de6_handrail_r";

			GameObject headlightF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightF.GetComponent<MeshFilter>().mesh = headlightFMesh;
			headlightF.name = "de6_headlight_f";

			GameObject headlightFUpper = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightFUpper.GetComponent<MeshFilter>().mesh = upperHeadlightMesh;
			headlightFUpper.name = "de6_headlight_f_upper";

			GameObject headlightR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightR.GetComponent<MeshFilter>().mesh = headlightRMesh;
			headlightR.name = "de6_headlight_r";

			GameObject ditchLightRF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightRF.GetComponent<MeshFilter>().mesh = ditchLightRFMesh;
			ditchLightRF.name = "de6_ditch_light_rf";

			GameObject ditchLightLF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightLF.GetComponent<MeshFilter>().mesh = ditchLightLFMesh;
			ditchLightLF.name = "de6_ditch_light_lf";

			GameObject ditchLightRR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightRR.GetComponent<MeshFilter>().mesh = ditchLightRRMesh;
			ditchLightRR.name = "de6_ditch_light_rr";

			GameObject ditchLightLR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightLR.GetComponent<MeshFilter>().mesh = ditchLightLRMesh;
			ditchLightLR.name = "de6_ditch_light_lr";

			GameObject shortHood = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			shortHood.GetComponent<MeshFilter>().mesh = shortHoodMesh;
			shortHood.name = "de6_short_hood";

			GameObject shortHoodBase = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			shortHoodBase.GetComponent<MeshFilter>().mesh = shortHoodBaseMesh;
			shortHoodBase.name = "de6_short_hood_base";

			GameObject numberBoards = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			numberBoards.GetComponent<MeshFilter>().mesh = numberBoardsMesh;
			numberBoards.name = "de6_number_boards";

			GameObject horn = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			horn.GetComponent<MeshFilter>().mesh = hornMesh;
			horn.name = "de6_horn";

			GameObject cab = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			cab.GetComponent<MeshFilter>().mesh = cabMesh;
			cab.name = "de6_cab";

			GameObject longHood = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			longHood.GetComponent<MeshFilter>().mesh = longHoodMesh;
			longHood.name = "de6_long_hood";

			GameObject fan1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan1.GetComponent<MeshFilter>().mesh = fan1Mesh;
			fan1.name = "de6_fan_1";

			GameObject fan2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan2.GetComponent<MeshFilter>().mesh = fan2Mesh;
			fan2.name = "de6_fan_2";

			GameObject fan3 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan3.GetComponent<MeshFilter>().mesh = fan3Mesh;
			fan3.name = "de6_fan_3";

			GameObject fan4 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan4.GetComponent<MeshFilter>().mesh = fan4Mesh;
			fan4.name = "de6_fan_4";

			GameObject exhaust = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			exhaust.GetComponent<MeshFilter>().mesh = exhaustMesh;
			exhaust.name = "de6_exhaust";

			Main.Logger.Log("Split diesel_body_LOD0 mesh.");
			return splitLocoBodyLOD0;
		}

		private static Mesh GetPlowFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetPlowRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeadlightFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetUpperHeadlightFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, upperHeadlightLimitX, upperHeadlightLimitY, upperHeadlightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeadlightRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetDitchLightRFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetDitchLightLFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetDitchLightRRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetDitchLightLRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetShortHoodMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, shortHoodLimitX, shortHoodLimitY, shortHoodLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetShortHoodBaseMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, shortHoodBaseLimitX, shortHoodBaseLimitY, shortHoodBaseLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetNumberBoardsMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, numberBoardsLimitX, numberBoardsLimitY, numberBoardsLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHornMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, hornLimitX, hornLimitY, hornLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCabDoorHingesMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, doorHingeLimitX2, doorHingeLimitY, doorHingeLimitZ);
			markPartOfMesh(mesh.vertices, triangles, doorHingeLimitX, doorHingeLimitY, doorHingeLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailLFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail2LimitX2, handrail2LimitY, handrail2LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailRFMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail2LimitX, handrail2LimitY, handrail2LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailLMMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail3LimitX2, handrail3LimitY, handrail3LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailRMMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail3LimitX, handrail3LimitY, handrail3LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailLRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail4LimitX2, handrail4LimitY, handrail4LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailRRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrail4LimitX, handrail4LimitY, handrail4LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCabMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, cabLimitX, cabLimitY, cabLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailTopLMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrailTopLimitX2, handrailTopLimitY, handrailTopLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailTopRMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrailTopLimitX, handrailTopLimitY, handrailTopLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetFan1Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetFan2Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ2);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetFan3Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ3);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetFan4Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ4);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetExhaustMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, exhaustLimitX, exhaustLimitY, exhaustLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetLongHoodMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, longHoodLimitX, longHoodLimitY, longHoodLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}
	}
}
