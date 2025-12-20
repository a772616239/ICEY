using System;
using ExtensionMethods;

public class ThiefBombAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(ThiefBombAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		base.AnimChangeState(ThiefBombAction.StateEnum.Idle, 1f);
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (!(state == "Bomb") && !(state == "Hit1") && !(state == "Hit2") && !(state == "LookGround"))
			{
				if (state == "Idle" || state == "Walk")
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

	public void LookRound()
	{
		base.AnimChangeState(ThiefBombAction.StateEnum.LookGround, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(ThiefBombAction.StateEnum.Walk, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(ThiefBombAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(ThiefBombAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	public override bool IsInAttackState()
	{
		return false;
	}

	public override bool IsInDeadState(string state)
	{
		return state == "Bomb";
	}

	public override bool IsInWeakSta()
	{
		return false;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return false;
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return false;
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Walk",
		"LookGround"
	};

	public enum StateEnum
	{
		Idle,
		Bomb,
		Hit1,
		Hit2,
		LookGround,
		Walk
	}
}
