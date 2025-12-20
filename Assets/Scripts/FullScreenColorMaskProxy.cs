using System;
using UnityEngine;

public class FullScreenColorMaskProxy : ControllerProxyBase<FullScreenColorMaskController>
{
	protected override void ResetToDefault()
	{
		this.color = Color.clear;
	}

	protected override void ThisUpdate()
	{
		base.effect.color = this.color;
	}

	public Color color;
}
