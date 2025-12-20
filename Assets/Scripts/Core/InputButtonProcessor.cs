using System;
using UnityEngine;

namespace Core
{
	public class InputButtonProcessor
	{
		public bool OnClick
		{
			get
			{
				return this.OnReleased && this._pressedTime < 0.3f;
			}
		}

		public bool OnPressed
		{
			get
			{
				return this.Pressed && !this._wasPressed;
			}
		}

		public bool OnPressedForSeveralSeconds(float second = 0.333333343f, bool accelerate = true)
		{
			int num = WorldTime.SecondToFrame(second);
			int num2 = WorldTime.SecondToFrame(0.0833333358f);
			int b = num2;
			if (accelerate)
			{
				int a = num - this._pressedFrame / num2;
				num = Mathf.Max(a, b);
			}
			return this.OnPressed || (this.Pressed && this._pressedFrame != 0 && this._pressedFrame % num == 0);
		}

		public bool OnReleased
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && !this._isPressed && this._wasPressed;
			}
		}

		public bool Pressed
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && this._isPressed;
			}
		}

		public bool LongPressed
		{
			get
			{
				return this.Pressed && this._pressedTime >= 0.3f;
			}
		}

		public bool Released
		{
			get
			{
				return Input.JoystickIsOpen && this.IsOpen && !this._isPressed;
			}
		}

		public void Update(bool isPressed)
		{
			if (this._wasPressed && isPressed)
			{
				this._pressedTime += Time.unscaledDeltaTime;
				this._pressedFrame++;
			}
			else if (!this._wasPressed && !isPressed)
			{
				this._pressedTime = 0f;
				this._pressedFrame = 0;
			}
			this._wasPressed = this._isPressed;
			this._isPressed = isPressed;
		}

		public bool IsOpen = true;

		private bool _wasPressed;

		private bool _isPressed;

		private float _pressedTime;

		private int _pressedFrame;

		private const float LongPressTime = 0.3f;
	}
}
