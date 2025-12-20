using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class FullScreenColorMaskController : SingletonMono<FullScreenColorMaskController>
{
	private void Awake()
	{
		this.render = base.GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		this.render.color = this.color;
	}

	private void OnEnable()
	{
		this.ResetToDefault();
	}

	private void ResetToDefault()
	{
		this.render.color = Color.clear;
	}

	private SpriteRenderer render;

	public Color color = Color.clear;
}
