using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SaveManager : BaseBehaviour
{
	public static GameData GameData
	{
		get
		{
			return SaveManager._gameDataLoaded;
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event EventHandler<SaveLoadedEventArgs> OnGameLoaded;

	public static bool IsBusy
	{
		get
		{
			return SaveData.IsBusy;
		}
	}

	public static bool IsAutoSaveDataExists
	{
		get
		{
			return SaveData.IsAutoSaveDataExists();
		}
	}

	public static Coroutine AutoSave(GameData gameData)
	{
		return R.Coroutine.Start(SaveManager.AutoSaveWhenIsNotBusy(gameData));
	}

	private static IEnumerator AutoSaveWhenIsNotBusy(GameData gameData)
	{
		while (SaveManager.IsBusy)
		{
			yield return null;
		}
		SaveData.Save(gameData);
		yield break;
	}

	public static Coroutine AutoLoad()
	{
		SaveManager._gameDataLoaded = null;
		SaveData.Load();
		return R.Coroutine.Start(SaveManager.AutoLoadAndWaitForLoadedCoroutine());
	}

	private static IEnumerator AutoLoadAndWaitForLoadedCoroutine()
	{
		while (SaveManager._gameDataLoaded == null)
		{
			yield return null;
		}
		yield break;
	}

	public static void AutoDelete()
	{
		SaveData.Delete();
	}

	public static Coroutine ModifySaveData(Action<GameData> action)
	{
		return R.Coroutine.Start(SaveManager.ModifySaveDataCoroutine(action));
	}

	private static IEnumerator ModifySaveDataCoroutine(Action<GameData> action)
	{
		yield return SaveManager.AutoLoad();
		GameData gameData = SaveManager.GameData;
		action(gameData);
		SaveManager.AutoSave(gameData);
		yield break;
	}

	private void OnSavedGameLoaded(object sender, SaveLoadedEventArgs e)
	{
		GameData gameData = e.GameData;
		SaveManager._gameDataLoaded = gameData;
		if (SaveManager.OnGameLoaded != null)
		{
			SaveManager.OnGameLoaded(sender, e);
		}
	}

	private void OnEnable()
	{
		SaveData.OnGameLoaded += this.OnSavedGameLoaded;
	}

	private void OnDisable()
	{
		SaveData.OnGameLoaded -= this.OnSavedGameLoaded;
	}

	private static GameData _gameDataLoaded;
}
