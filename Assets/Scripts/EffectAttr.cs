using System;
using UnityEngine;

[Serializable]
public class EffectAttr
{
	public int id;

	public bool isFollow;

	public int maxCount = 20;

	public int effectStartCount = 1;

	public EffectController.FXRotationCondition rotation;

	public Transform effect;

	public Vector3 scale;

	[HideInInspector]
	public string prefabPath;

	public string functionName;
}
