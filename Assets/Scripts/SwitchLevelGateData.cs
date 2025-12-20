using System;
using UnityEngine;

[Serializable]
public class SwitchLevelGateData
{
	[Header("目标场景名")]
	public string ToLevelId;

	[Header("目标场景大门编号")]
	public int ToId;

	[Header("自身大门编号")]
	public int MyId;

	[NonSerialized]
	public SceneGate.OpenType OpenType = SceneGate.OpenType.None;

	[NonSerialized]
	public bool InAir;

	[NonSerialized]
	public Vector3 SelfPosition;

	[NonSerialized]
	public Vector3 TargetPosition;

	[NonSerialized]
	public Vector2 TriggerSize;
}
