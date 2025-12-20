using System;
using UnityEngine;

public class WaterUvAnimation : MonoBehaviour
{
	private void Awake()
	{
		this.mat = base.GetComponent<Renderer>().materials[this.MaterialNomber];
	}

	private void Update()
	{
		if (this.IsReverse)
		{
			this.offset -= Time.deltaTime * this.Speed;
			if (this.offset < 0f)
			{
				this.offset = 1f;
			}
		}
		else
		{
			this.offset += Time.deltaTime * this.Speed;
			if (this.offset > 1f)
			{
				this.offset = 0f;
			}
		}
		Vector2 value = new Vector2(0f, this.offset);
		this.mat.SetTextureOffset("_BumpMap", value);
		this.mat.SetFloat("_OffsetYHeightMap", this.offset);
	}

	public bool IsReverse;

	public float Speed = 1f;

	public int MaterialNomber;

	private Material mat;

	private float deltaFps;

	private bool isVisible;

	private bool isCorutineStarted;

	private float offset;

	private float delta;
}
