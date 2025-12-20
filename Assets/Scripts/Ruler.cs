using System;
using UnityEngine;

public class Ruler : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (string.IsNullOrEmpty(this._property))
		{
			this._property = base.name;
		}
		if (!this._show)
		{
			return;
		}
		this._color.a = this._alpha;
		Gizmos.color = this._color;
		this.DrawCube(base.transform.position + this._offset, this._size, this._angle);
		DebugX.DrawText(this._property + this._size, base.transform.position);
	}

	private void DrawCube(Vector3 center, Vector2 size, float angle)
	{
		float num = size.x / 2f;
		float num2 = size.y / 2f;
		Vector3 vector = this.VectorRotate(new Vector2(-num, num2), angle);
		Vector3 vector2 = this.VectorRotate(new Vector2(num, num2), angle);
		Vector3 vector3 = this.VectorRotate(new Vector2(-num, -num2), angle);
		Vector3 vector4 = this.VectorRotate(new Vector2(num, -num2), angle);
		switch (this._ancor)
		{
		case Ruler.AncorEnum.TopLeft:
			center -= vector;
			break;
		case Ruler.AncorEnum.TopRight:
			center -= vector2;
			break;
		case Ruler.AncorEnum.BottomLeft:
			center -= vector3;
			break;
		case Ruler.AncorEnum.BottomRight:
			center -= vector4;
			break;
		}
		vector += center;
		vector2 += center;
		vector3 += center;
		vector4 += center;
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector4);
		Gizmos.DrawLine(vector4, vector3);
		Gizmos.DrawLine(vector3, vector);
	}

	private Vector3 VectorRotate(Vector2 vector, float angle)
	{
		angle *= 0.0174532924f;
		float x = vector.x;
		float y = vector.y;
		float x2 = x * Mathf.Cos(angle) + y * Mathf.Sin(angle);
		float y2 = -x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
		return new Vector3(x2, y2);
	}

	[SerializeField]
	private bool _show = true;

	[SerializeField]
	private string _property;

	[SerializeField]
	private Ruler.AncorEnum _ancor = Ruler.AncorEnum.BottomLeft;

	[SerializeField]
	private Vector2 _size = new Vector2(20f, 10f);

	[SerializeField]
	private Vector3 _offset;

	[SerializeField]
	private float _angle;

	[SerializeField]
	private Color _color = Color.cyan;

	[Range(0f, 1f)]
	[SerializeField]
	private float _alpha = 0.5f;

	private enum AncorEnum
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
