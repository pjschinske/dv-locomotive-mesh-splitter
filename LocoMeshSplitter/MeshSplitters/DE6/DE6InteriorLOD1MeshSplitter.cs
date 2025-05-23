using System;
using System.Collections.Generic;
using System.Text;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;
using UnityEngine;

namespace LocoMeshSplitter.MeshSplitters.DE6
{
	internal class DE6InteriorLOD1MeshSplitter
	{
		public static GameObject SplitLocoInteriorLOD1
		{ get; private set; }

		private static readonly RangeFloat speedoLimitX = new(-0.94f, -0.64f);
		private static readonly RangeFloat speedoLimitY = new(2.42f, 3.01f);
		private static readonly RangeFloat speedoLimitZ = new(6.66f, 6.83f);

		//the control stand with the air brakes
		private static readonly RangeFloat controlStand1LimitX = new(-0.9f, -0.3f);
		private static readonly RangeFloat controlStand1LimitY = new(1.5f, 3.1f);
		private static readonly RangeFloat controlStand1LimitZ = new(5.6f, 6.121f);

		//the control stand with the throttle, reverser, tach, etc.
		private static readonly RangeFloat controlStand2LimitX = new(-0.96f, -0.33f);
		private static readonly RangeFloat controlStand2LimitY = new(1.5f, 3f);
		private static readonly RangeFloat controlStand2LimitZ = new(6.121f, 6.8f);

		private static readonly RangeFloat handrailLimitX = new(-0.45f, -0.378f);
		private static readonly RangeFloat handrailLimitY = new(1.58f, 2.55f);
		private static readonly RangeFloat handrailLimitZ = new(6.41f, 6.85f);

		private static readonly RangeFloat heaterHoseLimitX = new(-1f, -0.63f);
		private static readonly RangeFloat heaterHoseLimitY = new(1.75f, 1.9f);
		private static readonly RangeFloat heaterHoseLimitZ = new(6.63f, 6.85f);

		private static readonly RangeFloat heaterLimitX = new(-1.45f, -0.96f);
		private static readonly RangeFloat heaterLimitY = new(1.5f, 2.28f);
		private static readonly RangeFloat heaterLimitZ = new(6.5f, 6.81f);

		private static readonly RangeFloat tableLimitX = new(-1.45f, -0.89f);
		private static readonly RangeFloat tableLimitY = new(2.28f, 2.37f);
		private static readonly RangeFloat tableLimitZ = new(6.41f, 6.84f);

		private static readonly RangeFloat toiletDoorLimitX = new(-0.3f, 0.3f);
		private static readonly RangeFloat toiletDoorLimitY = new(1.19f, 2.82f);
		private static readonly RangeFloat toiletDoorLimitZ = new(6.85f, 7f);

		private static readonly RangeFloat cabinetLimitX = new(0.35f, 0.78f);
		private static readonly RangeFloat cabinetLimitY = new(1.5f, 2.7f);
		private static readonly RangeFloat cabinetLimitZ = new(6.39f, 6.84f);

		private static readonly RangeFloat cabLightLimitX = new(-1.25f, -1f);
		private static readonly RangeFloat cabLightLimitY = new(3.5f, 3.7f);
		private static readonly RangeFloat cabLightLimitZ = new(5.7f, 6.1f);

		public const string CAB_MESH_NAME = "cab_LOD1";
		public const int CAB_MESH_PATHID = 4110;

		internal static void Init()
		{
			SplitLocoInteriorLOD1 = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(SplitLocoInteriorLOD1);
		}

