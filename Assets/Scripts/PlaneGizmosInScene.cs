using System;
using UnityEngine;

public class PlaneGizmosInScene : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (string.IsNullOrEmpty(this.property))
		{
			this.property = base.name;
		}
		if (!this._show)
		{
			return;
		}
		this._color.a = this._alpha;
		Gizmos.color = this._color;
		Gizmos.DrawCube(base.transform.position, this._size);
		DebugX.DrawText(this.property, base.transform.position);
	}

	[SerializeField]
	private bool _show = true;

	[SerializeField]
	private string property;

	[SerializeField]
	private Vector2 _size = new Vector2(20f, 10f);

	[SerializeField]
	private Color _color = Color.cyan;

	[Range(0f, 1f)]
	[SerializeField]
	private float _alpha = 0.2f;
}
