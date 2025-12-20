using System;
using UnityEngine;
using UnityEngine.Analytics;

public class Analytics : MonoBehaviour
{
	private void Start()
	{
		TapDB.onStart(global::Analytics.AppId, global::Analytics.Channel, null, true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
	}

	private void OnApplicationQuit()
	{
		TapDB.onStop();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			TapDB.onStop();
		}
		else
		{
			TapDB.onResume();
		}
	}

	public static void SetUser(string userId, TGTUserType userType, TGTUserSex userSex, int userAge, string userName)
	{
		TapDB.setUser(userId, userType, userSex, userAge, userName);
		UnityEngine.Analytics.Analytics.SetUserId(userId);
		UnityEngine.Analytics.Analytics.SetUserGender((Gender)userSex);
	}

	private static readonly string AppId = "lwqo2sdxkqa07p3y";

	private static readonly string Channel = "TapTap";
}
