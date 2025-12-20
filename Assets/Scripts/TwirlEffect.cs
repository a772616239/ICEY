using System;
using UnityEngine;

[ExecuteInEditMode]
public class TwirlEffect : MonoBehaviour
{
	public void Start()
	{
		this._material = base.GetComponent<MeshRenderer>().sharedMaterial;
	}

	private void Update()
	{
		Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, this._angle), Vector3.one);
		this._material.SetMatrix("_RotationMatrix", value);
		this._material.SetVector("_Center", new Vector2(this._center.x, this._center.y));
		this._material.SetVector("_Radius", new Vector2(this._radius.x, this._radius.y));
	}

	[SerializeField]
	private Vector2 _radius = new Vector2(0.3f, 0.3f);

	[SerializeField]
	private float _angle = 50f;

	[SerializeField]
	private Vector2 _center = new Vector2(0.5f, 0.5f);

	private Material _material;
}
