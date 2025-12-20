using System;
using System.Collections;
using GameWorld;
using UnityEngine;

public class StoryE15P1 : BaseBehaviour
{
	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
		return true;
	}

	private IEnumerator P1Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e15t1", null, false);
		R.Audio.StopBGM(false);
		this._errorBoom.SetActive(true);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.BlackScene.FadeBlack(0f, false);
		R.Ui.HideUI(true);
		this.gate.Enter(false);
		yield break;
	}

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private GameObject _errorBoom;
}
