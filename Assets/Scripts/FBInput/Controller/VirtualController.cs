using System;
using Core;
using UnityEngine;

namespace FBInput.Controller
{
	public class VirtualController : IController
	{
		private MobileInputPlayer _player
		{
			get
			{
				return SingletonMono<MobileInputPlayer>.Instance;
			}
		}

		public void Update()
		{
			VirtualController.Button1.Update(this._player.GetButton("Button1"));
			VirtualController.Button2.Update(this._player.GetButton("Button2") || VirtualController.KeyFlash());
			VirtualController.Button3.Update(this._player.GetButton("Button3") || VirtualController.KeyExecute());
			VirtualController.Button4.Update(this._player.GetButton("Button4") || VirtualController.KeyJump());
			VirtualController.Button5.Update(this._player.GetButton("Button5") || VirtualController.KeyAtk());
			VirtualController.Button6.Update(this._player.GetButton("Options"));
			VirtualController.Options.Update(this._player.GetButton("Options") || UnityEngine.Input.GetKey(KeyCode.Escape));
			VirtualController.AnyKey.Update(VirtualController.Button1.Pressed || VirtualController.Button2.Pressed || VirtualController.Button3.Pressed || VirtualController.Button4.Pressed || VirtualController.Button5.Pressed || UnityEngine.Input.touchCount > 0);
			VirtualController.L2.Update(this._player.GetButton("L2"));
			VirtualController.R2.Update(this._player.GetButton("R2"));
			Vector2 keyboardAxis = VirtualController.KeyboardAxis();
			VirtualController.LeftJoystick.Update((keyboardAxis != Vector2.zero) ? keyboardAxis : this._player.GetJoystick("Joystick"), default(Vector2));
			VirtualController.LeftSwipe.Update(this._player.GetJoystick("Swipe"), default(Vector2));
		}

		private static bool KeyJump()
		{
			return UnityEngine.Input.GetKey(KeyCode.Space);
		}

		private static bool KeyAtk()
		{
			return UnityEngine.Input.GetKey(KeyCode.J);
		}

		private static bool KeyFlash()
		{
			return UnityEngine.Input.GetKey(KeyCode.K) || UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
		}

		private static bool KeyExecute()
		{
			return UnityEngine.Input.GetKey(KeyCode.L);
		}

		private static Vector2 KeyboardAxis()
		{
			float x = 0f;
			float y = 0f;
			if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow))
			{
				x -= 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow))
			{
				x += 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow))
			{
				y += 1f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow))
			{
				y -= 1f;
			}
			if (x == 0f && y == 0f)
			{
				return Vector2.zero;
			}
			return new Vector2(x, y).normalized;
		}

		public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button1 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button2 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button3 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button4 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button5 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Button6 = new InputButtonProcessor();

		public static readonly InputButtonProcessor Options = new InputButtonProcessor();

		public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

		public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

		public static readonly InputJoystickProcessor LeftJoystick = new InputJoystickProcessor();

		public static readonly InputJoystickProcessor LeftSwipe = new InputJoystickProcessor();
	}
}
