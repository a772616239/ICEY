using System;
using UnityEngine;

public class TapDB
{
	private static AndroidJavaClass getAgent()
	{
		if (TapDB.agent == null)
		{
			TapDB.agent = new AndroidJavaClass(TapDB.JAVA_CLASS);
		}
		return TapDB.agent;
	}

	private static AndroidJavaClass getUnityClass()
	{
		if (TapDB.unityClass == null)
		{
			TapDB.unityClass = new AndroidJavaClass(TapDB.UNTIFY_CLASS);
		}
		return TapDB.unityClass;
	}

	private static void TapDB_nativeInit(string appId, string channel, string gameVersion, bool requestPermission)
	{
		
	}

	private static void TapDB_nativeOnResume()
	{
		
	}

	private static void TapDB_nativeOnStop()
	{
		
	}

	public static void onStart(string appId, string channel, string gameVersion, bool requestPermission)
	{
		TapDB.TapDB_nativeInit(appId, channel, gameVersion, requestPermission);
		TapDB.TapDB_nativeOnResume();
	}

	public static void onResume()
	{
		TapDB.TapDB_nativeOnResume();
	}

	public static void onStop()
	{
		TapDB.TapDB_nativeOnStop();
	}

	public static void setUser(string userId, TGTUserType userType, TGTUserSex userSex, int userAge, string userName)
	{
		
	}

	public static void setLevel(int level)
	{
		
	}

	public static void setServer(string server)
	{
		
	}

	public static void onChargeRequest(string orderId, string product, int amount, string currencyType, int virtualCurrencyAmount, string payment)
	{
		
	}

	public static void onChargeSuccess(string orderId)
	{
		
	}

	public static void onChargeFail(string orderId, string reason)
	{
		
	}

	public static void onChargeOnlySuccess(string orderId, string product, int amount, string currencyType, int virtualCurrencyAmount, string payment)
	{
		
	}

	public static string JAVA_CLASS = "com.unity3d.player.UnityPlayer";

	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";

	private static AndroidJavaClass agent;

	private static AndroidJavaClass unityClass;
}
