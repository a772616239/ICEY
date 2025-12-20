using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class SettingData
{
	public void Save()
	{
		PlayerPrefs.SetString("GameSettings", JsonMapper.ToJson(this));
		PlayerPrefs.Save();
	}

	public bool CheatMode;

	public bool SubtitleVisiable = true;

	public float EffectsVolume = 100f;

	public float BGMVolume = 100f;

	public bool IsEffectsMute;

	public bool IsBGMMute;

	public string Language;

	public string AudioLanguage;

	public bool IsVibrate = true;

	public int VSync = 1;

	public int FPS = 60;

	public bool DynamicJoystickOpen;

	public Dictionary<string, KeyCode> KeyMap = new Dictionary<string, KeyCode>();

	public List<int> AchievementInfo = new List<int>();
}
