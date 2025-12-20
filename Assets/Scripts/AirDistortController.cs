using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AirDistortController : MonoBehaviour
{
	private void Start()
	{
		this.srender = base.GetComponent<SpriteRenderer>();
		this.material = this.srender.material;
		this.material.SetFloat("_DispScrollSpeedX", this.X方向移动速度);
		this.material.SetFloat("_DispScrollSpeedY", this.Y方向移动速度);
		this.material.SetFloat("_StrengthX", this.X方向扭曲强度);
		this.material.SetFloat("_StrengthY", this.Y方向扭曲强度);
		this.material.SetFloat("_Strength", this.总扭曲强度);
	}

	public float X方向移动速度;

	public float Y方向移动速度;

	public float X方向扭曲强度;

	public float Y方向扭曲强度 = 1f;

	public float 总扭曲强度 = 1f;

	private Material material;

	private SpriteRenderer srender;
}
