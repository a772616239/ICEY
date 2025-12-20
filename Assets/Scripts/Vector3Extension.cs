using System;
using UnityEngine;

public static class Vector3Extension
{
	public static Vector3 Rotate(this Vector3 v, float degrees)
	{
		return Quaternion.Euler(0f, 0f, degrees) * v;
	}

	public static Vector3 Times(this Vector3 v, Vector3 other)
	{
		return new Vector3(v.x * other.x, v.y * other.y, v.z * other.z);
	}
}
