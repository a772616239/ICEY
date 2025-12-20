using System;
using UnityEngine;
using Xft;

public class XffectHelper : MonoBehaviour
{
	public void SetVortexSpeed(float speed)
	{
		this.distortionLayer.VortexMag = speed;
	}

	public void SetDir(int dir)
	{
		if (dir > 0)
		{
			this.faceTrans.localPosition = new Vector3(0f, 0f, -100000f);
			this.distortionLayer.EmitPoint = new Vector3(-50f, 70f, 0f);
		}
		else
		{
			this.faceTrans.localPosition = new Vector3(0f, 0f, 100000f);
			this.distortionLayer.EmitPoint = new Vector3(50f, 70f, 0f);
			this.distortionLayer.VortexMag = -this.distortionLayer.VortexMag;
		}
	}

	public EffectLayer distortionLayer;

	public Transform faceTrans;
}
