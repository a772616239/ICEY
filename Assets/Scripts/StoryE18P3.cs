using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE18P3 : BaseBehaviour
{
	private bool killTrinity
	{
		get
		{
			return RoundStorage.Get("E18_KillTrinity", false);
		}
	}

	private bool e18Dahal
	{
		get
		{
			return RoundStorage.Get("E18_Dahal", false);
		}
		set
		{
			RoundStorage.Set("E18_Dahal", value);
		}
	}

	private void Start()
	{
		if (!this.e18Dahal)
		{
			this.E18DahalStory();
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private void OnDestroy()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_Artefact>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical2>(null);
	}

	private bool EnemyDie(string eventDefine, object sender, EventArgs msg)
	{
		GameArea.CameraRange.xMin = GameArea.MapRange.min.x;
		GameArea.CameraRange.xMax = GameArea.MapRange.max.x;
		GameArea.PlayerRange.xMin = GameArea.MapRange.min.x;
		GameArea.PlayerRange.xMax = GameArea.MapRange.max.x;
		this._battleZoneGateL.DisAppear();
		this._battleZoneGateR.DisAppear();
		R.SceneData.CanAIRun = false;
		R.Mode.EnterMode(Mode.AllMode.Normal);
		return true;
	}

	private void E18DahalStory()
	{
		this.e18Dahal = true;
		if (this.killTrinity)
		{
			base.StartCoroutine(this.P3CoroutineKill());
		}
		else
		{
			base.StartCoroutine(this.P3CoroutineNotKill());
		}
	}

	private IEnumerator P3CoroutineKill()
	{
		R.Audio.PlayBGM(287, true);
		GameObject dahal = Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.达哈尔终极版, new Vector2?(this.spwanPoint.position), false, true);
		yield return base.StartCoroutine(this.PlayerMove());
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.CameraMoveTo(new Vector2(this.spwanPoint.position.x, R.Camera.Controller.MovableCamera.position.y), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(2f);
		R.Camera.Controller.CameraMoveTo(new Vector2(R.Player.Transform.position.x, R.Camera.Controller.MovableCamera.position.y), 1f, Ease.Linear);
		yield return new WaitForSeconds(3f);
		R.Camera.Controller.IsFollowPivot = true;
		InputSetting.Resume(false);
		this._battleZoneGateL.Appear();
		this._battleZoneGateR.Appear();
		GameArea.EnemyRange.xMin = this._battleZoneGateL.transform.Find("PlayerLimitPosLX").transform.position.x + 1f;
		GameArea.EnemyRange.xMax = this._battleZoneGateR.transform.Find("PlayerLimitPosRX").transform.position.x - 1f;
		GameArea.PlayerRange.xMin = this._battleZoneGateL.transform.Find("PlayerLimitPosLX").transform.position.x;
		GameArea.PlayerRange.xMax = this._battleZoneGateR.transform.Find("PlayerLimitPosRX").transform.position.x;
		GameArea.CameraRange.xMin = this._battleZoneGateL.transform.Find("CameraLimitPosLX").transform.position.x;
		GameArea.CameraRange.xMax = this._battleZoneGateR.transform.Find("CameraLimitPosRX").transform.position.x;
		R.SceneData.CanAIRun = true;
		yield return R.Audio.PlayVoiceOver("e18t21", null, false);
		yield break;
	}

	private IEnumerator P3CoroutineNotKill()
	{
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		this.dahalThanks.SetActive(true);
		yield return base.StartCoroutine(this.PlayerMove());
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.CameraMoveTo(new Vector2(this.spwanPoint.position.x, R.Camera.Controller.MovableCamera.position.y), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(1.5f);
		this.dahalThanks.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "IdleToThanks", false);
		this.dahalThanks.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "Thanks", true, 0f);
		yield return new WaitForSeconds(1f);
		R.Camera.Controller.CameraMoveTo(new Vector2(R.Player.Transform.position.x, R.Camera.Controller.MovableCamera.position.y), 1f, Ease.Linear);
		yield return new WaitForSeconds(2f);
		R.Camera.Controller.IsFollowPivot = true;
		InputSetting.Resume(false);
		CameraFilterUtils.Create<CameraFilterPack_TV_Artefact>(null);
		yield return R.Audio.PlayVoiceOver("e18t17", null, false);
		R.Ui.Toast.Show(ScriptLocalization.Story.e18s3, 2f, true);
		yield return R.Audio.PlayVoiceOver("e18t18", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Artefact>(null);
		yield return R.Audio.PlayVoiceOver("e18t19", null, false);
		CameraFilterUtils.Create<CameraFilterPack_TV_Chromatical2>(null);
		R.Ui.Toast.Show(ScriptLocalization.Story.e18s4, 2f, true);
		yield return new WaitForSeconds(3f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical2>(null);
		yield return R.Audio.PlayVoiceOver("e18t20", null, false);
		R.Trophy.AwardTrophy(20);
		this.gate2.openType = SceneGate.OpenType.Right;
		yield break;
	}

	private IEnumerator PlayerMove()
	{
		yield return new WaitForSeconds(0.1f);
		R.Player.Action.TurnRound(-1);
		InputSetting.Stop(false);
		R.Player.ActionController.StartMove();
		while (R.Player.Transform.position.x - this.playerStop.position.x > 0f)
		{
			yield return null;
		}
		R.Player.ActionController.StopMove();
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private Transform spwanPoint;

	[SerializeField]
	private Transform playerStop;

	[SerializeField]
	private GameObject dahalThanks;

	[SerializeField]
	private BattleZoneGate _battleZoneGateL;

	[SerializeField]
	private BattleZoneGate _battleZoneGateR;
}
