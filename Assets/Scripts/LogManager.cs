using System;
using UnityEngine;

public class LogManager : MonoBehaviour
{
	private void Awake()
	{
		if (Debug.isDebugBuild)
		{
			Log.CurrentLevel = Log.LogLevel.Debug;
		}
		else
		{
			Log.CurrentLevel = Log.LogLevel.Warning;
		}
	}
}
