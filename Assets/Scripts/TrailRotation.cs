using System;
using UnityEngine;

public class TrailRotation : MonoBehaviour
{
	private void Start()
	{
		this.rotationVector = new Vector3(this.rotationSpeedX, this.rotationSpeedY, this.rotationSpeedZ);
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(this.rotationSpeedX, this.rotationSpeedY, this.rotationSpeedZ) * Time.deltaTime);
	}

	private void OnDisable()
	{
		base.transform.localPosition = Vector3.zero;
	}

	[SerializeField]
	public float rotationSpeedX = 90f;

	[SerializeField]
	public float rotationSpeedY = 90f;

	[SerializeField]
	public float rotationSpeedZ = 90f;

	public Vector3 rotationVector;
}
