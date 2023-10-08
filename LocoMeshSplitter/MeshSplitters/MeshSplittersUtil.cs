using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LocoMeshSplitter.MeshSplitters
{
	internal static class MeshSplittersUtil
	{
		internal struct Range
		{
			public readonly int start;//inclusive
			public readonly int end;//exclusive

			public Range(int start, int end)
			{
				this.start = start;
				this.end = end;
			}
			public bool contains(int x)
			{
				return x >= start && x < end;
			}

			public override string ToString()
			{
				return $"{base.ToString()}: start={start}, end={end}";
			}
		}

		internal struct RangeFloat
		{
			public readonly float start;//inclusive
			public readonly float end;//exclusive

			public RangeFloat(float start, float end)
			{
				this.start = start;
				this.end = end;
			}
			public bool contains(float x)
			{
				return x >= start && x <= end;
			}

			public override string ToString()
			{
				return $"{base.ToString()}: start={start}, end={end}";
			}
		}

		//mark part of mesh, to be either kept or destroyed with deleteMarkedPartOfMesh or
		//deleteUnmarkedPartOfMesh.
		internal static void markPartOfMesh(Vector3[] vertices, int[] triangles, RangeFloat xRange, RangeFloat yRange, RangeFloat zRange)
		{
			//Main.Logger.Log(xRange.ToString());
			//Main.Logger.Log(yRange.ToString());
			//Main.Logger.Log(zRange.ToString());

			vertices = (Vector3[])vertices.Clone();
			//if the vertex is out of range, set it to zero
			for (int i = 0; i < vertices.Length; i++)
			{
				if (xRange.contains(vertices[i].x)
					&& yRange.contains(vertices[i].y)
					&& zRange.contains(vertices[i].z))
				{
					vertices[i] = Vector3.zero;
				}
			}
			//triangles that contain out-of-range vertices get marked
			for (int i = 0; i < triangles.Length - 2; i += 3)
			{
				//Only set to negative 1 if it hasn't been already and all it's vertices have been set to zero
				if (triangles[i] != -1
					&& vertices[triangles[i]].Equals(Vector3.zero)
					&& vertices[triangles[i + 1]].Equals(Vector3.zero)
					&& vertices[triangles[i + 2]].Equals(Vector3.zero))
				{
					triangles[i] = -1;
					triangles[i + 1] = -1;
					triangles[i + 2] = -1;
				}
			}
		}

		internal static void deleteMarkedPartOfMesh(Mesh mesh, int[] markedTriangles)
		{
			mesh.triangles = markedTriangles.Where((source, index) => source != -1).ToArray();
		}

		internal static void deleteUnmarkedPartOfMesh(Mesh mesh, int[] markedTriangles)
		{
			mesh.triangles = mesh.triangles.Where((source, index) => markedTriangles[index] == -1).ToArray();
		}

		//hide the part of a mesh enclosed in three ranges
		internal static void hidePartOfMesh(Mesh mesh, RangeFloat xRange, RangeFloat yRange, RangeFloat zRange)
		{
			Vector3[] vertices = (Vector3[])mesh.vertices.Clone();
			int[] triangles = (int[])mesh.triangles.Clone();
			//if the vertex is out of range, set it to zero
			for (int i = 0; i < vertices.Length; i++)
			{
				if (xRange.contains(vertices[i].x)
					&& yRange.contains(vertices[i].y)
					&& zRange.contains(vertices[i].z))
				{
					vertices[i] = Vector3.zero;
				}
			}
			//triangles that contain out-of-range vertices get removed
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (vertices[triangles[i]].Equals(Vector3.zero)
					&& vertices[triangles[i + 1]].Equals(Vector3.zero)
					&& vertices[triangles[i + 2]].Equals(Vector3.zero))
				{
					triangles[i] = -1;
					triangles[i + 1] = -1;
					triangles[i + 2] = -1;
				}
			}
			triangles = triangles.Where((source, index) => source != -1).ToArray();
			mesh.triangles = triangles;
		}
	}
}
