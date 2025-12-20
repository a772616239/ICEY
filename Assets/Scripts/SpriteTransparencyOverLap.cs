using System;
using ExtensionMethods;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTransparencyOverLap : BaseBehaviour
{
	private void Awake()
	{
		this._render = base.GetComponent<SpriteRenderer>();
		this._bounds = this._render.bounds;
		this._rect = new Rect(this._bounds.min, this._bounds.size);
		this._color = this._render.color;
	}

	private void Update()
	{
		Vector3 position = R.Camera.Camera.WorldToViewportPoint(R.Player.Transform.position);
		bool flag = this._bounds.IntersectRay(R.Camera.Camera.ViewportPointToRay(position));
		if ((flag && this._color.a > this.finalAlpha) || (!flag && this._color.a < 1f))
		{
			float num = (1f - this.finalAlpha) / 0.5f;
			float num2 = this._color.a + (float)((!flag) ? 1 : -1) * num * Time.deltaTime;
			num2 = Mathf.Clamp(num2, this.finalAlpha, 1f);
			if (Math.Abs(num2 - this._color.a) > 1.401298E-45f)
			{
				this._color = this._color.SetAlpha(num2);
				this._render.color = this._color;
			}
		}
	}

	private SpriteRenderer _render;

	private Bounds _bounds;

	private Rect _rect;

	private Color _color;

	public float finalAlpha = 0.5f;
}
