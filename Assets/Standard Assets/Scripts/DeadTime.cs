using System;
using UnityEngine;

public class DeadTime : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.Destroy(this.destroyRoot ? base.transform.root.gameObject : base.gameObject, this.deadTime);
	}

	public float deadTime = 1.5f;

	public bool destroyRoot;
}
