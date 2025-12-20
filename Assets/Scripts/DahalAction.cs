using System;
using ExtensionMethods;
using UnityEngine;

public class DahalAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(DahalAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(DahalAction.StateEnum.Idle, 1f);
		this._dahalAnimListener = base.GetComponent<DahalAnimListener>();
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
				base.AnimChangeState(DahalAction.StateEnum.Atk10_3, 1f);
			}
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk12":
		case "Atk2":
		case "Atk5":
		case "Atk6":
		case "AirPush":
		case "Atk10_2":
			this.spineAnim.Play(args.state, true, false, this.eAttr.atkSpeed);
			break;
		case "Atk1End1":
		case "Atk1End2":
		case "Atk1Ready1":
		case "Atk1_1":
		case "Atk1_2":
		case "Atk1_3":
		case "Atk1Ready2":
		case "Atk1Ready3":
		case "Atk2End":
		case "Atk2Ready":
		case "Atk13":
		case "Atk3":
		case "Atk4":
		case "Atk4Charging":
		case "Atk4End":
		case "Atk4Ready":
		case "Atk5Ready":
		case "Atk5End":
		case "Atk6End":
		case "Atk6Ready":
		case "Atk7":
		case "Atk72":
		case "Atk8":
		case "Atk9":
		case "Atk82":
		case "Atk92":
		case "AirStandUp":
		case "AirPushEnd":
		case "Atk10_1":
		case "Atk10_3":
		case "Atk1Move1":
		case "Atk1Move2":
		case "Atk1Move3":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "Die":
		case "QTEDie":
		case "QTEDie2":
		case "Hit1":
		case "HitToFly1":
		case "HitToFly2":
		case "HitToFly3":
		case "GetUp":
		case "HitGround":
		case "GroundDodge":
		case "Angry":
		case "QTEHit":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Idle":
		case "DieIdle":
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
			this.Atk6 = false;
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
			this._dahalAnimListener.StopShoot();
			this._dahalAnimListener.StopATK6Effect();
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
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
		if (args.lastState == "Angry")
		{
			base.GetComponent<DahalHurt>().HitIntoSecondLevel();
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
		this.Atk1Used = true;
		base.AnimChangeState(DahalAction.StateEnum.Atk1Ready1, 1f);
	}

	public override void Attack4(int dir)
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
		base.AnimChangeState(DahalAction.StateEnum.Atk4Ready, 1f);
	}

	public override void Attack8(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.Atk8Uesd = true;
		base.AnimChangeState(DahalAction.StateEnum.Atk7, 1f);
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
		base.AnimChangeState(DahalAction.StateEnum.Atk9, 1f);
	}

	public override void Attack11(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.Atk10Uesd = true;
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(DahalAction.StateEnum.Atk10_1, 1f);
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
		base.AnimChangeState(DahalAction.StateEnum.GroundDodge, 1f);
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
		if (base.GetComponent<DahalHurt>().currentPhase != 1)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(DahalAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 4f, new Vector3(0f, 0f, 0f), default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.paBody = true;
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(60, 0.5f);
		base.AnimChangeState(DahalAction.StateEnum.AirStandUp, 1f);
	}

	public void Angry()
	{
		base.FaceToPlayer();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(DahalAction.StateEnum.Angry, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DahalAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(DahalAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(DahalAction.DieSta);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(DahalAction.StateEnum.Idle, 1f);
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
		base.AnimChangeState(DahalAction.StateEnum.QTEDie, 1f);
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
		base.AnimChangeState(DahalAction.StateEnum.QTEHit, 1f);
		Transform transform = R.Effect.Generate(178, null, base.transform.position, default(Vector3), default(Vector3), true);
		transform.localScale = base.transform.localScale;
	}

	public bool DahyalAutoMove(int type)
	{
		if (this.isAutoMoveing)
		{
			return true;
		}
		if (!this.IsInNormalState())
		{
			return false;
		}
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

	public override void AnimMove()
	{
		base.AnimChangeState(DahalAction.StateEnum.Move, 1f);
	}

	private void AnimMoveQuick()
	{
		base.AnimChangeState(DahalAction.StateEnum.Rush, 1f);
	}

	public override bool IsInSideStepState()
	{
		return this.stateMachine.currentState.IsInArray(DahalAction.SideStepSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(DahalAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(DahalAction.AttackSta) && !lastState.IsInArray(DahalAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(DahalAction.AttackSta) && lastState.IsInArray(DahalAction.AttackSta);
	}

	public bool Atk6;

	private Vector3 _atk102StartPos;

	public bool Atk1Used;

	public bool Atk8Uesd;

	public bool Atk10Uesd;

	public bool DahalDie;

	public static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move",
		"Rush"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1End1",
		"Atk1End2",
		"Atk1Ready1",
		"Atk1Ready2",
		"Atk1Ready3",
		"Atk2",
		"Atk2End",
		"Atk2Ready",
		"Atk3",
		"Atk1_1",
		"Atk12",
		"Atk13",
		"Atk5Ready",
		"Atk5",
		"Atk5End",
		"Atk6Ready",
		"Atk6",
		"Atk6End",
		"Atk7",
		"Atk8",
		"Atk9",
		"Atk82",
		"Atk92",
		"AirPush",
		"AirPushEnd",
		"AirStandUp",
		"Atk10_1",
		"Atk10_2",
		"Atk10_3",
		"Atk72",
		"Atk1Move1",
		"Atk1Move2",
		"Atk1Move3",
		"Atk1_2",
		"Atk1_3"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit1",
		"HitToFly1",
		"HitToFly2",
		"HitToFly3",
		"Fall",
		"HitGround",
		"GetUp",
		"QTEHit"
	};

	private static readonly string[] DieSta = new string[]
	{
		"Die",
		"QTEDie",
		"QTEDie2",
		"Atk4",
		"Atk4Charging",
		"Atk4End",
		"Atk4Ready",
		"DieIdle"
	};

	private static readonly string[] SideStepSta = new string[]
	{
		"GroundDodge",
		"Rush"
	};

	private static readonly string[] QTESta = new string[]
	{
		"QTEDie",
		"QTEDie2",
		"QTEHit"
	};

	private DahalAnimListener _dahalAnimListener;

	private readonly float _airFric = 8f;

	public enum StateEnum
	{
		Atk1End1,
		Atk1End2,
		Atk1Ready1,
		Atk1Ready2,
		Atk1Ready3,
		Atk2,
		Atk2End,
		Atk2Ready,
		Atk3,
		Atk4,
		Atk4Charging,
		Atk4End,
		Atk4Ready,
		Atk1_1,
		Atk12,
		Atk13,
		Die,
		Hit1,
		Idle,
		Move,
		WeakMod,
		Atk5Ready,
		Atk5,
		Atk5End,
		Atk6Ready,
		Atk6,
		Atk6End,
		Atk7,
		Atk8,
		Atk9,
		Atk82,
		Atk92,
		HitToFly1,
		HitToFly2,
		HitToFly3,
		Fall,
		HitGround,
		GetUp,
		AirPush,
		AirPushEnd,
		AirStandUp,
		Atk10_1,
		Atk10_2,
		Atk10_3,
		GroundDodge,
		Atk72,
		Atk1Move1,
		Atk1Move2,
		Atk1Move3,
		Atk1_2,
		Atk1_3,
		Rush,
		Angry,
		QTEDie,
		QTEDie2,
		QTEHit,
		DieIdle
	}
}
