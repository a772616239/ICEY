using System;
using UnityEngine;

public static class MotionBlurMeshUtility
{
	public static void CopyMesh(Mesh a_rMeshFrom, ref Mesh a_rMeshTo)
	{
		if (a_rMeshTo == null)
		{
			a_rMeshTo = new Mesh();
		}
		a_rMeshTo.Clear();
		if (a_rMeshFrom == null)
		{
			return;
		}
		a_rMeshTo.vertices = a_rMeshFrom.vertices;
		a_rMeshTo.colors = a_rMeshFrom.colors;
		a_rMeshTo.uv = a_rMeshFrom.uv;
		a_rMeshTo.uv2 = a_rMeshFrom.uv2;
		a_rMeshTo.normals = a_rMeshFrom.normals;
		a_rMeshTo.tangents = a_rMeshFrom.tangents;
		int subMeshCount = a_rMeshFrom.subMeshCount;
		a_rMeshTo.subMeshCount = subMeshCount;
		for (int i = 0; i < subMeshCount; i++)
		{
			a_rMeshTo.SetTriangles(a_rMeshFrom.GetTriangles(i), i);
		}
	}
}
