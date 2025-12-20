using System;
using UnityEngine;

public struct AreaRect
{
	public AreaRect(Rect r)
	{
		this.xMin = r.xMin;
		this.xMax = r.xMax;
		this.yMin = r.yMin;
		this.yMax = r.yMax;
	}

	public Rect ToRect()
	{
		return Rect.MinMaxRect(this.xMin, this.yMin, this.xMax, this.yMax);
	}

	public float xMin;

	public float xMax;

	public float yMin;

	public float yMax;
}
