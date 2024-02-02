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

		//removes all the extra vertices in a mesh that are no longer needed
		//taken from https://forum.unity.com/threads/remove-vertices-that-are-not-in-triangle-solved.342335/
		internal static void removeUnusedVertices(Mesh aMesh)
		{
			Vector3[] vertices = aMesh.vertices;
			Vector3[] normals = aMesh.normals;
			Vector4[] tangents = aMesh.tangents;
			Vector2[] uv = aMesh.uv;
			Vector2[] uv2 = aMesh.uv2;
			List<int> indices = new List<int>();

			Dictionary<int, int> vertMap = new Dictionary<int, int>(vertices.Length);

			List<Vector3> newVerts = new List<Vector3>(vertices.Length);
			List<Vector3> newNormals = new List<Vector3>(vertices.Length);
			List<Vector4> newTangents = new List<Vector4>(vertices.Length);
			List<Vector2> newUV = new List<Vector2>(vertices.Length);
			//List<Vector2> newUV2 = new List<Vector2>(vertices.Length);

			System.Func<int, int> remap = (int aIndex) =>
			{
				int res = -1;
				if (!vertMap.TryGetValue(aIndex, out res))
				{
					res = newVerts.Count;
					vertMap.Add(aIndex, res);
					newVerts.Add(vertices[aIndex]);
					if (normals != null && normals.Length > 0)
						newNormals.Add(normals[aIndex]);
					if (tangents != null && tangents.Length > 0)
						newTangents.Add(tangents[aIndex]);
					if (uv != null && uv.Length > 0)
						newUV.Add(uv[aIndex]);
					//if (uv2 != null && uv2.Length > 0)
					//newUV2.Add(uv2[aIndex]);
				}
				return res;
			};
			for (int subMeshIndex = 0; subMeshIndex < aMesh.subMeshCount; subMeshIndex++)
			{
				var topology = aMesh.GetTopology(subMeshIndex);
				indices.Clear();
				aMesh.GetIndices(indices, subMeshIndex);
				for (int i = 0; i < indices.Count; i++)
				{
					indices[i] = remap(indices[i]);
				}
				aMesh.SetIndices(indices, topology, subMeshIndex);
			}
			aMesh.SetVertices(newVerts);
			if (newNormals.Count > 0)
				aMesh.SetNormals(newNormals);
			if (newTangents.Count > 0)
				aMesh.SetTangents(newTangents);
			if (newUV.Count > 0)
				aMesh.SetUVs(0, newUV);
			//if (newUV2.Count > 0)
			//aMesh.SetUVs(1, newUV2);
		}
	}
}
