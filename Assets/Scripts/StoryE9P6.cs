using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using UnityEngine;

public class StoryE9P6 : BaseBehaviour
{
	private bool IsE9BossDie
	{
		get
		{
			return RoundStorage.Get("E9_BossDie", false);
		}
		set
		{
			RoundStorage.Set("E9_BossDie", value);
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private bool OnBattle(string eventDefine, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			this.IsE9BossDie = true;
			base.StartCoroutine(this.Sequence0());
		}
		return true;
	}

	private IEnumerator Sequence0()
	{
		SceneGate.OpenType originOpenType = this._gate.openType;
		this._gate.openType = SceneGate.OpenType.None;
		this._battleZoneGate.Appear();
		R.Mode.EnterMode(Mode.AllMode.Story);
		R.Audio.PlayVoiceOver("e9t11", null, false);
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.CameraMoveTo(this._animator.transform.position, 2f, Ease.Linear);
		yield return new WaitForSeconds(1.5f);
		this._animator.Play("door");
		yield return new WaitForSeconds(4f);
		CameraFilterUtils.Create<CameraFilterPack_TV_Vcr>(null);
		this._arrow.SetActive(true);
		yield return new WaitForSeconds(2f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Vcr>(null);
		R.Camera.Controller.CameraMoveTo(R.Player.Transform.position, 2f, Ease.Linear);
		yield return new WaitForSeconds(2.1f);
		R.Camera.Controller.IsFollowPivot = true;
		this._gate.openType = originOpenType;
		this._battleZoneGate.DisAppear();
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield break;
	}

	[SerializeField]
	private SceneGate _gate;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private GameObject _arrow;

	[SerializeField]
	private BattleZoneGate _battleZoneGate;
}
