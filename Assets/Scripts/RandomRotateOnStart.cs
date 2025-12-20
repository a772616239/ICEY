using System;
using UnityEngine;

public class RandomRotateOnStart : BaseBehaviour
{
	private void Start()
	{
		this.t = base.transform;
		this.t.Rotate(this.NormalizedRotateVector * (float)UnityEngine.Random.Range(0, 360));
		this.isInitialized = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.t.Rotate(this.NormalizedRotateVector * (float)UnityEngine.Random.Range(0, 360));
		}
	}

	public Vector3 NormalizedRotateVector = new Vector3(0f, 1f, 0f);

	private Transform t;

	private bool isInitialized;
}
