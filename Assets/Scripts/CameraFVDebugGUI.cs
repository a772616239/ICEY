using System;
using UnityEngine;

public class CameraFVDebugGUI : DebugGuiBase
{
	protected override string UIDebugButtonName
	{
		get
		{
			return "FV调节";
		}
	}

	public override void OnDebugGUI()
	{
		if (!this.InVisible)
		{
			return;
		}
		int width = Screen.width;
		int num = (width - 620) / 2;
		int num2 = 110;
		GUI.BeginGroup(new Rect((float)num, (float)num2, 620f, 220f));
		GUI.Box(new Rect(0f, 0f, 620f, 220f), "FV Settings");
		GUI.Label(new Rect(20f, 60f, 300f, 60f), string.Format("FV：({0})", SingletonMono<CameraController>.Instance.FieldOfView));
		SingletonMono<CameraController>.Instance.FieldOfView = GUI.HorizontalSlider(new Rect(240f, 70f, 400f, 60f), SingletonMono<CameraController>.Instance.FieldOfView, 10f, 60f);
		if (GUI.Button(new Rect(280f, 120f, 100f, 60f), "Close"))
		{
			this.InVisible = false;
		}
		GUI.EndGroup();
	}

	protected override void GUIStart()
	{
	}
}
