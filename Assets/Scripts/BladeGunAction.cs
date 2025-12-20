using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class BladeGunAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024mapC;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(BladeGunAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(BladeGunAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState != "FlyToFall")
		{
			return;
		}
		Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
		float f = currentSpeed.x;
		f = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
		currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
		this.eAttr.timeController.SetSpeed(currentSpeed);
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		this.gun.mode = SkeletonUtilityBone.Mode.Follow;
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
		if (args.nextState == "Atk1")
		{
			this.eAttr.paBody = true;
		}
		if (args.lastState == "Atk1")
		{
			this.eAttr.paBody = false;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (BladeGunAction._003C_003Ef__switch_0024mapC == null)
			{
				BladeGunAction._003C_003Ef__switch_0024mapC = new Dictionary<string, int>(14)
				{
					{
						"Atk1End",
						0
					},
					{
						"Atk1Ready",
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
						"FallHitGround",
						0
					},
					{
						"FlyToFall",
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
						"Warning",
						0
					},
					{
						"Fall",
						1
					},
					{
						"HitFall",
						1
					},
					{
						"HitToFly2",
						1
					},
					{
						"Atk1",
						1
					},
					{
						"Idle",
						1
					}
				};
			}
			int num;
			if (BladeGunAction._003C_003Ef__switch_0024mapC.TryGetValue(state, out num))
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
		if (this.stateMachine.currentState != "Atk1Ready" && this.stateMachine.currentState != "Atk1")
		{
			base.AnimChangeState(BladeGunAction.StateEnum.Atk1Ready, 1f);
		}
		else if (this.stateMachine.currentState == "Atk1")
		{
			base.AnimChangeState(BladeGunAction.StateEnum.Atk1, 1f);
		}
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
		base.AnimChangeState(BladeGunAction.StateEnum.Atk2, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(BladeGunAction.StateEnum.Atk1End, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(BladeGunAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState == "Atk2";
	}

	public override bool IsInDeadState(string state)
	{
		return state == "Die";
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState == "Atk2" && lastState != "Atk2";
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return nextState != "Atk2" && lastState == "Atk2";
	}

	public float airFric = 8f;

	[SerializeField]
	private SkeletonUtilityBone gun;

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Atk1Ready",
		"Atk1",
		"Atk1End",
		"Warning"
	};

	public enum StateEnum
	{
		Idle,
		Atk1Ready,
		Atk1,
		Atk1End,
		Atk2,
		Die,
		Fall,
		FallHitGround,
		FlyToFall,
		Hit1,
		HitToFly1,
		HitToFly2,
		Warning,
		HitFall
	}

	public enum HurtSta
	{
		Fall,
		FallHitGround,
		FlyToFall,
		Hit1,
		HitToFly1,
		HitToFly2,
		HitFall
	}
}
