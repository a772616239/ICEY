using System;
using UnityEngine;

[AddComponentMenu("FantaBlade/Add A Line In Scene")]
public class LineInScene : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (this.show)
		{
			Vector3 start = this.offset + base.transform.position;
			Vector3 end = this.offset + base.transform.position + new Vector3(this.length * Mathf.Cos(0.0174532924f * this.angle), this.length * Mathf.Sin(0.0174532924f * this.angle));
			UnityEngine.Debug.DrawLine(start, end, this.color);
		}
	}

	[SerializeField]
	public bool show = true;

	[SerializeField]
	public float length = 1f;

	[SerializeField]
	public Color color = Color.cyan;

	[SerializeField]
	public Vector3 offset;

	[SerializeField]
	public float angle;
}
