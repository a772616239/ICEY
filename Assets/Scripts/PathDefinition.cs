using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathDefinition : BaseBehaviour
{
	public IEnumerator<Transform> GetPathEnumerator()
	{
		if (this.Points == null || this.Points.Length < 1)
		{
			yield break;
		}
		int direction = 1;
		int index = 0;
		for (;;)
		{
			yield return this.Points[index];
			if (this.Points.Length != 1)
			{
				if (index <= 0)
				{
					direction = 1;
				}
				else if (index >= this.Points.Length - 1)
				{
					direction = -1;
				}
				index += direction;
			}
		}
		yield break;
	}

	public void OnDrawGizmos()
	{
		if (this.Points == null || this.Points.Length < 2)
		{
			return;
		}
		List<Transform> list = (from t in this.Points
		where t != null
		select t).ToList<Transform>();
		if (list.Count < 2)
		{
			return;
		}
		for (int i = 1; i < list.Count; i++)
		{
			Gizmos.DrawLine(list[i - 1].position, list[i].position);
		}
	}

	public Transform[] Points;
}
