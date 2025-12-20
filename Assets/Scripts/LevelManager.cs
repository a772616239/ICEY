using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
	public static string SceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	private static int DeathCount
	{
		get
		{
			return RoundStorage.Get("DeathCount", 0);
		}
		set
		{
			RoundStorage.Set("DeathCount", value);
		}
	}

	public static AsyncOperation LoadScene(string levelName)
	{
		if (LevelManager._asyncOperation != null && !LevelManager._asyncOperation.isDone)
		{
			return LevelManager._asyncOperation;
		}
		LevelManager._asyncOperation = SceneManager.LoadSceneAsync(levelName);
		if (LevelManager._asyncOperation == null)
		{
			Log.Alert(levelName + "场景不存在，是不是没放在build里？还是名字写错了？");
		}
		return LevelManager._asyncOperation;
	}

	public static Coroutine LoadLevelByGateId(string levelName, SceneGate.OpenType openType = SceneGate.OpenType.None)
	{
		return LevelManager.LoadLevelByGateId(levelName, 1, openType);
	}

	public static Coroutine LoadLevelByGateId(string levelName, int gateId, SceneGate.OpenType openType = SceneGate.OpenType.None)
	{
		return R.SceneGate.Enter(new SwitchLevelGateData
		{
			ToLevelId = levelName,
			ToId = gateId,
			OpenType = openType
		}, false);
	}

	public static Coroutine LoadLevelByPosition(string levelName, Vector3 position, bool needProgressBar = false)
	{
		return R.SceneGate.Enter(new SwitchLevelGateData
		{
			ToLevelId = levelName,
			ToId = -1,
			TargetPosition = position
		}, needProgressBar);
	}

	public static Coroutine OnRoundOver()
	{
		return R.Coroutine.Start(LevelManager.RoundOverCoroutine());
	}

	public static Coroutine OnPlayerDie(bool recordDeath = true)
	{
		return R.Coroutine.Start(LevelManager.LoadGame(recordDeath));
	}

	private static IEnumerator RoundOverCoroutine()
	{
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private static IEnumerator LoadGame(bool recordDeath)
	{
		R.DeadReset();
		yield return R.GameData.Load();
		yield return LevelManager.LoadLevelByPosition(R.GameData.SceneName, R.GameData.PlayerPosition, false);
		if (recordDeath)
		{
			LevelManager.DeathCount++;
			R.GameData.Save(true);
		}
		R.Player.Transform.position = R.GameData.PlayerPosition;
		R.Player.Action.TurnRound(R.GameData.PlayerAttributeGameData.faceDir);
		R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		PlayerAction.Reborn();
		yield break;
	}

	private static AsyncOperation _asyncOperation;

	public struct LevelName
	{
		public const string Start = "ui_start";
	}
}
