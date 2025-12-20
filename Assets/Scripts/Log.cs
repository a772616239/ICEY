using System;
using UnityEngine;

public static class Log
{
	private static string Now
	{
		get
		{
			DateTime now = DateTime.Now;
			return string.Format("[{0} {1}]", now.ToString("u"), now.Millisecond);
		}
	}

	public static void Debug(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Debug)
		{
			UnityEngine.Debug.LogFormat("{0} [Debug] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static void Info(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Info)
		{
			UnityEngine.Debug.LogFormat("{0} [Info] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static void Notice(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Notice)
		{
			UnityEngine.Debug.LogFormat("{0} [Notice] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static void Warning(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Warning)
		{
			UnityEngine.Debug.LogWarningFormat("{0} [Warning] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static void Error(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Error)
		{
			UnityEngine.Debug.LogErrorFormat("{0} [Error] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static void Alert(object obj)
	{
		if (Log.CurrentLevel <= Log.LogLevel.Alert)
		{
			UnityEngine.Debug.LogErrorFormat("{0} [Alert] {1}", new object[]
			{
				Log.Now,
				obj
			});
		}
	}

	public static Log.LogLevel CurrentLevel;

	public enum LogLevel
	{
		Emerg = 7,
		Alert = 6,
		Crit = 5,
		Error = 4,
		Warning = 3,
		Notice = 2,
		Info = 1,
		Debug = 0
	}
}
