using System;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtension
{
	public static List<Vector3> GetCorners(this Bounds obj, bool includePosition = true)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = -1; i <= 1; i += 2)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				list.Add(((!includePosition) ? Vector3.zero : obj.center) + (obj.size / 2f).Times(new Vector3((float)i, (float)j)));
			}
		}
		return list;
	}
}
