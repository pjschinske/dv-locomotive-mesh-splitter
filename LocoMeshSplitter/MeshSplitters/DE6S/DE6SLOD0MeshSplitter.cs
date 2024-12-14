using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters.DE6S
{
	internal class DE6SLOD0MeshSplitter
	{
		public static GameObject SplitLocoBodyLOD0
		{ get; private set; }

		//not a cowcatcher smh
		private static readonly RangeFloat plowLimitX = new(-1.5f, 1.5f);
		private static readonly RangeFloat plowLimitY = new(0, 1);
		private static readonly RangeFloat plowLimitZ = new(7.75f, 10);
		private static readonly RangeFloat plowLimitZ2 = new(-10f, -7.75f);

		//front and back handrails
		private static readonly RangeFloat handrail1LimitX = new(-1.4f, 1.4f);
		private static readonly RangeFloat handrail1LimitY = new(1.61f, 2.6f);
		private static readonly RangeFloat handrail1LimitZ = new(7.7f, 8);
		private static readonly RangeFloat handrail1LimitZ2 = new(-8, -7.7f);

		private static readonly RangeFloat headlightLimitX = new(-0.21f, 0.21f);
		private static readonly RangeFloat headlightLimitY = new(2.3f, 3.1f);
		private static readonly RangeFloat headlightLimitZ = new(7.1f, 7.4f);
		private static readonly RangeFloat headlightLimitZ2 = new(-7.4f, -7.1f);

		private static readonly RangeFloat ditchLightLimitX = new(-0.73f, -0.49f);
		private static readonly RangeFloat ditchLightLimitX2 = new(0.49f, 0.73f);
		private static readonly RangeFloat ditchLightLimitY = new(1.7f, 2.2f);
		private static readonly RangeFloat ditchLightLimitZ = new(7.1f, 7.4f);
		private static readonly RangeFloat ditchLightLimitZ2 = new(-7.3f, -7.1f);

		private static readonly RangeFloat shortHoodBaseLimitX = new(-0.93f, 0.93f);
		private static readonly RangeFloat shortHoodBaseLimitY = new(1.64f, 1.68f);
		private static readonly RangeFloat shortHoodBaseLimitZ = new(5.3f, 7.3f);
		private static readonly RangeFloat longHoodBaseLimitZ = new(-7.3f, 5.33f);

		private static readonly RangeFloat shortHoodLimitX = new(-1.1f, 1.1f);
		private static readonly RangeFloat shortHoodLimitY = new(1.66f, 3.2f);
		private static readonly RangeFloat shortHoodLimitZ = new(5.3f, 7.3f);

		//side handrails in front of cab
		private static readonly RangeFloat handrail2LimitX = new(-1.6f, -1.43f);
		private static readonly RangeFloat handrail2LimitX2 = new(1.43f, 1.6f);
		private static readonly RangeFloat handrail2LimitY = new(1.526f, 2.7f);
		private static readonly RangeFloat handrail2LimitZ = new(4.35f, 7.1f);

		//side handrails at rear
		private static readonly RangeFloat handrail4LimitX = new(-1.6f, -1.43f);
		private static readonly RangeFloat handrail4LimitX2 = new(1.43f, 1.6f);
		private static readonly RangeFloat handrail4LimitY = new(1.526f, 2.7f);
		private static readonly RangeFloat handrail4LimitZ = new(-7.1f, -4.35f);

		private static readonly RangeFloat handrailTopLimitX = new(-1.1f, -0.8f);
		private static readonly RangeFloat handrailTopLimitX2 = new(0.8f, 1.1f);
		private static readonly RangeFloat handrailTopLimitY = new(2.838f, 3f);
		private static readonly RangeFloat handrailTopLimitZ = new(-7.1f, 7.1f);

		private static readonly RangeFloat fanLimitX = new(-0.5f, 0.5f);
		private static readonly RangeFloat fanLimitY = new(3.1f, 3.4f);
		private static readonly RangeFloat fanLimitZ = new(4.0f, 5.1f);
		private static readonly RangeFloat fanLimitZ2 = new(2.5f, 3.6f);
		private static readonly RangeFloat fanLimitZ3 = new(-4.9f, -3.8f);
		private static readonly RangeFloat fanLimitZ4 = new(-6.3f, -5.3f);

		private static readonly RangeFloat exhaustLimitX = new(-0.38f, 0.38f);
		private static readonly RangeFloat exhaustLimitY = new(3.1f, 3.3f);
		private static readonly RangeFloat exhaustLimitZ = new(-1.8f, -1.3f);

		private static readonly RangeFloat longHoodLimitX = new(-1.1f, 1.1f);
		private static readonly RangeFloat longHoodLimitY = new(1.66f, 4f);
		private static readonly RangeFloat longHoodLimitZ = new(-8.5f, 5.34f);

		//TODO: rear side handrails
		//LODs, interior

		internal static void Init()
		{
			SplitLocoBodyLOD0 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoBodyLOD0);
		}

		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh("de6_slug");
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the de6_slug LOD0 mesh");
				return null;
			}
			Main.Logger.Log($"Splitting diesel_body_LOD0 mesh...");

			GameObject splitLocoBodyLOD0 = new GameObject("de6_slug_LOD0");
			splitLocoBodyLOD0.SetActive(false);

			Mesh plowFMesh = GetPlowFMesh(locoMesh);
			Mesh plowRMesh = GetPlowRMesh(locoMesh);
			Mesh handrailFMesh = GetHandrailFMesh(locoMesh);
			Mesh handrailRMesh = GetHandrailRMesh(locoMesh);
			Mesh headlightFMesh = GetHeadlightFMesh(locoMesh);
			Mesh headlightRMesh = GetHeadlightRMesh(locoMesh);
			Mesh ditchLightRFMesh = GetDitchLightRFMesh(locoMesh);
			Mesh ditchLightLFMesh = GetDitchLightLFMesh(locoMesh);
			Mesh ditchLightRRMesh = GetDitchLightRRMesh(locoMesh);
			Mesh ditchLightLRMesh = GetDitchLightLRMesh(locoMesh);
			Mesh shortHoodBaseMesh = GetShortHoodBaseMesh(locoMesh);
			Mesh longHoodBaseMesh = GetLongHoodBaseMesh(locoMesh);

			int[] triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, plowLimitX, plowLimitY, plowLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail1LimitX, handrail1LimitY, handrail1LimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, headlightLimitX, headlightLimitY, headlightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX, ditchLightLimitY, ditchLightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, ditchLightLimitX2, ditchLightLimitY, ditchLightLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, shortHoodBaseLimitX, shortHoodBaseLimitY, shortHoodBaseLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, shortHoodBaseLimitX, shortHoodBaseLimitY, longHoodBaseLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh handrailLFMesh = GetHandrailLFMesh(locoMesh);
			Mesh handrailRFMesh = GetHandrailRFMesh(locoMesh);
			Mesh handrailLRMesh = GetHandrailLRMesh(locoMesh);
			Mesh handrailRRMesh = GetHandrailRRMesh(locoMesh);
			Mesh handrailTopLMesh = GetHandrailTopLMesh(locoMesh);
			Mesh handrailTopRMesh = GetHandrailTopRMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, handrail2LimitX, handrail2LimitY, handrail2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail2LimitX2, handrail2LimitY, handrail2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail4LimitX, handrail4LimitY, handrail4LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrail4LimitX2, handrail4LimitY, handrail4LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailTopLimitX, handrailTopLimitY, handrailTopLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailTopLimitX2, handrailTopLimitY, handrailTopLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh fan1Mesh = GetFan1Mesh(locoMesh);
			Mesh fan2Mesh = GetFan2Mesh(locoMesh);
			Mesh fan3Mesh = GetFan3Mesh(locoMesh);
			Mesh fan4Mesh = GetFan4Mesh(locoMesh);
			Mesh exhaustMesh = GetExhaustMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ2);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ3);
			markPartOfMesh(locoMesh.vertices, triangles, fanLimitX, fanLimitY, fanLimitZ4);
			markPartOfMesh(locoMesh.vertices, triangles, exhaustLimitX, exhaustLimitY, exhaustLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh longHoodMesh = GetLongHoodMesh(locoMesh);
			Mesh shortHoodMesh = GetShortHoodMesh(locoMesh);

			triangles = (int[])locoMesh.triangles.Clone();

			markPartOfMesh(locoMesh.vertices, triangles, shortHoodLimitX, shortHoodLimitY, shortHoodLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, longHoodLimitX, longHoodLimitY, longHoodLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			GameObject splitLoco = new GameObject("de6s_body");
			splitLoco.transform.SetParent(splitLocoBodyLOD0.transform);
			splitLoco.transform.localScale = new Vector3(-1, 1, 1);
			splitLoco.AddComponent<MeshFilter>().mesh = locoMesh;
			splitLoco.AddComponent<MeshRenderer>();

			GameObject plowF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			plowF.GetComponent<MeshFilter>().mesh = plowFMesh;
			plowF.name = "de6s_plow_f";

			GameObject plowR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			plowR.GetComponent<MeshFilter>().mesh = plowRMesh;
			plowR.name = "de6s_plow_r";

			GameObject handrailF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailF.GetComponent<MeshFilter>().mesh = handrailFMesh;
			handrailF.name = "de6s_handrail_f";

			GameObject handrailLF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailLF.GetComponent<MeshFilter>().mesh = handrailLFMesh;
			handrailLF.name = "de6s_handrail_lf";

			GameObject handrailRF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailRF.GetComponent<MeshFilter>().mesh = handrailRFMesh;
			handrailRF.name = "de6s_handrail_rf";

			GameObject handrailLR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailLR.GetComponent<MeshFilter>().mesh = handrailLRMesh;
			handrailLR.name = "de6s_handrail_lr";

			GameObject handrailRR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailRR.GetComponent<MeshFilter>().mesh = handrailRRMesh;
			handrailRR.name = "de6s_handrail_rr";

			GameObject handrailTopL = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailTopL.GetComponent<MeshFilter>().mesh = handrailTopLMesh;
			handrailTopL.name = "de6s_handrail_top_l";

			GameObject handrailTopR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailTopR.GetComponent<MeshFilter>().mesh = handrailTopRMesh;
			handrailTopR.name = "de6s_handrail_top_r";

			GameObject handrailR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			handrailR.GetComponent<MeshFilter>().mesh = handrailRMesh;
			handrailR.name = "de6s_handrail_r";

			GameObject headlightF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightF.GetComponent<MeshFilter>().mesh = headlightFMesh;
			headlightF.name = "de6s_headlight_f";

			GameObject headlightR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			headlightR.GetComponent<MeshFilter>().mesh = headlightRMesh;
			headlightR.name = "de6s_headlight_r";

			GameObject ditchLightRF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightRF.GetComponent<MeshFilter>().mesh = ditchLightRFMesh;
			ditchLightRF.name = "de6s_ditch_light_rf";

			GameObject ditchLightLF = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightLF.GetComponent<MeshFilter>().mesh = ditchLightLFMesh;
			ditchLightLF.name = "de6s_ditch_light_lf";

			GameObject ditchLightRR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightRR.GetComponent<MeshFilter>().mesh = ditchLightRRMesh;
			ditchLightRR.name = "de6s_ditch_light_rr";

			GameObject ditchLightLR = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			ditchLightLR.GetComponent<MeshFilter>().mesh = ditchLightLRMesh;
			ditchLightLR.name = "de6s_ditch_light_lr";

			GameObject shortHood = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			shortHood.GetComponent<MeshFilter>().mesh = shortHoodMesh;
			shortHood.name = "de6s_short_hood";

			GameObject shortHoodBase = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			shortHoodBase.GetComponent<MeshFilter>().mesh = shortHoodBaseMesh;
			shortHoodBase.name = "de6s_short_hood_base";

			GameObject longHood = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			longHood.GetComponent<MeshFilter>().mesh = longHoodMesh;
			longHood.name = "de6s_long_hood";

			GameObject longHoodBase = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			longHoodBase.GetComponent<MeshFilter>().mesh = longHoodBaseMesh;
			longHoodBase.name = "de6s_long_hood_base";

			GameObject fan1 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan1.GetComponent<MeshFilter>().mesh = fan1Mesh;
			fan1.name = "de6s_fan_1";

			GameObject fan2 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan2.GetComponent<MeshFilter>().mesh = fan2Mesh;
			fan2.name = "de6s_fan_2";

			GameObject fan3 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan3.GetComponent<MeshFilter>().mesh = fan3Mesh;
			fan3.name = "de6s_fan_3";

			GameObject fan4 = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			fan4.GetComponent<MeshFilter>().mesh = fan4Mesh;
			fan4.name = "de6s_fan_4";

			GameObject exhaust = UnityEngine.Object.Instantiate(splitLoco, splitLocoBodyLOD0.transform);
			exhaust.GetComponent<MeshFilter>().mesh = exhaustMesh;
			exhaust.name = "de6s_exhaust";

			Main.Logger.Log("Split de6_slug LOD0 mesh.");
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

		private static Mesh GetLongHoodBaseMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, shortHoodBaseLimitX, shortHoodBaseLimitY, longHoodBaseLimitZ);
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
