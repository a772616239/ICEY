using System;
using UnityEngine;

public class LightPoint : BaseBehaviour
{
	private void Awake()
	{
		this.Position = base.transform.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = this.color;
		Gizmos.DrawSphere(base.transform.position, 0.2f);
		for (int i = 1; i <= 8; i++)
		{
			Gizmos.DrawSphere(new Vector3(Mathf.Cos(45f * (float)i * 0.0174532924f) * 0.3f, Mathf.Sin(45f * (float)i * 0.0174532924f) * 0.3f) + base.transform.position, 0.05f);
		}
	}

	[SerializeField]
	public Color color = Color.white;

	[SerializeField]
	[Range(0f, 8f)]
	public float intensity = 1f;

	public Vector3 Position;
}
