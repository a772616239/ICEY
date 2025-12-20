using System;
using UnityEngine;

namespace Core
{
	public class InputAxisProcessor
	{
		public bool OnPressed
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && this._isPressed && !this._wasPressed;
			}
		}

		public bool Pressed
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && this._isPressed;
			}
		}

		public bool OnReleased
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && !this._isPressed && this._wasPressed;
			}
		}

		public bool Released
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && !this._isPressed;
			}
		}

		public void Update(float axis, float axisRaw = 0f)
		{
			if (this.IsOpen)
			{
				this.Value = axis * (float)((!this.isReverse) ? 1 : -1);
				this.ValueRaw = axisRaw * (float)((!this.isReverse) ? 1 : -1);
			}
			else
			{
				this.Value = 0f;
				this.ValueRaw = 0f;
			}
			this._wasPressed = this._isPressed;
			this._isPressed = (Mathf.Abs(axisRaw) >= 0.1f);
		}

		private const float Threshold = 0.1f;

		private bool _wasPressed;

		private bool _isPressed;

		public bool IsOpen = true;

		public float Value;

		public float ValueRaw;

		public bool isReverse;
	}
}
