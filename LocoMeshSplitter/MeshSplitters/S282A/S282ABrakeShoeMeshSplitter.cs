using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LocoMeshSplitter.MeshSplitters.MeshSplittersUtil;

namespace LocoMeshSplitter.MeshSplitters.S282A
{

	//Derail Valley has one mesh for all brake shoes. This splits that mesh up into separate
	//meshes for each brake shoe.
	public class S282ABrakeShoeMeshSplitter
	{
		public static GameObject BrakeShoes
		{ get; private set; }

		private static readonly RangeFloat brakeShoeLimitX = new(-.81f, -.31f);
		private static readonly RangeFloat brakeShoeLimitY = new(0, .66f);
		private static readonly RangeFloat brakeShoeLimitZ = new(.31f, .74f);

		internal static void Init()
		{
			BrakeShoes = SplitMesh();
			UnityEngine.Object.DontDestroyOnLoad(BrakeShoes);
		}

		private static GameObject SplitMesh()
		{
			Mesh brakeShoeMesh = MeshFinder.FindMesh("s282_brake_shoes");
			if (brakeShoeMesh is null)
			{
				Main.Logger.Critical($"MeshSplitter can't find the s282 brake pad mesh");
				return null;
			}

			int[] triangles = (int[])brakeShoeMesh.triangles.Clone();
			markPartOfMesh(brakeShoeMesh.vertices, triangles, brakeShoeLimitX, brakeShoeLimitY, brakeShoeLimitZ);
			deleteUnmarkedPartOfMesh(brakeShoeMesh, triangles);

			GameObject brakePads = new GameObject("s282a_brake_shoes");
			brakePads.SetActive(false);

			GameObject brakeShoe1 = new GameObject("s282a_brake_shoe_1");
			brakeShoe1.transform.SetParent(brakePads.transform);
			brakeShoe1.transform.localScale = new Vector3(-1, 1, 1);
			brakeShoe1.transform.localPosition = new Vector3(0, 0, 8.175f);
			brakeShoe1.AddComponent<MeshFilter>().mesh = brakeShoeMesh;
			brakeShoe1.AddComponent<MeshRenderer>();

			//spawn rest of brake pads
			GameObject brakeShoe2 = UnityEngine.Object.Instantiate(brakeShoe1, brakePads.transform);
			brakeShoe2.name = "s282a_brake_shoe_2";
			brakeShoe2.transform.localPosition = new Vector3(0, 0, 6.465f);
			GameObject brakeShoe3 = UnityEngine.Object.Instantiate(brakeShoe1, brakePads.transform);
			brakeShoe3.name = "s282a_brake_shoe_3";
			brakeShoe3.transform.localPosition = new Vector3(0, 0, 4.91f);
			GameObject brakeShoe4 = UnityEngine.Object.Instantiate(brakeShoe1, brakePads.transform);
			brakeShoe4.name = "s282a_brake_shoe_4";
			brakeShoe4.transform.localPosition = new Vector3(0, 0, 3.36f);

			return brakePads;
		}
	}
}
