using System;
using System.Collections.Generic;
using ExtensionMethods;

public class GiantRobotAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map4;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(GiantRobotAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(GiantRobotAction.StateEnum.IdleToReady, 1f);
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (GiantRobotAction._003C_003Ef__switch_0024map4 == null)
			{
				GiantRobotAction._003C_003Ef__switch_0024map4 = new Dictionary<string, int>(14)
				{
					{
						"Atk1",
						0
					},
					{
						"Atk2",
						0
					},
					{
						"Atk3",
						0
					},
					{
						"Atk4",
						0
					},
					{
						"Atk5",
						0
					},
					{
						"IdleToReady",
						0
					},
					{
						"LHandHit",
						0
					},
					{
						"ReadyToWeakMod",
						0
					},
					{
						"RHandHit",
						0
					},
					{
						"WeakModToReady",
						0
					},
					{
						"DieQTE",
						0
					},
					{
						"WeakMod",
						1
					},
					{
						"Idle",
						1
					},
					{
						"Ready",
						1
					}
				};
			}
			int num;
			if (GiantRobotAction._003C_003Ef__switch_0024map4.TryGetValue(state, out num))
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

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.nextState == "ReadyToWeakMod")
		{
			base.GetComponent<GiantRobotAnimEvent>().DisappearHalo();
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
		base.AnimChangeState(GiantRobotAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(GiantRobotAction.StateEnum.Atk2, 1f);
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
		base.AnimChangeState(GiantRobotAction.StateEnum.Atk3, 1f);
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
		base.AnimChangeState(GiantRobotAction.StateEnum.Atk4, 1f);
	}

	public override void Attack5(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(GiantRobotAction.StateEnum.Atk5, 1f);
	}

	public override void AnimMove()
	{
	}

	public override void AnimReady()
	{
		base.AnimChangeState(GiantRobotAction.StateEnum.Ready, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState == "Ready" && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(GiantRobotAction.AttackSta);
	}

	public override bool IsInDeadState(string state)
	{
		return state == "DieQTE";
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(GiantRobotAction.AttackSta) && !lastState.IsInArray(GiantRobotAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(GiantRobotAction.AttackSta) && lastState.IsInArray(GiantRobotAction.AttackSta);
	}

	public override void AnimExecute()
	{
		base.ExitWeakState(true);
		base.AnimChangeState(GiantRobotAction.StateEnum.DieQTE, 1f);
	}

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5"
	};

	public enum StateEnum
	{
		Idle,
		IdleToReady,
		Ready,
		Atk1,
		Atk2,
		Atk3,
		LHandHit,
		RHandHit,
		ReadyToWeakMod,
		WeakMod,
		WeakModToReady,
		Atk4,
		Atk5,
		DieQTE
	}
}
