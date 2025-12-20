using System;
using GameWorld;
using LitJson;
using UnityEngine;

public static class R
{
	public static EffectController Effect
	{
		get
		{
			return SingletonMono<EffectController>.Instance;
		}
	}

	public static GameObject Windy
	{
		get
		{
			GameObject result;
			if ((result = R._windy) == null)
			{
				result = (R._windy = GameObject.FindGameObjectWithTag("Windy"));
			}
			return result;
		}
	}

	public static Equipment Equipment
	{
		get
		{
			return R.GameData.Equipment;
		}
	}

	public static GameData GameData
	{
		get
		{
			GameData result;
			if ((result = R._gameData) == null)
			{
				result = (R._gameData = new GameData());
			}
			return result;
		}
		set
		{
			R._gameData = value;
		}
	}

	public static SettingData Settings
	{
		get
		{
			if (R._settings != null)
			{
				return R._settings;
			}
			if (PlayerPrefs.HasKey("GameSettings"))
			{
				R._settings = JsonMapper.ToObject<SettingData>(PlayerPrefs.GetString("GameSettings"));
				Log.Info("Settings are loaded");
			}
			else
			{
				R._settings = new SettingData();
				Log.Info("Settings are created");
			}
			return R._settings;
		}
	}

	public static UIController Ui
	{
		get
		{
			return SingletonMono<UIController>.Instance;
		}
	}

	public static AudioManager Audio
	{
		get
		{
			return SingletonMono<AudioManager>.Instance;
		}
	}

	public static SceneGateManager SceneGate
	{
		get
		{
			return SingletonMono<SceneGateManager>.Instance;
		}
	}

	public static void DeadReset()
	{
		R.Mode.Reset();
		Cliff.Reset();
		WorldTime.Reset();
		R.Ui.Reset();
		Log.Info("死亡重置数据");
	}

	public static void RoundReset()
	{
		R.Mode.Reset();
		Cliff.Reset();
		WorldTime.Reset();
		RoundStorage.Clear();
		PlayerAction.Reborn();
		R.GameData.BattleZoneDict.Clear();
		R.Ui.Reset();
		Log.Info("周目结束重置数据");
	}

	public static void SaveReset()
	{
		R.Equipment.Clear();
		R.Mode.Reset();
		R.SceneData.Clear();
		R.GameData.Reset();
		BattleAssessmentManager.Reset();
		Cliff.Reset();
		PlayerAction.Reset();
		WorldTime.Reset();
		R.Ui.Reset();
		UIStartController.IsEnterWithVoice = false;
		SaveManager.AutoDelete();
		Log.Info("删档重置数据");
	}

	public static void PauseGame()
	{
		WorldTime.Pause();
		EventManager.PostEvent<UIPauseController, PauseGameArgs>("PauseGame", R.Ui.Pause, PauseGameArgs.Pause);
		R.Audio.PauseVoiceOver();
	}

	public static void ResumeGame()
	{
		WorldTime.Resume();
		EventManager.PostEvent<UIPauseController, PauseGameArgs>("PauseGame", R.Ui.Pause, PauseGameArgs.Resume);
		R.Audio.ResumeVoiceOver();
	}

	public static readonly EnemyManager Enemy = new EnemyManager();

	public static readonly PlayerManager Player = new PlayerManager();

	public static readonly CameraManager Camera = new CameraManager();

	public static readonly SceneData SceneData = new SceneData();

	public static readonly Mode Mode = new Mode();

	public static CoroutineManager Coroutine = new CoroutineManager();

	private static GameObject _windy;

	private static GameData _gameData;

	private static SettingData _settings;

	public static TrophyManager Trophy = new TrophyManager();
}
