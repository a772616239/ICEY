using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CheckPointData
{
	public int PointId;

	public Vector2 TriggerSize = new Vector2(3f, 3f);

	public Vector2 TriggerOffset = Vector2.zero;

	public bool HasWall;

	public bool canMultiTrigger;

	public float DelayTime = 0.5f;

	public Vector3 LeftWall = Vector3.zero;

	public Vector3 RightWall = Vector3.zero;

	public List<int> battleRoundId = new List<int>();
}
