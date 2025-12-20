using System;
using UnityEngine;

public class AchievementDebugGUI : DebugGuiBase
{
	protected override string UIDebugButtonName
	{
		get
		{
			return "成就系统";
		}
	}

	protected override void GUIStart()
	{
		this._guiArea = new Rect((float)(Screen.width / 2 - 320), 20f, 640f, 500f);
	}

	public override void OnDebugGUI()
	{
		GUILayout.BeginArea(this._guiArea);
		this._scrollPosition = GUILayout.BeginScrollView(this._scrollPosition, new GUILayoutOption[0]);
		for (int i = 0; i < 34; i++)
		{
			AchievementManager.AchievementInfo achievementInfo = SingletonMono<AchievementManager>.Instance.GetAchievementInfo(i);
			bool achievementUnlockState = SingletonMono<AchievementManager>.Instance.GetAchievementUnlockState(i);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(achievementInfo.Name, new GUILayoutOption[0]);
			if (!achievementUnlockState && i != 0 && GUILayout.Button("Unlock", new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.Width(80f)
			}))
			{
				R.Trophy.AwardTrophy(i);
			}
			GUILayout.EndHorizontal();
			GUILayout.Label(achievementInfo.Detail, new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private Rect _guiArea;

	private Vector2 _scrollPosition;
}
