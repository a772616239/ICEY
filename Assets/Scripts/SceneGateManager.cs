using System;
using System.Collections;
using System.Collections.Generic;
using GameWorld;
using UnityEngine;

public class SceneGateManager : SingletonMono<SceneGateManager>
{
	public float Progress
	{
		get
		{
			return (this.Async == null) ? 0f : this.Async.progress;
		}
	}

	private IEnumerator LoadLevelCoroutine(SwitchLevelGateData data, bool needProgressBar)
	{
		this.IsLocked = true;
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		float player2Ground = Physics2D.Raycast(R.Player.Transform.position, Vector2.down, 100f, LayerManager.GroundMask).distance;
		this.Async = LevelManager.LoadScene(data.ToLevelId);
		this.Async.allowSceneActivation = false;
		while (!this.AllowSceneActivation)
		{
			yield return null;
		}
		if (needProgressBar)
		{
			yield return R.Ui.BlackScene.FadeBlack(0.3f, false);
		}
		this.Async.allowSceneActivation = true;
		yield return this.Async;
		base.StartCoroutine(this.Preload());
		this.IsLocked = false;
		if (data.ToId != -1)
		{
			this.Exit(this.FindGateData(data.ToId), player2Ground, data.OpenType);
		}
		else
		{
			SwitchLevelGateData data2 = new SwitchLevelGateData
			{
				SelfPosition = data.TargetPosition
			};
			this.Exit(data2, 0f, SceneGate.OpenType.None);
		}
		yield break;
	}

	public Coroutine Enter(SwitchLevelGateData data, bool needProgressBar = false)
	{
		EventManager.PostEvent<GameObject, PassGateEventArgs>("PassGate", base.gameObject, new PassGateEventArgs(PassGateEventArgs.PassGateStatus.Enter, data, LevelManager.SceneName));
		Log.Info(string.Format("从 Gate {1} 离开 {0}", LevelManager.SceneName, data.MyId));
		return base.StartCoroutine(this.EnterCoroutine(data, needProgressBar));
	}

	public Coroutine Exit(SwitchLevelGateData data, float groundDis, SceneGate.OpenType enterGateOpenType)
	{
		EventManager.PostEvent<GameObject, PassGateEventArgs>("PassGate", base.gameObject, new PassGateEventArgs(PassGateEventArgs.PassGateStatus.Exit, data, LevelManager.SceneName));
		Log.Info(string.Format("从 Gate {1} 进入 {0}", LevelManager.SceneName, data.MyId));
		return base.StartCoroutine(this.ExitCoroutine(data, groundDis, enterGateOpenType));
	}

	private IEnumerator EnterCoroutine(SwitchLevelGateData data, bool needProgressBar)
	{
		R.SceneData.CanAIRun = false;
		InputSetting.Stop(false);
		this.IsLocked = true;
		if (!needProgressBar)
		{
			yield return R.Ui.BlackScene.FadeBlack(0.3f, false);
		}
		this.IsLocked = false;
		yield return base.StartCoroutine(this.LoadLevelCoroutine(data, needProgressBar));
		yield break;
	}

	private IEnumerator ExitCoroutine(SwitchLevelGateData data, float player2Ground, SceneGate.OpenType enterGateOpenType)
	{
		this.IsLocked = true;
		Vector3 pos = R.Player.Transform.position;
		pos.x = data.SelfPosition.x;
		if (data.InAir)
		{
			pos.y = data.SelfPosition.y;
		}
		else
		{
			float distance = Physics2D.Raycast(data.SelfPosition, Vector2.down, 100f, LayerManager.GroundMask).distance;
			float num = data.SelfPosition.y - distance;
			pos.y = player2Ground + num;
			if (data.OpenType != SceneGate.OpenType.PressKey)
			{
				if (enterGateOpenType != SceneGate.OpenType.Left)
				{
					if (enterGateOpenType == SceneGate.OpenType.Right)
					{
						pos.x -= data.TriggerSize.x + 1f;
					}
				}
				else
				{
					pos.x += data.TriggerSize.x + 1f;
				}
			}
		}
		R.Player.Transform.position = pos;
		if (R.GameData.WindyVisiable && R.Windy != null)
		{
			if (data.OpenType == SceneGate.OpenType.Right)
			{
				R.Windy.transform.position = pos + new Vector3(-2f, 1f, 0f);
			}
			else if (data.OpenType == SceneGate.OpenType.Left)
			{
				R.Windy.transform.position = pos + new Vector3(2f, 1f, 0f);
			}
			else
			{
				R.Windy.transform.position = pos;
			}
		}
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		yield return new WaitForFixedUpdate();
		if (enterGateOpenType != SceneGate.OpenType.Left)
		{
			if (enterGateOpenType == SceneGate.OpenType.Right)
			{
				R.Player.ActionController.TurnRound(-1);
			}
		}
		else
		{
			R.Player.ActionController.TurnRound(1);
		}
		yield return R.Ui.BlackScene.FadeTransparent(0.3f, false);
		this.IsLocked = false;
		yield break;
	}

	public SwitchLevelGateData FindGateData(int id)
	{
		return this.FindGate(id).data;
	}

	public SceneGate FindGate(int id)
	{
		for (int i = 0; i < this.GatesInCurrentScene.Count; i++)
		{
			if (this.GatesInCurrentScene[i].data.MyId == id)
			{
				return this.GatesInCurrentScene[i];
			}
		}
		throw new NullReferenceException(string.Concat(new object[]
		{
			"No Gate in Scene ",
			LevelManager.SceneName,
			" id ",
			id
		}));
	}

	private IEnumerator Preload()
	{
		yield break;
	}

	public List<SceneGate> GatesInCurrentScene = new List<SceneGate>();

	public bool AllowSceneActivation = true;

	private AsyncOperation Async;

	public bool IsLocked;
}
