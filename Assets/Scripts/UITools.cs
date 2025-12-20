using System;
using UnityEngine;

public static class UITools
{
	public static int ScreenHeight
	{
		get
		{
			return SingletonMono<UIController>.Instance.RootWidget.height;
		}
	}

	public static int ScreenWidth
	{
		get
		{
			return SingletonMono<UIController>.Instance.RootWidget.width;
		}
	}

	public static string SceneName
	{
		get
		{
			return LevelManager.SceneName;
		}
	}

	public static Vector2 ScreenPositionToLocalPosition(Vector2 screenPosition)
	{
		screenPosition.Scale(UITools.Multiplier);
		return screenPosition;
	}

	private static Vector2 Multiplier
	{
		get
		{
			return new Vector2((float)UITools.ScreenWidth / UITools.ScreenSize.x, (float)UITools.ScreenHeight / UITools.ScreenSize.y);
		}
	}

	private static Vector2 ScreenSize
	{
		get
		{
			return NGUITools.screenSize;
		}
	}

	private static string _sceneName;
}
