using System;
using System.Collections;
using UnityEngine;

namespace SRDebugger.Internal
{
	public class BugReportScreenshotUtil
	{
		public static IEnumerator ScreenshotCaptureCo()
		{
			if (BugReportScreenshotUtil.ScreenshotData != null)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger] Warning, overriding existing screenshot data.");
			}
			yield return new WaitForEndOfFrame();
			Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			tex.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
			tex.Apply();
			BugReportScreenshotUtil.ScreenshotData = tex.EncodeToPNG();
			UnityEngine.Object.Destroy(tex);
			yield break;
		}

		public static byte[] ScreenshotData;
	}
}
