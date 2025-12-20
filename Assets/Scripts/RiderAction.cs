using System;
using ExtensionMethods;
using UnityEngine;

public class RiderAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(RiderAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
	}

	protected override void Update()
	{
		base.Update();
		if (this.IsInDefenceState() && Time.time - this.startDefenceTime >= this.defenceTime)
		{
			base.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk3":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "Atk2":
			this.spineAnim.Play(args.state, true, false, this.eAttr.atkSpeed);
			break;
		case "Atk2End":
		case "Atk2Ready":
		case "Die":
		case "Hit1":
		case "Die2":
		case "DieQTE":
		case "HitQTE":
		case "Defense":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Idle":
		case "Move":
		case "MoveFast":
		case "Ready":
		case "WeakMod":
		case "DefenseState":
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
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
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
		base.AnimChangeState(RiderAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(RiderAction.StateEnum.Atk2Ready, 1f);
	}

	public override void Attack3(int dir)
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
		base.AnimChangeState(RiderAction.StateEnum.Atk3, 1f);
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
		if (this.stateMachine.currentState.IsInArray(RiderAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		base.AnimChangeState(RiderAction.StateEnum.Atk1, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(RiderAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state == "DieQTE";
	}

	public override void AnimReady()
	{
		base.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(RiderAction.StateEnum.MoveFast, 1f);
	}

	public override void Defence()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInDefenceState())
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
		if (this.stateMachine.currentState.IsInArray(RiderAction.QTESta))
		{
			return;
		}
		base.Defence();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.startDefenceTime = Time.time;
		this.defenceTime = UnityEngine.Random.Range(2f, 4f);
		base.AnimChangeState(RiderAction.StateEnum.DefenseState, 1f);
	}

	public override void DefenceSuccess()
	{
		if (this.eAttr.isDead)
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
		R.Audio.PlayEffect(184, new Vector3?(base.transform.position));
		base.AnimChangeState(RiderAction.StateEnum.Defense, 1f);
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(RiderAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(RiderAction.AttackSta) && !lastState.IsInArray(RiderAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(RiderAction.AttackSta) && lastState.IsInArray(RiderAction.AttackSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInDefenceState()
	{
		return this.stateMachine.currentState.IsInArray(RiderAction.DefenseSta);
	}

	public override bool IsInSideStepState()
	{
		return false;
	}

	public override void AnimQTEHurt()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(RiderAction.StateEnum.HitQTE, 1f);
	}

	public override void AnimExecute()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(RiderAction.StateEnum.DieQTE, 1f);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Ready";
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Ready",
		"MoveFast"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk2End",
		"Atk2Ready"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit1",
		"HitQTE"
	};

	private static readonly string[] DefenseSta = new string[]
	{
		"Defense",
		"DefenseState"
	};

	private static readonly string[] QTESta = new string[]
	{
		"DieQTE",
		"HitQTE"
	};

	public enum StateEnum
	{
		Atk1,
		Atk2,
		Atk3,
		Atk2End,
		Atk2Ready,
		Die,
		Hit1,
		Idle,
		Move,
		Ready,
		WeakMod,
		Die2,
		Defense,
		DieQTE,
		HitQTE,
		DefenseState,
		MoveFast
	}
}
