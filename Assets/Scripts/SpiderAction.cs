using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SpiderAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map13;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(SpiderAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(SpiderAction.StateEnum.Show, 1f);
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
		if (this.stateMachine.currentState == "FlyToFall")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			if (currentSpeed.y > 0f)
			{
				currentSpeed.y = 0f;
				this.eAttr.timeController.SetSpeed(currentSpeed);
			}
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
		if (args.lastState == "JumpForwad" || args.lastState == "JumpBack" || args.lastState == "Atk2")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (SpiderAction._003C_003Ef__switch_0024map13 == null)
			{
				SpiderAction._003C_003Ef__switch_0024map13 = new Dictionary<string, int>(21)
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
						"Die",
						0
					},
					{
						"Execute",
						0
					},
					{
						"FallHitGround",
						0
					},
					{
						"FlyToFall",
						0
					},
					{
						"GetUp",
						0
					},
					{
						"Hit1",
						0
					},
					{
						"HitToFly1",
						0
					},
					{
						"JumpBack",
						0
					},
					{
						"JumpForward",
						0
					},
					{
						"Null",
						0
					},
					{
						"Show",
						0
					},
					{
						"AirDieFlyToFall",
						0
					},
					{
						"AirDieHitGround",
						0
					},
					{
						"Fall",
						1
					},
					{
						"HitToFly2",
						1
					},
					{
						"Idle",
						1
					},
					{
						"Move",
						1
					},
					{
						"HitFall",
						1
					},
					{
						"AirDieFall",
						1
					}
				};
			}
			int num;
			if (SpiderAction._003C_003Ef__switch_0024map13.TryGetValue(state, out num))
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
		base.AnimChangeState(SpiderAction.StateEnum.Atk1, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(SpiderAction.StateEnum.Atk2, 1f);
	}

	public void Jump(bool back)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.FaceToPlayer();
		this.eAttr.timeController.SetGravity(0f);
		SpiderAction.StateEnum stateEnum = (!back) ? SpiderAction.StateEnum.JumpForward : SpiderAction.StateEnum.JumpBack;
		base.AnimChangeState(stateEnum, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(SpiderAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(SpiderAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(SpiderAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(SpiderAction.AttackSta);
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(SpiderAction.DieSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(SpiderAction.AttackSta) && !lastState.IsInArray(SpiderAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(SpiderAction.AttackSta) && lastState.IsInArray(SpiderAction.AttackSta);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	public float airFric = 8f;

	public static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	public static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Fall",
		"FallHitGround",
		"FlyToFall",
		"Hit1",
		"HitToFly1",
		"HitToFly2",
		"HitFall"
	};

	public static readonly string[] JumpSta = new string[]
	{
		"JumpForward",
		"JumpBack"
	};

	public static readonly string[] DieSta = new string[]
	{
		"Die",
		"Null",
		"AirDieFlyToFall",
		"AirDieFall",
		"AirDieHitGround"
	};

	public enum StateEnum
	{
		Idle,
		Move,
		Show,
		Atk1,
		Atk2,
		Die,
		Null,
		Execute,
		Fall,
		FallHitGround,
		FlyToFall,
		GetUp,
		Hit1,
		HitToFly1,
		HitToFly2,
		JumpForward,
		JumpBack,
		HitFall,
		AirDieFlyToFall,
		AirDieFall,
		AirDieHitGround
	}
}
