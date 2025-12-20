using System;
using SRF;
using UnityEngine;

namespace SRDebugger.UI.Other
{
	public class LoadingSpinnerBehaviour : SRMonoBehaviour
	{
		private void Update()
		{
			this._dt += Time.unscaledDeltaTime;
			Vector3 eulerAngles = base.CachedTransform.localRotation.eulerAngles;
			float num = eulerAngles.z;
			float num2 = this.SpinDuration / (float)this.FrameCount;
			bool flag = false;
			while (this._dt > num2)
			{
				num -= 360f / (float)this.FrameCount;
				this._dt -= num2;
				flag = true;
			}
			if (flag)
			{
				base.CachedTransform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, num);
			}
		}

		private float _dt;

		public int FrameCount = 12;

		public float SpinDuration = 0.8f;
	}
}
