using System;
using UnityEngine;

public class CameraEffectMotionBlurProxy : ControllerProxyBase<CameraController>
{
	protected override void ThisStart()
	{
		this._autoEnable = false;
	}

	protected override void OnThisEnable()
	{
		base.OnThisEnable();
		if (object.ReferenceEquals(ControllerProxyBase<CameraController>.effectLock, this) && base.effect != null)
		{
			base.effect.OpenMotionBlur(this.frame / 60f, this.scale, base.transform.position);
		}
	}

	protected override void OnThisDisable()
	{
		if (object.ReferenceEquals(ControllerProxyBase<CameraController>.effectLock, this) && base.effect != null)
		{
			base.effect.CloseMotionBlur();
		}
	}

	[SerializeField]
	private float scale = 1f;

	[SerializeField]
	private uint frame = 8u;
}
