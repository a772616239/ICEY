using System;
using System.Collections.Generic;

public class FaustAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map3;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(FaustAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		base.AnimChangeState(FaustAction.StateEnum.Appear, 1f);
	}

	protected override void Update()
	{
		base.FaceToPlayer();
		base.Update();
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (FaustAction._003C_003Ef__switch_0024map3 == null)
			{
				FaustAction._003C_003Ef__switch_0024map3 = new Dictionary<string, int>(7)
				{
					{
						"Appear",
						0
					},
					{
						"CallToIdle",
						0
					},
					{
						"IdleToCall",
						0
					},
					{
						"DisAppear",
						0
					},
					{
						"Die",
						0
					},
					{
						"Idle",
						1
					},
					{
						"Call",
						1
					}
				};
			}
			int num;
			if (FaustAction._003C_003Ef__switch_0024map3.TryGetValue(state, out num))
			{
				if (num != 0)
				{
					if (num == 1)
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
	}

	public override void AnimMove()
	{
	}

	public override void AnimReady()
	{
		base.AnimChangeState(FaustAction.StateEnum.Idle, 1f);
	}

	public override bool IsInAttackState()
	{
		return false;
	}

	public override bool IsInDeadState(string state)
	{
		return state == "Die";
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

	public override bool IsInNormalState()
	{
		return base.IsInNormalState() && this.stateMachine.currentState == "Idle";
	}

	public enum StateEnum
	{
		Idle,
		Null,
		Appear,
		DisAppear,
		IdleToCall,
		Call,
		CallToIdle,
		Die
	}
}
