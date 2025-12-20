using System;
using TapIabHelper;
using UnityEngine;

[DisallowMultipleComponent]
public class TapTapManager : MonoBehaviour
{
	private void Start()
	{
		TapTapAPI.OnStart(new TapTapAPI.Callback(this.LicenseCallback));
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnApplicationQuit()
	{
		TapTapAPI.OnDestroy();
	}

	private void LicenseCallback(int code)
	{
		code = 0;
		TapTapManager.LicenseState = code;
		switch (code)
		{
		case 0:
			this._licenseState = "授权状态：\n LICENSE_OK";
			return;
		case 1:
			this._licenseState = "授权状态：\n LICENSE_NOT_INSTALL_TAPTAP";
			return;
		case 2:
			this._licenseState = "授权状态：\n LICENSE_NO";
			return;
		case 3:
			this._licenseState = "授权状态：\n LICENSE_NOT_SERVICE_UNAVAILABLE";
			return;
		default:
			this._licenseState = "授权状态：\n 未知状态";
			return;
		}
	}

	public static void OpenTapTap()
	{
		TapTapAPI.OpenTapTap();
	}

	public static void DownloadTapTap()
	{
		TapTapAPI.DownloadTapTap();
	}

	public static void QuitGame()
	{
		TapTapAPI.OnDestroy();
		Application.Quit();
	}

	public static int LicenseState = 3;

	private string _licenseState;
}
