using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class MultiTapButton : Button
	{
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (Time.unscaledTime - this._lastTap > this.ResetTime)
			{
				this._tapCount = 0;
			}
			this._lastTap = Time.unscaledTime;
			this._tapCount++;
			if (this._tapCount == this.RequiredTapCount)
			{
				base.OnPointerClick(eventData);
				this._tapCount = 0;
			}
		}

		private float _lastTap;

		private int _tapCount;

		public int RequiredTapCount = 3;

		public float ResetTime = 0.5f;
	}
}