		private static GameObject SplitMesh()
		{
			Mesh locoMesh = MeshFinder.FindMesh(CAB_MESH_NAME, CAB_MESH_PATHID);
			if (locoMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the DE6 interior LOD0 mesh");
				return null;
			}
			Main.Logger.Log($"Splitting DE6 interior LOD0 mesh...");

			GameObject splitLocoInteriorLOD1 = new GameObject(CAB_MESH_NAME);
			splitLocoInteriorLOD1.SetActive(false);

			Mesh tableMesh = GetTableMesh(locoMesh);
			Mesh heaterMesh = GetHeaterMesh(locoMesh);
			int[] triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, tableLimitX, tableLimitY, tableLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, heaterLimitX, heaterLimitY, heaterLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh controlStand1Mesh = GetControlStand1Mesh(locoMesh);
			Mesh speedoMesh = GetSpeedoMesh(locoMesh);
			Mesh handrailMesh = GetHandrailMesh(locoMesh);
			Mesh heaterHoseMesh = GetHeaterHoseMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, controlStand1LimitX, controlStand1LimitY, controlStand1LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, speedoLimitX, speedoLimitY, speedoLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, handrailLimitX, handrailLimitY, handrailLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, heaterHoseLimitX, heaterHoseLimitY, heaterHoseLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			Mesh controlStand2Mesh = GetControlStand2Mesh(locoMesh);
			Mesh toiletDoorMesh = GetToiletDoorMesh(locoMesh);
			Mesh cabinetMesh = GetCabinetMesh(locoMesh);
			Mesh cabLightMesh = GetCabLightMesh(locoMesh);
			triangles = (int[])locoMesh.triangles.Clone();
			markPartOfMesh(locoMesh.vertices, triangles, controlStand2LimitX, controlStand2LimitY, controlStand2LimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, toiletDoorLimitX, toiletDoorLimitY, toiletDoorLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cabinetLimitX, cabinetLimitY, cabinetLimitZ);
			markPartOfMesh(locoMesh.vertices, triangles, cabLightLimitX, cabLightLimitY, cabLightLimitZ);
			deleteMarkedPartOfMesh(locoMesh, triangles);

			GameObject splitInterior = new GameObject("de6_interior");
			splitInterior.transform.SetParent(splitLocoInteriorLOD1.transform);
			splitInterior.transform.localScale = new Vector3(-1, 1, 1);
			splitInterior.AddComponent<MeshFilter>().mesh = locoMesh;
			splitInterior.AddComponent<MeshRenderer>();

			GameObject controlStand1 = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			controlStand1.GetComponent<MeshFilter>().mesh = controlStand1Mesh;
			controlStand1.name = "de6_control_stand_1";

			GameObject controlStand2 = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			controlStand2.GetComponent<MeshFilter>().mesh = controlStand2Mesh;
			controlStand2.name = "de6_control_stand_2";

			GameObject speedo = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			speedo.GetComponent<MeshFilter>().mesh = speedoMesh;
			speedo.name = "de6_speedometer";

			GameObject handrail = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			handrail.GetComponent<MeshFilter>().mesh = handrailMesh;
			handrail.name = "de6_handrail";

			GameObject heater = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			heater.GetComponent<MeshFilter>().mesh = heaterMesh;
			heater.name = "de6_heater";

			GameObject heaterHose = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			heaterHose.GetComponent<MeshFilter>().mesh = heaterHoseMesh;
			heaterHose.name = "de6_heater_hose";

			GameObject table = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			table.GetComponent<MeshFilter>().mesh = tableMesh;
			table.name = "de6_table";

			GameObject toiletDoor = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			toiletDoor.GetComponent<MeshFilter>().mesh = toiletDoorMesh;
			toiletDoor.name = "de6_front_hood_door";

			GameObject cabinet = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			cabinet.GetComponent<MeshFilter>().mesh = cabinetMesh;
			cabinet.name = "de6_cabinet";

			GameObject cabLight = UnityEngine.Object.Instantiate(splitInterior, splitLocoInteriorLOD1.transform);
			cabLight.GetComponent<MeshFilter>().mesh = cabLightMesh;
			cabLight.name = "de6_cab_light";

			Main.Logger.Log("Split LocoDE6 interior LOD0 mesh.");
			return splitLocoInteriorLOD1;
		}

		private static Mesh GetSpeedoMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, speedoLimitX, speedoLimitY, speedoLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHandrailMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, handrailLimitX, handrailLimitY, handrailLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeaterHoseMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, heaterHoseLimitX, heaterHoseLimitY, heaterHoseLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetHeaterMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, heaterLimitX, heaterLimitY, heaterLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetTableMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, tableLimitX, tableLimitY, tableLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetControlStand1Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, controlStand1LimitX, controlStand1LimitY, controlStand1LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetControlStand2Mesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, controlStand2LimitX, controlStand2LimitY, controlStand2LimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetToiletDoorMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, toiletDoorLimitX, toiletDoorLimitY, toiletDoorLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCabinetMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, cabinetLimitX, cabinetLimitY, cabinetLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}

		private static Mesh GetCabLightMesh(Mesh de6Mesh)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(de6Mesh);
			int[] triangles = (int[])mesh.triangles.Clone();
			markPartOfMesh(mesh.vertices, triangles, cabLightLimitX, cabLightLimitY, cabLightLimitZ);
			deleteUnmarkedPartOfMesh(mesh, triangles);

			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			return mesh;
		}
	}
}
