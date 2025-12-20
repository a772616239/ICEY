using System;
using UnityEngine;

public class CameraAnimControlTools : BaseBehaviour
{
	public void CameraShake(AnimationEvent animationEvent)
	{
		int intParameter = animationEvent.intParameter;
		float floatParameter = animationEvent.floatParameter;
		R.Camera.Controller.CameraShake((float)intParameter / 60f, floatParameter, CameraController.ShakeTypeEnum.Rect, false);
	}
}
