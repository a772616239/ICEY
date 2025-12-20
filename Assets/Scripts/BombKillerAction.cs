using System;
using UnityEngine;

public class BombKillerAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(BombKillerAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(BombKillerAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState == "FlyToFall")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
			this.eAttr.timeController.SetSpeed(currentSpeed);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.atk1Success)
		{
			Vector3 position = base.player.position;
			position.x = base.transform.position.x;
			base.player.position = position;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		BombKillerAction.StateEnum stateEnum = EnumTools.ToEnum<BombKillerAction.StateEnum>(args.state, false);
		switch (stateEnum)
		{
		case BombKillerAction.StateEnum.Atk1Ready:
		case BombKillerAction.StateEnum.Atk1Success:
		case BombKillerAction.StateEnum.Atk1Fail:
		case BombKillerAction.StateEnum.Atk2:
		case BombKillerAction.StateEnum.Die:
		case BombKillerAction.StateEnum.ExecuteDie:
		case BombKillerAction.StateEnum.FlyToFall:
		case BombKillerAction.StateEnum.GetUp:
		case BombKillerAction.StateEnum.Hit1:
		case BombKillerAction.StateEnum.Hit2:
		case BombKillerAction.StateEnum.HitGround:
		case BombKillerAction.StateEnum.HitToFly1:
		case BombKillerAction.StateEnum.AirDie:
		case BombKillerAction.StateEnum.AirDieFlyToFall:
		case BombKillerAction.StateEnum.AirDieHitGround:
		case BombKillerAction.StateEnum.Null:
			this.spineAnim.Play(stateEnum, false, true, 1f);
			break;
		case BombKillerAction.StateEnum.Execute:
		case BombKillerAction.StateEnum.Fall:
		case BombKillerAction.StateEnum.HitToFly2:
		case BombKillerAction.StateEnum.Idle:
		case BombKillerAction.StateEnum.Move:
		case BombKillerAction.StateEnum.AirDieFall:
		case BombKillerAction.StateEnum.HitFall:
			this.spineAnim.Play(stateEnum, true, false, 1f);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (args.nextState == "FlyToFall")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			this.eAttr.timeController.SetSpeed(currentSpeed);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == "FlyToFall" && !this.eAttr.willBeExecute)
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (this.ExitAtkSta(args.lastState, args.nextState) && this.atk1Success)
		{
			this.atk1Success = false;
			base.GetComponent<BombKillerAnimEvent>().GenerateExplosion_Atk1();
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
		base.AnimChangeState(BombKillerAction.StateEnum.Atk1Ready, 1f);
	}

	public void Atk1Success()
	{
		Vector3 position = base.transform.position;
		position.z = base.player.position.z - 0.01f;
		base.transform.position = position;
		this.atk1Success = true;
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
		base.AnimChangeState(BombKillerAction.StateEnum.Atk2, 1f);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	public override bool IsInNormalState()
	{
		return EnumTools.IsInEnum<BombKillerAction.NormalSta>(this.stateMachine.currentState, false) && base.IsInNormalState();
	}

	public override void AnimMove()
	{
		base.AnimChangeState(BombKillerAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(BombKillerAction.StateEnum.Idle, 1f);
	}

	public override bool IsInAttackState()
	{
		return EnumTools.IsInEnum<BombKillerAction.AttackSta>(this.stateMachine.currentState, false);
	}

	public override bool IsInDeadState(string state)
	{
		return EnumTools.IsInEnum<BombKillerAction.DieSta>(state, false);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return EnumTools.IsInEnum<BombKillerAction.AttackSta>(nextState, false) && !EnumTools.IsInEnum<BombKillerAction.AttackSta>(lastState, false);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !EnumTools.IsInEnum<BombKillerAction.AttackSta>(nextState, false) && EnumTools.IsInEnum<BombKillerAction.AttackSta>(lastState, false);
	}

	public bool atk1Success;

	public float airFric = 8f;

	public enum StateEnum
	{
		Atk1Ready,
		Atk1Success,
		Atk1Fail,
		Atk2,
		Die,
		Execute,
		ExecuteDie,
		Fall,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly1,
		HitToFly2,
		Idle,
		Move,
		AirDie,
		AirDieFlyToFall,
		AirDieFall,
		AirDieHitGround,
		HitFall,
		Null
	}

	public enum AttackSta
	{
		Atk1Ready,
		Atk1Success,
		Atk1Fail,
		Atk2
	}

	public enum DieSta
	{
		Die,
		Execute,
		ExecuteDie,
		AirDie,
		AirFlyToFall,
		AirDieFall,
		AirDieHitGround,
		Null
	}

	public enum HurtSta
	{
		Fall,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly1,
		HitToFly2,
		HitFall
	}

	public enum NormalSta
	{
		Idle,
		Move
	}
}
