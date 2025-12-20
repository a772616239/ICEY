using System;
using ExtensionMethods;
using UnityEngine;

public class RiderGunAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(RiderGunAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(RiderGunAction.StateEnum.Ready, 1f);
	}

	protected override void Update()
	{
		base.Update();
		if (this.IsInDefenceState() && Time.time - this.startDefenceTime >= this.defenceTime)
		{
			base.AnimChangeState(RiderGunAction.StateEnum.Ready, 1f);
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		RiderGunAction.StateEnum stateEnum = EnumTools.ToEnum<RiderGunAction.StateEnum>(args.state, false);
		switch (stateEnum)
		{
		case RiderGunAction.StateEnum.Atk1:
		case RiderGunAction.StateEnum.Atk2Ready:
		case RiderGunAction.StateEnum.Atk2End:
		case RiderGunAction.StateEnum.Atk3:
		case RiderGunAction.StateEnum.Defense:
		case RiderGunAction.StateEnum.DieQTE:
		case RiderGunAction.StateEnum.Hit1:
		case RiderGunAction.StateEnum.HitQTE:
		case RiderGunAction.StateEnum.HitToWeakMod:
		case RiderGunAction.StateEnum.GunAtk2Ready:
		case RiderGunAction.StateEnum.GunAtk2:
		case RiderGunAction.StateEnum.GunAtk2End:
			this.spineAnim.Play(stateEnum, false, true, 1f);
			break;
		case RiderGunAction.StateEnum.Atk2:
		case RiderGunAction.StateEnum.DefenseState:
		case RiderGunAction.StateEnum.Move:
		case RiderGunAction.StateEnum.WeakMod:
		case RiderGunAction.StateEnum.Ready:
		case RiderGunAction.StateEnum.MoveFast:
			this.spineAnim.Play(stateEnum, true, false, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.Atk2Ready, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.Atk3, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.GunAtk2Ready, 1f);
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
		if (this.stateMachine.currentState.IsInArray(RiderGunAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		base.AnimChangeState(RiderGunAction.StateEnum.Atk1, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(RiderGunAction.StateEnum.Ready, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(RiderGunAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state == "DieQTE";
	}

	public override void AnimReady()
	{
		base.AnimChangeState(RiderGunAction.StateEnum.Ready, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(RiderGunAction.StateEnum.MoveFast, 1f);
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
		if (this.stateMachine.currentState.IsInArray(RiderGunAction.QTESta))
		{
			return;
		}
		base.Defence();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.startDefenceTime = Time.time;
		this.defenceTime = UnityEngine.Random.Range(2f, 4f);
		base.AnimChangeState(RiderGunAction.StateEnum.DefenseState, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.Defense, 1f);
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(RiderGunAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(RiderGunAction.AttackSta) && !lastState.IsInArray(RiderGunAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(RiderGunAction.AttackSta) && lastState.IsInArray(RiderGunAction.AttackSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInDefenceState()
	{
		return this.stateMachine.currentState.IsInArray(RiderGunAction.DefenseSta);
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
		base.AnimChangeState(RiderGunAction.StateEnum.HitQTE, 1f);
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
		base.AnimChangeState(RiderGunAction.StateEnum.DieQTE, 1f);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Ready";
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Move",
		"Ready",
		"MoveFast"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2Ready",
		"Atk2",
		"Atk2End",
		"Atk3",
		"GunAtk2Ready",
		"GunAtk2",
		"GunAtk2End"
	};

	private static readonly string[] DefenseSta = new string[]
	{
		"Defense",
		"DefenseState"
	};

	public static readonly string[] QTESta = new string[]
	{
		"DieQTE",
		"HitQTE"
	};

	public enum StateEnum
	{
		Atk1,
		Atk2Ready,
		Atk2,
		Atk2End,
		Atk3,
		Defense,
		DefenseState,
		DieQTE,
		Hit1,
		HitQTE,
		HitToWeakMod,
		Move,
		WeakMod,
		Ready,
		GunAtk2Ready,
		GunAtk2,
		GunAtk2End,
		MoveFast
	}
}
