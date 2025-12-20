using System;
using System.Collections.Generic;
using UnityEngine;

public static class TrnasformExtension
{
	public static Bounds TransformBounds(this Transform self, Bounds bounds)
	{
		Vector3 center = self.TransformPoint(bounds.center);
		List<Vector3> corners = bounds.GetCorners(true);
		Bounds result = new Bounds(center, Vector3.zero);
		foreach (Vector3 position in corners)
		{
			result.Encapsulate(self.TransformPoint(position));
		}
		return result;
	}

	public static Bounds InverseTransformBounds(this Transform self, Bounds bounds)
	{
		Vector3 center = self.InverseTransformPoint(bounds.center);
		List<Vector3> corners = bounds.GetCorners(true);
		Bounds result = new Bounds(center, Vector3.zero);
		foreach (Vector3 position in corners)
		{
			result.Encapsulate(self.InverseTransformPoint(position));
		}
		return result;
	}
}
