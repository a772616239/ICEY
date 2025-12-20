using System;
using UnityEngine;

namespace FBInput
{
	public class FBSwipe : AbstractTouchElement
	{
		public Vector2 Direction { get; private set; }

		protected override void OnTouchDown(Vector2 touchPosition)
		{
			this._touchStartPosition = touchPosition;
			this._timeOnTouchDown = Time.time;
		}

		protected override void OnTouchUp(Vector2 touchPosition)
		{
			Vector2 vector = touchPosition - this._touchStartPosition;
			bool flag = vector.sqrMagnitude >= this.MinTriggerDistance * this.MinTriggerDistance;
			bool flag2 = Time.time - this._timeOnTouchDown <= this.MaxTriggerTime;
			this.Direction = ((!flag || !flag2) ? Vector2.zero : vector.normalized);
		}

		protected override bool TestTouchArea(Vector2 screenPosition)
		{
			return screenPosition.x < (float)UITools.ScreenWidth / 2f;
		}

		private readonly float MinTriggerDistance = 20f;

		private readonly float MaxTriggerTime = 0.5f;

		private Vector2 _touchStartPosition;

		private float _timeOnTouchDown;
	}
}
