using System;
using UnityEngine;

public class GameInfoDebugGUI : DebugGuiBase
{
	protected override string UIDebugButtonName
	{
		get
		{
			return "FPS显示";
		}
	}

	protected override void GUIStart()
	{
		this.InVisible = UnityEngine.Debug.isDebugBuild;
	}

	private void Update()
	{
	}

	public override void OnDebugGUI()
	{
		if (this._debugBoxStyle == null)
		{
			this._debugBoxStyle = GUI.skin.box;
			this._debugBoxStyle.alignment = TextAnchor.UpperLeft;
		}
		GUI.depth = 0;
		GUI.Label(new Rect(10f, (float)(Screen.height - 120), 400f, 200f), "FPS              : " + WorldTime.Fps.ToString("f2"));
		GUI.Label(new Rect(10f, (float)(Screen.height - 120 + 20), 400f, 200f), "Physic FPS   : " + WorldTime.PhysicsFps.ToString("f2"));
		if (SingletonMono<WorldTime>.Instance != null)
		{
			GUI.Label(new Rect(10f, (float)(Screen.height - 120 + 40), 400f, 200f), "Level Time    : " + WorldTime.LevelTime.ToString("f2"));
		}
		GUI.Label(new Rect(10f, (float)(Screen.height - 120 + 60), 400f, 200f), "Scene Name : " + UITools.SceneName);
		GUI.Label(new Rect(10f, (float)(Screen.height - 120 + 80), 400f, 200f), "Mode            : " + R.Mode.CurrentMode);
	}

	private GUIStyle _debugBoxStyle;
}
