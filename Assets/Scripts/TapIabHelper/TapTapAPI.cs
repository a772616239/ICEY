using System;
using UnityEngine;

namespace TapIabHelper
{
	public class TapTapAPI
	{
		private static AndroidJavaClass getAgent()
		{
			if (TapTapAPI.agent == null)
			{
				TapTapAPI.agent = new AndroidJavaClass(TapTapAPI.JAVA_CLASS);
			}
			return TapTapAPI.agent;
		}

		private static AndroidJavaClass getUnityClass()
		{
			if (TapTapAPI.unityClass == null)
			{
				TapTapAPI.unityClass = new AndroidJavaClass(TapTapAPI.UNITY_CLASS);
			}
			return TapTapAPI.unityClass;
		}

		public static void OnStart(TapTapAPI.Callback licenseCallback)
		{
			//TapTapAPI._licenseCallback = licenseCallback;
			//AndroidJavaObject @static = TapTapAPI.getUnityClass().GetStatic<AndroidJavaObject>("currentActivity");
			//TapTapAPI.getAgent().CallStatic("init", new object[]
			//{
			//	@static,
			//	string.Empty,
			//	new TapTapAPI.TapTapSDKListener()
			//});
		}

		public static void OnDestroy()
		{
			//TapTapAPI.getAgent().CallStatic("destroy", new object[0]);
		}

		public static void DownloadTapTap()
		{
			//TapTapAPI.getAgent().CallStatic("downloadTapTap", new object[0]);
		}

		public static void OpenTapTap()
		{
			//TapTapAPI.getAgent().CallStatic("openTapTap", new object[0]);
		}

		public const string PUBLIC_KEY = "";

		public static string JAVA_CLASS = "com.unity3d.player.UnityPlayer";

		public static string JAVA_INTERFACE = "com.unity3d.player.UnityPlayer";

		private static string UNITY_CLASS = "com.unity3d.player.UnityPlayer";

		private static AndroidJavaClass agent;

		private static AndroidJavaClass unityClass;

		public const int LICENSE_OK = 0;

		public const int LICENSE_NOT_INSTALL_TAPTAP = 1;

		public const int LICENSE_NO = 2;

		public const int LICENSE_NOT_SERVICE_UNAVAILABLE = 3;

		public const int BILLING_RESULT_OK = 0;

		public const int BILLING_RESULT_NOT_INSTALL_TAPTAP = 1;

		public const int BILLING_RESULT_ERR = 2;

		private static TapTapAPI.Callback _licenseCallback;

		public delegate void Callback(int code);

		public class TapTapSDKListener : AndroidJavaProxy
		{
			public TapTapSDKListener() : base(TapTapAPI.JAVA_INTERFACE)
			{
			}

			public void onLicenseCallback(int code)
			{
				if (TapTapAPI._licenseCallback != null)
				{
					TapTapAPI._licenseCallback(code);
				}
			}
		}
	}
}
