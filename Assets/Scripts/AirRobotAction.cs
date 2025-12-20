using System;
using ExtensionMethods;
using UnityEngine;

public class AirRobotAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(AirRobotAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(AirRobotAction.StateEnum.Idle, 1f);
		base.transform.position = base.transform.position.SetY(base.transform.position.y + 2.5f);
	}

	protected override void Update()
	{
		this.eAttr.stiffTime = Mathf.Clamp(this.eAttr.stiffTime - Time.deltaTime, 0f, float.PositiveInfinity);
		float gravity = (float)((!this.InGravitySta()) ? 0 : 1);
		this.eAttr.timeController.SetGravity(gravity);
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			this.isAutoMoveing = false;
		}
		if (!this.isAutoMoveing || SingletonMono<WorldTime>.Instance.IsFrozen)
		{
			return;
		}
		float num = Mathf.Abs(this.eAttr.moveSpeed * Time.deltaTime);
		Vector3 position = base.transform.position += new Vector3(num * (float)this.eAttr.faceDir, 0f, 0f);
		position.y = Mathf.Clamp(position.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 0.5f, float.PositiveInfinity);
		base.transform.position = position;
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (!(state == "Die") && !(state == "HitFall") && !(state == "HitGround"))
			{
				if (state == "Fall" || state == "Idle" || state == "MoveHorizontal")
				{
					this.spineAnim.Play(args.state, true, false, 1f);
				}
			}
			else
			{
				this.spineAnim.Play(args.state, false, true, 1f);
			}
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
		}
		if (args.nextState == "MoveHorizontal")
		{
			R.Audio.PlayEffect(87, new Vector3?(base.transform.position));
		}
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(AirRobotAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(AirRobotAction.DieSta);
	}

	private bool InGravitySta()
	{
		return this.stateMachine.currentState.IsInArray(AirRobotAction.GravitySta);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(AirRobotAction.StateEnum.Idle, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(AirRobotAction.StateEnum.Idle, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(AirRobotAction.StateEnum.MoveHorizontal, 1f);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return false;
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return false;
	}

	public override bool IsInAttackState()
	{
		return false;
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"MoveHorizontal"
	};

	private static readonly string[] DieSta = new string[]
	{
		"Die",
		"Fall",
		"HitFall",
		"HitGround"
	};

	private static readonly string[] GravitySta = new string[]
	{
		"Fall",
		"HitFall",
		"HitGround"
	};

	public enum StateEnum
	{
		Atk1,
		Atk1End,
		Atk1Ready,
		Die,
		Fall,
		GetUp,
		Hit1,
		HitFall,
		HitGround,
		Idle,
		MoveHorizontal,
		MoveVertical,
		DieFly,
		DieFall,
		DieHitGround,
		Hit2,
		EMP,
		HitToFly,
		Atk2,
		Atk2End,
		Atk2Ready
	}
}
