using System;
using System.Collections;
using UnityEngine;

public class SaveDebugGUI : DebugGuiBase
{
	protected override void GUIStart()
	{
	}

	protected override string UIDebugButtonName
	{
		get
		{
			return "存档管理";
		}
	}

	public override void OnDebugGUI()
	{
		if (GUILayout.Button("AutoSave", new GUILayoutOption[]
		{
			GUILayout.Height(100f)
		}))
		{
			R.GameData.Save(false);
		}
		if (GUILayout.Button("AutoLoad", new GUILayoutOption[]
		{
			GUILayout.Height(100f)
		}))
		{
			base.StartCoroutine(this.LoadGame());
		}
		if (GUILayout.Button("DeleteAutoSave", new GUILayoutOption[]
		{
			GUILayout.Height(100f)
		}))
		{
			SaveManager.AutoDelete();
		}
	}

	private IEnumerator LoadGame()
	{
		R.GameData.Load();
		if (R.GameData.SceneName != LevelManager.SceneName)
		{
			yield return LevelManager.LoadLevelByGateId(R.GameData.SceneName, SceneGate.OpenType.None);
		}
		R.Player.Transform.position = R.GameData.PlayerPosition;
		R.Player.Action.TurnRound(R.GameData.PlayerAttributeGameData.faceDir);
		R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		yield break;
	}
}
