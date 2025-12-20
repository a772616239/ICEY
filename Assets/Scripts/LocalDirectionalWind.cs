using System;
using UnityEngine;

public class LocalDirectionalWind : LocalWind
{
	private void Awake()
	{
		this.windRenderers = base.transform.GetComponentsInChildren<MeshRenderer>();
	}

	public override Vector2 GetDir(IBlowable blowable, Vector2 pos)
	{
		return this.direction.normalized;
	}

	public override void OnReset()
	{
	}

	public override void OnPause(bool isPause)
	{
	}

	public Vector2 direction = new Vector2(0f, 1f);

	private MeshRenderer[] windRenderers;
}
