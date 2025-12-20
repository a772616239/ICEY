using System;
using UnityEngine;

public class CameraFV : BaseBehaviour
{
	private void Awake()
	{
		this.Position = base.transform.position;
	}

	private void OnDrawGizmos()
	{
		DebugX.DrawText("FV" + this.fv, base.transform.position);
	}

	[Range(10f, 100f)]
	[SerializeField]
	public float fv = 52f;

	[Range(-5f, 5f)]
	[SerializeField]
	public float offsetY;

	public Vector3 Position;
}
