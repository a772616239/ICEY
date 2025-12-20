using System;
using UnityEngine;

public static class SRInstantiate
{
	public static T Instantiate<T>(T prefab) where T : Component
	{
		return UnityEngine.Object.Instantiate<T>(prefab);
	}

	public static GameObject Instantiate(GameObject prefab)
	{
		return UnityEngine.Object.Instantiate<GameObject>(prefab);
	}

	public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return UnityEngine.Object.Instantiate<T>(prefab, position, rotation);
	}
}
