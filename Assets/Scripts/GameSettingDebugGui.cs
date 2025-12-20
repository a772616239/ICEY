using System;
using UnityEngine;

public class GameSettingDebugGui : DebugGuiBase
{
	protected override string UIDebugButtonName
	{
		get
		{
			return "游戏设置";
		}
	}

	protected override void GUIStart()
	{
		this._guiArea = new Rect((float)(Screen.width / 2 - 320), 20f, 640f, 500f);
	}

	public override void OnDebugGUI()
	{
		GUILayout.BeginArea(this._guiArea);
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		GUILayout.Label(string.Format("Current AI Status: {0}", (!R.SceneData.CanAIRun) ? "OFF" : "On"), new GUILayoutOption[0]);
		if (GUILayout.Button("Switch", new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.Width(80f)
		}))
		{
			R.SceneData.CanAIRun = !R.SceneData.CanAIRun;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		GUILayout.Label(string.Format("Blood Palace Mode: {0}", (!R.SceneData.BloodPalaceMode) ? "OFF" : "On"), new GUILayoutOption[0]);
		if (GUILayout.Button("Switch", new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.Width(80f)
		}))
		{
			this.EnterBloodPalace();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		string[] array = new string[]
		{
			"Easy",
			"Normal",
			"Hard",
			"Hell"
		};
		GUILayout.Label(string.Format("Difficulty: {0}", array[R.GameData.Difficulty]), new GUILayoutOption[0]);
		if (GUILayout.Button("Switch", new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.Width(80f)
		}))
		{
			R.GameData.Difficulty = (R.GameData.Difficulty + 1) % 4;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		GUILayout.Label(string.Format("Subtitle: {0}", (!R.Settings.SubtitleVisiable) ? "OFF" : "On"), new GUILayoutOption[0]);
		if (GUILayout.Button("Switch", new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.Width(80f)
		}))
		{
			R.Settings.SubtitleVisiable = !R.Settings.SubtitleVisiable;
			R.Settings.Save();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		if (GUILayout.Button("Open Enhancement", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.Ui.Enhancement.OpenWithAnim();
		}
		if (GUILayout.Button("Open Level Select", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.Ui.LevelSelect.OpenWithAnim(true, true);
		}
		if (GUILayout.Button("Reset Enhancement", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.GameData.Enhancement.SetToDefault();
		}
		if (GUILayout.Button("Full Level", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.GameData.Enhancement.SetAllTo(3);
		}
		if (GUILayout.Button(R.Settings.CheatMode ? "Cheat Off" : "Cheat On", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.Settings.CheatMode = !R.Settings.CheatMode;
			R.Settings.Save();
		}
		if (GUILayout.Button("$+100", new GUILayoutOption[]
		{
			GUILayout.Height(20f)
		}))
		{
			R.Equipment.CoinNum += 100;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		GUILayout.Label("Effects Volume: ", new GUILayoutOption[0]);
		R.Audio.EffectsVolume = GUILayout.HorizontalSlider(R.Audio.EffectsVolume, 0f, 100f, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width(600f)
		});
		GUILayout.Label("BGM Volume: ", new GUILayoutOption[0]);
		R.Audio.BGMVolume = GUILayout.HorizontalSlider(R.Audio.BGMVolume, 0f, 100f, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void EnterBloodPalace()
	{
		if (R.SceneData.BloodPalaceMode)
		{
			SingletonMono<BattleAssessmentManager>.Instance.ExitBloodPalace();
		}
		else
		{
			SingletonMono<BattleAssessmentManager>.Instance.EnterBloodPalace();
		}
	}

	private Rect _guiArea;
}
