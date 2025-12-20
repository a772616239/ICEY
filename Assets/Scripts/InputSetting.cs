using System;
using Core;
using UnityEngine;

public static class InputSetting
{
	public static bool IsWorking()
	{
		return Core.Input.JoystickIsOpen;
	}

	public static void Stop(bool noRecord = false)
	{
		Log.Info("stop the input system");
		if (noRecord)
		{
			Core.Input.JoystickIsOpen = false;
			SingletonMono<MobileInputPlayer>.Instance.Visible = false;
		}
		else
		{
			InputSetting._pauseCount += 1u;
			if (InputSetting._pauseCount > 0u)
			{
				Core.Input.JoystickIsOpen = false;
				if (R.Mode.CurrentMode == Mode.AllMode.UI || R.Mode.CurrentMode == Mode.AllMode.Story)
				{
					return;
				}
				SingletonMono<MobileInputPlayer>.Instance.Visible = false;
			}
		}
	}

	public static void Resume(bool noRecord = false)
	{
		Log.Info("resume the input system");
		if (noRecord)
		{
			Core.Input.JoystickIsOpen = true;
			if (R.Mode.CurrentMode == Mode.AllMode.UI || R.Mode.CurrentMode == Mode.AllMode.Story)
			{
				return;
			}
			SingletonMono<MobileInputPlayer>.Instance.Visible = true;
		}
		else
		{
			if (InputSetting._pauseCount == 0u)
			{
				Log.Warning("InputSetting is never pause when resume");
				return;
			}
			InputSetting._pauseCount -= 1u;
			if (InputSetting._pauseCount <= 0u)
			{
				Core.Input.JoystickIsOpen = true;
				if (R.Mode.CurrentMode == Mode.AllMode.UI || R.Mode.CurrentMode == Mode.AllMode.Story)
				{
					return;
				}
				SingletonMono<MobileInputPlayer>.Instance.Visible = true;
			}
		}
	}

	public static int JudgeDir(Vector3 from, Vector3 to)
	{
		return InputSetting.JudgeDir(from.x, to.x);
	}

	public static int JudgeDir(float from, float to)
	{
		return (to - from <= 0f) ? -1 : 1;
	}

	public const int LEFT = -1;

	public const int RIGHT = 1;

	public const int STOP = 0;

	public const int CURRENT = 3;

	public const int UP = 2;

	public const int DOWN = -2;

	public const int RIGHT_UP = 4;

	public const int RIGHT_DOWN = -4;

	public const int LEFT_UP = 5;

	public const int LEFT_DOWN = -5;

	public static bool Assistant = true;

	private static uint _pauseCount;
}
