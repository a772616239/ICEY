using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FooterAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024mapF;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(FooterAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(FooterAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState == "FlyToFall")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - 8f * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - 8f * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
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
		if (this.stateMachine.currentState == "Atk1" || this.stateMachine.currentState == "Atk3")
		{
			this.eAttr.timeController.SetGravity(0f);
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
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
		if (args.nextState.IsInArray(FooterAction.NormalSta))
		{
			this.hurtBox.localScale = Vector3.one;
			this.eAttr.timeController.SetGravity(1f);
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (FooterAction._003C_003Ef__switch_0024mapF == null)
			{
				FooterAction._003C_003Ef__switch_0024mapF = new Dictionary<string, int>(24)
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
						"Die",
						0
					},
					{
						"DieQTE",
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
						"HitToFly",
						0
					},
					{
						"MoveAway",
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
						"ExecuteDie",
						0
					},
					{
						"Null",
						0
					},
					{
						"Jump",
						0
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
						"Fall",
						1
					},
					{
						"AirDie",
						1
					},
					{
						"AirDieFall",
						1
					},
					{
						"HitFall",
						1
					},
					{
						"Execute",
						1
					}
				};
			}
			int num;
			if (FooterAction._003C_003Ef__switch_0024mapF.TryGetValue(state, out num))
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(FooterAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(FooterAction.StateEnum.Atk2, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(FooterAction.StateEnum.Atk3, 1f);
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
		R.Effect.Generate(128, base.transform, Vector3.up * 1.5f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(FooterAction.StateEnum.Atk3, 1f);
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
		base.SideStep();
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(FooterAction.StateEnum.MoveAway, 1f);
	}

	public void Jump()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(FooterAction.StateEnum.Jump, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(FooterAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(FooterAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(FooterAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(FooterAction.AttackSta);
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(FooterAction.DieSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(FooterAction.AttackSta) && !lastState.IsInArray(FooterAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(FooterAction.AttackSta) && lastState.IsInArray(FooterAction.AttackSta);
	}

	public override bool IsInSideStepState()
	{
		return this.stateMachine.currentState == "MoveAway";
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Fall",
		"FallHitGround",
		"FlyToFall",
		"GetUp",
		"Hit1",
		"HitToFly",
		"HitToFly2",
		"HitFall"
	};

	private static readonly string[] DieSta = new string[]
	{
		"Die",
		"DieQTE",
		"AirDie",
		"AirDieFlyToFall",
		"AirDieFall",
		"AirDieHitGround",
		"Execute",
		"ExecuteDie",
		"Null"
	};

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	private const float AirFric = 8f;

	public enum StateEnum
	{
		Atk1,
		Atk2,
		Atk3,
		Die,
		DieQTE,
		Fall,
		FallHitGround,
		FlyToFall,
		GetUp,
		Hit1,
		HitToFly,
		HitToFly2,
		Idle,
		Jump,
		Move,
		MoveAway,
		AirDie,
		AirDieFlyToFall,
		AirDieFall,
		AirDieHitGround,
		HitFall,
		Execute,
		ExecuteDie,
		Null
	}
}
