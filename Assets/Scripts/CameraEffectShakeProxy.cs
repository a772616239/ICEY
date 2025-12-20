using System;

public class CameraEffectShakeProxy : ControllerProxyBase<CameraController>
{
	protected override void ThisStart()
	{
		this._autoEnable = false;
	}

	protected override void OnThisEnable()
	{
		base.effect.CameraShake(this.frame / 60f, this.strength, CameraController.ShakeTypeEnum.Rect, false);
	}

	public uint frame = 10u;

	public float strength = 0.3f;
}
