using System;
using System.Collections;
using Core;
using GameWorld;
using UnityEngine;

public class Level56DahalDieTrigger : BaseBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
		this.dahalDieFlag = false;
		this.dahalShotFlag = false;
		this.startCheckDahalShot = false;
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.DieEvent), EventManager.ListenerQueue.Game);
		Log.Info(string.Concat(new object[]
		{
			"[DahalDie] 触发器就绪 dieEvent=",
			(this.dahalDieEvent != null) ? this.dahalDieEvent.name : "NULL",
			" shotEvent=",
			(this.dahalShotEvent != null) ? this.dahalShotEvent.name : "NULL"
		}));
	}

	public void OnDestroy()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.DieEvent), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		this.WatchBossDead();
		this.DiagTick();
		this.WeakPhaseGuard();
		this.DieWatchdog();
		if (!this.startCheckDahalShot || this.dahalShotFlag)
		{
			return;
		}
		float x = this.player.position.x;
		bool isDead = R.Player.Attribute.isDead;
		bool isNormal = R.Player.Action.IsInNormalState();
		this._shotWaitTime += Time.unscaledDeltaTime;
		if (this._shotWaitTime >= this._nextShotLogTime)
		{
			this._nextShotLogTime += 1f;
			Log.Info(string.Concat(new object[]
			{
				"[DahalDie] 等待炸桥触发 playerX=",
				x.ToString("F2"),
				" dead=",
				isDead,
				" normal=",
				isNormal,
				" 触发条件 x>40"
			}));
		}
		if (!isDead && isNormal && x > 40f)
		{
			this.dahalShotFlag = true;
			this.DahalShot();
		}
	}

	private bool EnsureBoss()
	{
		if (this._bossAttr != null)
		{
			return true;
		}
		GameObject boss = R.Enemy.Boss;
		if (boss == null)
		{
			return false;
		}
		this._bossAttr = boss.GetComponent<EnemyAttribute>();
		if (this._bossAttr == null)
		{
			return false;
		}
		this._bossAction = boss.GetComponent<DahalAction>();
		this._bossHurt = boss.GetComponent<EnemyBaseHurt>();
		return true;
	}

	private void DiagTick()
	{
		this._diagWait += Time.unscaledDeltaTime;
		if (this._diagWait < 2f)
		{
			return;
		}
		this._diagWait = 0f;
		if (!this.EnsureBoss())
		{
			if (!this._bossNullLogged)
			{
				this._bossNullLogged = true;
				Log.Warning("[DahalDie] 跟踪：R.Enemy.Boss 为空（Boss 未注册），依赖 EnemyKilled 事件");
			}
			return;
		}
		DahalAction act = this._bossAction;
		EnemyBaseHurt hurt = this._bossHurt;
		float dist = Vector3.Distance(this._bossAttr.transform.position, this.player.position);
		Log.Info(string.Concat(new object[]
		{
			"[DahalDie] 跟踪 hp=",
			this._bossAttr.currentHp,
			"/",
			this._bossAttr.maxHp,
			" phase=",
			(hurt != null) ? hurt.currentPhase : -1,
			" state=",
			(act != null && act.stateMachine != null) ? act.stateMachine.currentState : "null",
			" weak=",
			this._bossAttr.inWeakState,
			" onGround=",
			this._bossAttr.isOnGround,
			" dist=",
			dist.ToString("F2"),
			" willExec=",
			this._bossAttr.willBeExecute,
			" acceptExec=",
			this._bossAttr.accpectExecute,
			" dead=",
			this._bossAttr.isDead,
			" dieFlag=",
			this.dahalDieFlag,
			" px=",
			this.player.position.x.ToString("F2")
		}));
	}

	private void WeakPhaseGuard()
	{
		if (this.dahalDieFlag || !this.EnsureBoss())
		{
			return;
		}
		if (this._bossAttr.isDead || this._bossHurt == null || this._bossAction == null)
		{
			return;
		}
		int phase = this._bossHurt.currentPhase;
		if (phase != this._weakPhaseSeen)
		{
			if (this._weakPhaseSeen >= 0)
			{
				Log.Info(string.Concat(new object[]
				{
					"[DahalDie] 阶段推进到 ",
					phase,
					"，弱状态计时清零"
				}));
			}
			this._weakPhaseSeen = phase;
			this._weakAccum = 0f;
			this._weakHold = 0f;
			this._groundFixCount = 0;
		}
		if (!this._bossAttr.inWeakState)
		{
			this._weakHold = 0f;
			this._weakAccum = Mathf.Max(0f, this._weakAccum - Time.unscaledDeltaTime);
			return;
		}
		if (this.IsQTEStateRunning())
		{
			this._weakHold = 0f;
			return;
		}
		this._weakHold += Time.unscaledDeltaTime;
		this._weakAccum += Time.unscaledDeltaTime;
		if (this._weakHold > 0.1f && this._weakHold < 0.15f && !this._weakLogOnce)
		{
			this._weakLogOnce = true;
			Log.Info(string.Concat(new object[]
			{
				"[DahalDie] Boss 进入弱状态（血条已见底，等待处决）hp=",
				this._bossAttr.currentHp,
				" phase=",
				phase
			}));
		}
		if (!this._bossAttr.isOnGround && this._weakHold > 1.2f)
		{
			this._groundFixCount++;
			if (this._groundFixCount <= 6)
			{
				Log.Warning(string.Concat(new object[]
				{
					"[DahalDie] 弱状态悬空修正：Boss 不在落地状态（处决判定需要 onGround），强制落地 posY=",
					this._bossAttr.transform.position.y.ToString("F2")
				}));
			}
			Vector3 position = this._bossAttr.transform.position;
			position.y = LayerManager.YNum.GetGroundHeight(this._bossAttr.gameObject);
			this._bossAttr.transform.position = position;
			this._bossAttr.isFlyingUp = false;
			this._bossAttr.checkHitGround = false;
			this._bossAttr.timeController.SetGravity(1f);
			this._bossAttr.timeController.SetSpeed(Vector2.zero);
			this._weakHold = 0f;
			return;
		}
		float fastLimit = (phase <= 0) ? 3.5f : 2f;
		if (this._weakHold >= fastLimit)
		{
			this.DoQTE(phase);
			return;
		}
		float limit = (phase <= 0) ? 8f : 12f;
		if (this._weakAccum >= limit)
		{
			this.DoQTE(phase);
		}
	}

	private bool IsQTEStateRunning()
	{
		if (this._bossAction == null || this._bossAction.stateMachine == null)
		{
			return false;
		}
		string currentState = this._bossAction.stateMachine.currentState;
		if (string.IsNullOrEmpty(currentState))
		{
			return false;
		}
		return currentState.StartsWith("QTE") || currentState.StartsWith("Execute") || currentState == "DieIdle" || currentState == "Dead";
	}

	private void DoQTE(int phase)
	{
		this._weakAccum = 0f;
		this._weakHold = 0f;
		this._weakLogOnce = false;
		if (phase <= 0)
		{
			this._qteAttempts++;
			Log.Warning(string.Concat(new object[]
			{
				"[DahalDie] 处决兜底：第一段血量被锁在 ",
				this._bossAttr.currentHp,
				"，长时间没有触发处决，代替玩家执行 QTE（第 ",
				this._qteAttempts,
				" 次，phase 0 → 1）"
			}));
			this._bossAction.AnimQTEHurt();
			this._bossAttr.timeController.SetSpeed(Vector2.zero);
			if (R.Player.Action.IsInNormalState())
			{
				R.Player.Action.ChangeState(PlayerAction.StateEnum.Disappear, 1f);
			}
			if (this._qteAttempts >= 2)
			{
				Log.Warning("[DahalDie] 处决兜底：QTEHit 动画的事件没有推进阶段，直接调用 QTEHurt 结算阶段血量");
				this._bossHurt.QTEHurt();
				this._qteAttempts = 0;
			}
			return;
		}
		Log.Warning(string.Concat(new object[]
		{
			"[DahalDie] 处决兜底：第二段血量被锁在 ",
			this._bossAttr.currentHp,
			"，长时间没有触发处决，直接执行处决（phase ",
			phase,
			" → 死亡）"
		}));
		this._bossAction.AnimExecute();
		this._bossAttr.timeController.SetSpeed(Vector2.zero);
		this._bossAttr.currentHp = 0;
	}

	private void DieWatchdog()
	{
		if (!this.dahalDieFlag)
		{
			return;
		}
		this._sinceDie += Time.unscaledDeltaTime;
		if (!this.startCheckDahalShot && this._sinceDie > 30f)
		{
			this.startCheckDahalShot = true;
			Log.Warning("[DahalDie] 看门狗：30s 仍未开始炸桥检测，强制开启");
		}
		if (this.startCheckDahalShot && !this.dahalShotFlag && this._sinceDie > 45f)
		{
			this.dahalShotFlag = true;
			Log.Warning("[DahalDie] 看门狗：45s 仍未触发炸桥，忽略玩家坐标强制激活炸桥演出");
			this.DahalShot();
		}
		if (this.dahalShotFlag)
		{
			this._sinceShot += Time.unscaledDeltaTime;
			if (this._sinceShot > 25f)
			{
				this._sinceShot = -99999f;
				Log.Warning("[DahalDie] 看门狗：炸桥演出 25s 未切关（clip 长 18s，SwitchLevel 事件在 t=17.92），直接调用 SwitchLevel 强制进下一关");
				SceneStory.SwitchLevelGateAnimControlTools tools = UnityEngine.Object.FindObjectOfType<SceneStory.SwitchLevelGateAnimControlTools>();
				if (tools != null)
				{
					tools.SwitchLevel();
				}
				else
				{
					Log.Error("[DahalDie] 找不到 SwitchLevelGateAnimControlTools，无法强制切关");
				}
			}
		}
	}

	private void WatchBossDead()
	{
		if (this.dahalDieFlag)
		{
			return;
		}
		if (!this.EnsureBoss())
		{
			return;
		}
		if (this._bossAttr.currentHp <= 0)
		{
			Log.Info("[DahalDie] 兜底：Boss 血量<=0 但未收到 EnemyKilled 事件，主动触发死亡演出");
			this.DieEvent("EnemyKilled", this._bossAttr.gameObject, EventArgs.Empty);
		}
	}

	private bool DieEvent(string eventName, object sender, EventArgs msg)
	{
		if (this.dahalDieFlag)
		{
			return true;
		}
		this.dahalDieFlag = true;
		this._sinceDie = 0f;
		DahalAction component = null;
		if (R.Enemy.Boss != null)
		{
			component = R.Enemy.Boss.GetComponent<DahalAction>();
		}
		if (component == null)
		{
			component = UnityEngine.Object.FindObjectOfType<DahalAction>();
		}
		if (component != null)
		{
			component.DahalDie = true;
			this.StopBossAI(component);
			Log.Info("[DahalDie] 收到 EnemyKilled，bossState=" + ((component.stateMachine != null) ? component.stateMachine.currentState : "null"));
		}
		else
		{
			Log.Error("[DahalDie] 找不到 DahalAction（Boss 未注册），跳过 QTEDie 等待直接进入逃脱演出");
		}
		EventManager.PostEvent<Level56DahalDieTrigger, BattleEventArgs>("Battle", this, BattleEventArgs.End);
		base.StartCoroutine(this.DieEventStart(component));
		return true;
	}

	private void StopBossAI(DahalAction dAction)
	{
		if (dAction == null)
		{
			return;
		}
		BehaviorDesigner.Runtime.BehaviorTree tree = dAction.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
		if (tree != null && tree.enabled)
		{
			tree.enabled = false;
			Log.Info("[DahalDie] 已关闭 Boss 行为树，防止死后继续出招");
		}
		if (dAction.hurtBox != null && dAction.hurtBox.gameObject.activeSelf)
		{
			dAction.hurtBox.gameObject.SetActive(false);
		}
	}

	private IEnumerator DieEventStart(DahalAction dAction)	{
		float waited = 0f;
		while (dAction != null && dAction.stateMachine != null && dAction.stateMachine.currentState == "QTEDie" && waited < 25f)
		{
			waited += Time.unscaledDeltaTime;
			yield return null;
		}
		Log.Info(string.Concat(new object[]
		{
			"[DahalDie] QTEDie 等待结束 waited=",
			waited.ToString("F2"),
			"s state=",
			(dAction != null && dAction.stateMachine != null) ? dAction.stateMachine.currentState : "null",
			(dAction != null && waited >= 25f) ? " (超时兜底)" : string.Empty
		}));
		this.InputSet(false);
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		yield return new WaitForSeconds(0.5f);
		if (this.dahalDieEvent != null)
		{
			this.dahalDieEvent.SetActive(true);
			Log.Info("[DahalDie] 逃脱演出已激活 " + this.dahalDieEvent.name);
		}
		else
		{
			Log.Error("[DahalDie] dahalDieEvent 未配置，逃脱演出跳过");
		}
		yield return new WaitForSeconds(2f);
		this.startCheckDahalShot = true;
		Log.Info("[DahalDie] 开始检测炸桥区域");
		yield break;
	}

	private void DahalShot()
	{
		if (this.dahalShotEvent != null)
		{
			this.dahalShotEvent.SetActive(true);
			this.InputSet(true);
			this._sinceShot = 0f;
			Log.Info("[DahalDie] 炸桥演出已激活 " + this.dahalShotEvent.name);
			return;
		}
		Log.Error("[DahalDie] dahalShotEvent 未配置，炸桥演出跳过");
	}

	private void InputSet(bool open)
	{
		Core.Input.Game.Jump.IsOpen = open;
		Core.Input.Game.Flash.Down.IsOpen = open;
		Core.Input.Game.Flash.FaceDirection.IsOpen = open;
		Core.Input.Game.Flash.Left.IsOpen = open;
		Core.Input.Game.Flash.LeftDown.IsOpen = open;
		Core.Input.Game.Flash.LeftUp.IsOpen = open;
		Core.Input.Game.Flash.Right.IsOpen = open;
		Core.Input.Game.Flash.RightDown.IsOpen = open;
		Core.Input.Game.Flash.RightUp.IsOpen = open;
		Core.Input.Game.Flash.Up.IsOpen = open;
	}

	[Header("死亡飞走事件")]
	public GameObject dahalDieEvent;

	[Header("炸桥事件")]
	public GameObject dahalShotEvent;

	private bool dahalDieFlag;

	private bool dahalShotFlag;

	private bool startCheckDahalShot;

	private float _shotWaitTime;

	private float _nextShotLogTime = 1f;

	private EnemyAttribute _bossAttr;

	private DahalAction _bossAction;

	private EnemyBaseHurt _bossHurt;

	private float _diagWait;

	private bool _bossNullLogged;

	private float _sinceDie;

	private float _sinceShot = -99999f;

	private float _weakHold;

	private float _weakAccum;

	private int _weakPhaseSeen = -1;

	private int _groundFixCount;

	private bool _weakLogOnce;

	private int _qteAttempts;
}
