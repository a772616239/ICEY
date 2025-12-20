using System;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(0f, 0f, Time.deltaTime * this.rotationSpeed);
	}

	public float rotationSpeed = 1f;
}
