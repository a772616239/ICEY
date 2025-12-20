using System;
using UnityEngine;

namespace FBInput
{
	public class FBJoystick : AbstractTouchElement
	{
		public Vector2 Axis { get; private set; }

		private void Awake()
		{
			this._radiusSqr = this.Radius * this.Radius;
			this._transform = base.transform;
			this.SetDefaultState();
		}

		protected override void OnTouchDown(Vector2 touchPosition)
		{
			this._transform.localPosition = touchPosition;
			this._anchor = touchPosition;
			this.BackgroundSprite.color = Color.white;
			this.ThumbSprite.color = Color.white;
		}

		protected override void OnTouchUp(Vector2 localPosition)
		{
			this.SetDefaultState();
		}

		protected override void OnTouch(Vector2 localPosition)
		{
			Vector2 vector = localPosition - this._anchor;
			if (vector.sqrMagnitude < this._radiusSqr)
			{
				vector /= this.Radius;
			}
			else
			{
				vector = vector.normalized;
			}
			if (vector.sqrMagnitude >= FBJoystick.DeadZoneRadiusSqr)
			{
				this.Axis = ((!base.IsActive) ? Vector2.zero : vector);
			}
			this.ThumbSprite.transform.localPosition = this.Axis * this.Radius;
		}

		private void SetDefaultState()
		{
			this.Axis = Vector2.zero;
			this.BackgroundSprite.color = new Color(1f, 1f, 1f, 0.5f);
			this.ThumbSprite.color = new Color(1f, 1f, 1f, 0.5f);
		}

		protected override bool TestTouchArea(Vector2 screenPosition)
		{
			return screenPosition.x < (float)UITools.ScreenWidth / 2f;
		}

		public static readonly float DeadZoneRadius = 0.2f;

		private static readonly float DeadZoneRadiusSqr = FBJoystick.DeadZoneRadius * FBJoystick.DeadZoneRadius;

		[SerializeField]
		public UISprite BackgroundSprite;

		[SerializeField]
		public UISprite ThumbSprite;

		[SerializeField]
		public float Radius;

		private Vector2 _anchor;

		private Transform _transform;

		private float _radiusSqr;
	}
}
