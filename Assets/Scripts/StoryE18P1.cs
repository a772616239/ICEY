using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE18P1 : BaseBehaviour
{
	private bool e18Start
	{
		get
		{
			return RoundStorage.Get("E18_Start", false);
		}
		set
		{
			RoundStorage.Set("E18_Start", value);
		}
	}

	private bool p1
	{
		get
		{
			return RoundStorage.Get("E18_P1", false);
		}
		set
		{
			RoundStorage.Set("E18_P1", value);
		}
	}

	private bool p2
	{
		get
		{
			return RoundStorage.Get("E18_P2", false);
		}
		set
		{
			RoundStorage.Set("E18_P2", value);
		}
	}

	private bool p3
	{
		get
		{
			return RoundStorage.Get("E18_P3", false);
		}
		set
		{
			RoundStorage.Set("E18_P3", value);
		}
	}

	private bool p4
	{
		get
		{
			return RoundStorage.Get("E18_P4", false);
		}
		set
		{
			RoundStorage.Set("E18_P4", value);
		}
	}

	private bool p5
	{
		get
		{
			return RoundStorage.Get("E18_P5", false);
		}
		set
		{
			RoundStorage.Set("E18_P5", value);
		}
	}

	private bool p6
	{
		get
		{
			return RoundStorage.Get("E18_P6", false);
		}
		set
		{
			RoundStorage.Set("E18_P6", value);
		}
	}

	private bool p7
	{
		get
		{
			return RoundStorage.Get("E18_P7", false);
		}
		set
		{
			RoundStorage.Set("E18_P7", value);
		}
	}

	private bool killTrinity
	{
		set
		{
			RoundStorage.Set("E18_KillTrinity", value);
		}
	}

	private bool trinityDie
	{
		get
		{
			return this.Trinity == null || this.Trinity.GetComponent<EnemyAttribute>().isDead;
		}
	}

	private void Start()
	{
		if (!this.e18Start)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
	}

	public void OnEnable()
	{
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private IEnumerator P1Coroutine()
	{
		InputSetting.Stop(false);
		while (!R.Player.Attribute.isOnGround)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		R.Player.ActionController.TurnRound(-1);
		R.Player.ActionController.StartMove();
		yield return new WaitForSeconds(2f);
		R.Player.ActionController.StopMove();
		float startTime = Time.unscaledTime;
		this.e18Start = true;
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		this._lGate.enabled = false;
		this._rGate.enabled = false;
		this.Trinity = Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.崔妮蒂, new Vector2?(this.spwanPoint.position), false, false);
		this.Trinity.GetComponent<TrinityAction>().FaceToPlayer();
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.CameraMoveTo(new Vector2(this.spwanPoint.position.x, R.Camera.Controller.MovableCamera.position.y), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(5f);
		R.Camera.Controller.IsFollowPivot = true;
		InputSetting.Resume(false);
		this.p1 = true;
		yield return R.Audio.PlayVoiceOver("e18t1", null, false);
		while (Time.unscaledTime - startTime < 20f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p2 = true;
			R.Audio.PlayVoiceOver("e18t2", null, false);
		}
		while (Time.unscaledTime - startTime < 30f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p3 = true;
			R.Audio.PlayVoiceOver("e18t3", null, false);
		}
		while (Time.unscaledTime - startTime < 45f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p4 = true;
			CameraFilterUtils.Create<CameraFilterPack_TV_Old>(null);
			yield return R.Audio.PlayVoiceOver("e18t5", null, false);
			CameraFilterUtils.Remove<CameraFilterPack_TV_Old>(null);
		}
		while (Time.unscaledTime - startTime < 60f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p5 = true;
			CameraFilterUtils.Create<CameraFilterPack_TV_PlanetMars>(null);
			yield return R.Audio.PlayVoiceOver("e18t6", null, false);
			R.Ui.Toast.Show(ScriptLocalization.Story.e18s1, 2f, true);
			yield return R.Audio.PlayVoiceOver("e18t7", null, false);
			CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		}
		while (Time.unscaledTime - startTime < 75f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p6 = true;
			yield return R.Audio.PlayVoiceOver("e18t8", null, false);
			CameraFilterUtils.Create<CameraFilterPack_TV_PlanetMars>(null);
			R.Ui.Toast.Show(ScriptLocalization.Story.e18s2, 2f, true);
			yield return R.Audio.PlayVoiceOver("e18t9", null, false);
			CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
			yield return R.Audio.PlayVoiceOver("e18t10", null, false);
		}
		while (Time.unscaledTime - startTime < 135f)
		{
			yield return null;
		}
		if (!this.trinityDie)
		{
			this.p7 = true;
			UnityEngine.Object.Destroy(this.Trinity.transform.Find("HurtBox").gameObject);
			yield return R.Audio.PlayVoiceOver("e18t11", null, false);
			this.gate1.openType = SceneGate.OpenType.Left;
			this.gate2.openType = SceneGate.OpenType.Right;
			this._lGate.enabled = true;
			this._rGate.enabled = true;
		}
		yield break;
	}

	private bool EnemyDie(string eventDefine, object sender, EventArgs msg)
	{
		this.killTrinity = true;
		this.DestroyCameraEffect();
		if ((this.p1 || this.p2 || this.p3) && !this.p4 && !this.p5 && !this.p6)
		{
			base.StartCoroutine(this.KillTrinityInP123());
			return true;
		}
		if (this.p1 && this.p2 && this.p3 && (this.p4 || this.p5 || this.p6))
		{
			base.StartCoroutine(this.KillTinityInP6());
			return true;
		}
		return true;
	}

	private IEnumerator KillTrinityInP123()
	{
		yield return R.Audio.PlayVoiceOver("e18t12", null, false);
		R.Mode.EnterMode(Mode.AllMode.Normal);
		this.gate2.openType = SceneGate.OpenType.Right;
		this._lGate.enabled = true;
		yield break;
	}

	private IEnumerator KillTinityInP6()
	{
		yield return R.Audio.PlayVoiceOver("e18t13", null, false);
		R.Mode.EnterMode(Mode.AllMode.Normal);
		this.gate2.openType = SceneGate.OpenType.Right;
		this._lGate.enabled = true;
		yield break;
	}

	private void DestroyCameraEffect()
	{
		if (!SingletonMono<CameraController>.ApplicationIsQuitting)
		{
			CameraFilterUtils.Remove<CameraFilterPack_TV_Old>(null);
			CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
			CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		}
	}

	private void OnDestroy()
	{
		this.DestroyCameraEffect();
	}

	private GameObject Trinity;

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private Collider2D _lGate;

	[SerializeField]
	private Collider2D _rGate;

	[SerializeField]
	private Transform spwanPoint;
}
