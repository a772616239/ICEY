using System;
using ExtensionMethods;
using UnityEngine;

public class DahalFAction : EnemyBaseAction
{
	protected override void Awake()
	{
		base.Awake();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(DahalFAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(DahalFAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
			this.eAttr.timeController.SetSpeed(currentSpeed);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.stateMachine.currentState == "Idle")
		{
			base.FaceToPlayer();
		}
		if (this.stateMachine.currentState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			if (currentSpeed.y > 0f)
			{
				currentSpeed.y = 0f;
				this.eAttr.timeController.SetSpeed(currentSpeed);
			}
		}
		if (this.stateMachine.currentState == "Atk10_2")
		{
			this.eAttr.timeController.SetSpeed(Vector2.zero);
			if (base.transform.position.y - this._atk102StartPos.y >= 4f)
			{
				base.FaceToPlayer();
				base.AnimChangeState(DahalFAction.StateEnum.Atk10_3, 1f);
			}
		}
		this.UpdateHp();
	}

	private void UpdateHp()
	{
		switch (R.GameData.Difficulty)
		{
		case 0:
		case 1:
			this.NormalModeHp();
			break;
		case 2:
		case 3:
			this.HardModeHp();
			break;
		}
	}

	private void NormalModeHp()
	{
		if (this.eAttr.currentHp < this.eAttr.maxHp * 60 / 100 && this._first60)
		{
			this._first60 = false;
			this.UseAtk2 = true;
		}
		if (this.eAttr.currentHp >= this.eAttr.maxHp * 35 / 100 || !this._first30)
		{
			return;
		}
		this._first30 = false;
		this.UseAtk2 = true;
	}

	private void HardModeHp()
	{
		if (this.eAttr.currentHp < this.eAttr.maxHp * 75 / 100 && this._first75)
		{
			this._first75 = false;
			this.UseAtk2 = true;
		}
		if (this.eAttr.currentHp < this.eAttr.maxHp * 50 / 100 && this._first50)
		{
			this._first50 = false;
			this.UseAtk2 = true;
		}
		if (this.eAttr.currentHp >= this.eAttr.maxHp * 25 / 100 || !this._first25)
		{
			return;
		}
		this._first25 = false;
		this.UseAtk2 = true;
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "AirPush":
		case "Atk10_2":
		case "Atk2":
		case "Atk6":
			this.spineAnim.Play(args.state, true, false, this.eAttr.atkSpeed);
			break;
		case "Atk1End1":
		case "Atk1Ready1":
		case "Atk1_1":
		case "Atk1_2":
		case "Atk1_3":
		case "Atk7":
		case "Atk72":
		case "Atk9":
		case "AirStandUp":
		case "AirPushEnd":
		case "Atk10_1":
		case "Atk10_3":
		case "Atk1Move1":
		case "Atk1Move2":
		case "Atk1Move3":
		case "Atk2Ready":
		case "Atk2End":
		case "Atk6Ready":
		case "Atk6End":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "QTEDie2":
		case "Hit1":
		case "HitToFly1":
		case "HitToFly2":
		case "HitToFly3":
		case "GetUp":
		case "HitGround":
		case "GroundDodge":
		case "QTEHit":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Idle":
		case "Move":
		case "WeakMod":
		case "Fall":
		case "Rush":
			this.spineAnim.Play(args.state, true, false, 1f);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
			DahalFAnim component = base.GetComponent<DahalFAnim>();
			component.StopShoot();
			component.StopATK6Effect();
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			this._enemyAtk.atkId = Incrementor.GetNextId();
		}
		if (args.nextState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			this.eAttr.timeController.SetSpeed(currentSpeed);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == "HitToFly3" && !this.eAttr.willBeExecute)
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "Atk10_2")
		{
			this._atk102StartPos = base.transform.position;
		}
		if (args.nextState == "Atk1Ready1" || args.nextState == "AirStandUp" || args.nextState == "Atk92")
		{
			this.eAttr.timeController.SetSpeed(Vector2.zero);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.nextState == "Fall")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
	}

	public override void Attack1(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DahalFAction.StateEnum.Atk1Ready1, 1f);
	}

	public override void Attack2(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		this.Atk2HitWallTimes = 0;
		base.AnimChangeState(DahalFAction.StateEnum.Atk2Ready, 1f);
	}

	public override void Attack6(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		this.Atk6HitWall = false;
		base.AnimChangeState(DahalFAction.StateEnum.Atk6Ready, 1f);
	}

	public override void Attack7(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DahalFAction.StateEnum.Atk7, 1f);
	}

	public override void Attack9(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DahalFAction.StateEnum.Atk9, 1f);
	}

	public override void Attack10(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(DahalFAction.StateEnum.Atk10_1, 1f);
	}

	public override void SideStep()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInSideStepState())
		{
			return;
		}
		if (!this.eAttr.isOnGround)
		{
			return;
		}
		if (this.IsInWeakSta())
		{
			return;
		}
		base.SideStep();
		base.AnimChangeState(DahalFAction.StateEnum.GroundDodge, 1f);
	}

	public override void CounterAttack(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInAttackState())
		{
			return;
		}
		if (base.GetComponent<DahalFHurt>().currentPhase != 1)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(DahalFAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 4f, new Vector3(0f, 0f, 0f), default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.paBody = true;
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(60, 0.5f);
		base.AnimChangeState(DahalFAction.StateEnum.AirStandUp, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DahalFAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(DahalFAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state == "QTEDie2";
	}

	public override void AnimReady()
	{
		base.AnimChangeState(DahalFAction.StateEnum.Idle, 1f);
	}

	public override void AnimExecute()
	{
		base.AnimExecute();
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		base.WeakEffectDisappear("RollEnd");
		R.Camera.Controller.CameraZoom(base.transform.position + Vector3.up * 3f, 0.166666672f, 2f);
		R.Camera.Controller.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		base.AnimChangeState(DahalFAction.StateEnum.QTEDie2, 1f);
	}

	public override void AnimQTEHurt()
	{
		base.AnimQTEHurt();
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		R.Camera.Controller.CameraZoom(base.transform.position + Vector3.up * 3f, 0.166666672f, 2f);
		R.Camera.Controller.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		base.AnimChangeState(DahalFAction.StateEnum.QTEHit, 1f);
		Transform transform = R.Effect.Generate(178, null, base.transform.position, default(Vector3), default(Vector3), true);
		transform.localScale = base.transform.localScale;
	}

	public bool DahyalAutoMove(int type)
	{
		if (this.isAutoMoveing)
		{
			return true;
		}
		bool flag = this.IsInNormalState();
		if (flag)
		{
			this.isAutoMoveing = true;
			if (type != 1)
			{
				if (type == 2)
				{
					this.AnimMoveQuick();
				}
			}
			else
			{
				this.AnimMove();
			}
			return true;
		}
		return false;
	}

	public override void AnimMove()
	{
		base.AnimChangeState(DahalFAction.StateEnum.Move, 1f);
	}

	private void AnimMoveQuick()
	{
		base.AnimChangeState(DahalFAction.StateEnum.Rush, 1f);
	}

	public override bool IsInSideStepState()
	{
		return this.stateMachine.currentState.IsInArray(DahalFAction.SideStepSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(DahalFAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(DahalFAction.AttackSta) && !lastState.IsInArray(DahalFAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(DahalFAction.AttackSta) && lastState.IsInArray(DahalFAction.AttackSta);
	}

	private Vector3 _atk102StartPos;

	private readonly float _airFric = 8f;

	public bool UseAtk2;

	private bool _first60 = true;

	private bool _first30 = true;

	private bool _first75 = true;

	private bool _first50 = true;

	private bool _first25 = true;

	public bool Atk6HitWall;

	public int Atk2HitWallTimes;

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move",
		"Rush"
	};

	public static readonly string[] AttackSta = new string[]
	{
		"Atk1_1",
		"Atk1_2",
		"Atk1_3",
		"Atk1Move1",
		"Atk1Move2",
		"Atk1Move3",
		"Atk1End1",
		"Atk1Ready1",
		"Atk2",
		"Atk2End",
		"Atk2Ready",
		"Atk6Ready",
		"Atk6",
		"Atk6End",
		"Atk7",
		"Atk72",
		"Atk9",
		"AirPush",
		"AirPushEnd",
		"AirStandUp",
		"Atk10_1",
		"Atk10_2",
		"Atk10_3"
	};

	private static readonly string[] SideStepSta = new string[]
	{
		"GroundDodge",
		"Rush"
	};

	private static readonly string[] QTESta = new string[]
	{
		"QTEDie2",
		"QTEHit"
	};

	private EnemyAtk _enemyAtk;

	public enum StateEnum
	{
		Atk1_1,
		Atk1_2,
		Atk1_3,
		Atk1Move1,
		Atk1Move2,
		Atk1Move3,
		Atk12,
		Atk13,
		Atk1End1,
		Atk1End2,
		Atk1Ready1,
		Atk1Ready2,
		Atk1Ready3,
		Atk7,
		Atk72,
		Atk9,
		Atk92,
		AirPush,
		AirPushEnd,
		AirStandUp,
		Atk10_1,
		Atk10_2,
		Atk10_3,
		Idle,
		Move,
		Hit1,
		WeakMod,
		HitToFly1,
		HitToFly2,
		HitToFly3,
		Fall,
		HitGround,
		GetUp,
		GroundDodge,
		Rush,
		QTEDie2,
		QTEHit,
		Atk2Ready,
		Atk2,
		Atk2End,
		Atk6Ready,
		Atk6,
		Atk6End
	}
}
