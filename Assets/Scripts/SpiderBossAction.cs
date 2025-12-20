using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class SpiderBossAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map8;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(SpiderBossAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(SpiderBossAction.StateEnum.Idle, 1f);
		this.summonRate = 5f;
	}

	protected override void Update()
	{
		this.summonRate += Time.deltaTime;
		base.Update();
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.lastState == "JumpForward" || args.lastState == "JumpBack")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "WeakMod")
		{
			R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (SpiderBossAction._003C_003Ef__switch_0024map8 == null)
			{
				SpiderBossAction._003C_003Ef__switch_0024map8 = new Dictionary<string, int>(15)
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
						"DieQTE",
						0
					},
					{
						"Hit1",
						0
					},
					{
						"Hit2",
						0
					},
					{
						"HitQTE",
						0
					},
					{
						"IdleToWeakMod",
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
						"WeakModToIdle",
						0
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
						"WeakMod",
						1
					}
				};
			}
			int num;
			if (SpiderBossAction._003C_003Ef__switch_0024map8.TryGetValue(state, out num))
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
		base.AnimChangeState(SpiderBossAction.StateEnum.Atk1, 1f);
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
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(SpiderBossAction.StateEnum.Atk2, 1f);
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
		base.AnimChangeState(SpiderBossAction.StateEnum.Atk3, 1f);
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
		this.summonRate = 0f;
		if (R.Enemy.Count >= 6 && R.GameData.Difficulty <= 1)
		{
			return;
		}
		if (R.Enemy.Count >= 8 && R.GameData.Difficulty > 1)
		{
			return;
		}
		base.AnimChangeState(SpiderBossAction.StateEnum.Atk4, 1f);
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
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState((!back) ? SpiderBossAction.StateEnum.JumpForward : SpiderBossAction.StateEnum.JumpBack, 1f);
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
		if (this.stateMachine.currentState.IsInArray(SpiderBossAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 2f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(SpiderBossAction.StateEnum.Atk2, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(SpiderBossAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(SpiderBossAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(SpiderBossAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(SpiderBossAction.AttackSta);
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
		return nextState.IsInArray(SpiderBossAction.AttackSta) && !lastState.IsInArray(SpiderBossAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(SpiderBossAction.AttackSta) && lastState.IsInArray(SpiderBossAction.AttackSta);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	public override void AnimQTEHurt()
	{
		base.AnimQTEHurt();
		base.ChangeFace((base.player.transform.localScale.x >= 0f) ? 1 : -1);
		base.ExitWeakState(true);
		base.AnimChangeState(SpiderBossAction.StateEnum.HitQTE, 1f);
	}

	public override void AnimExecute()
	{
		base.AnimExecute();
		base.ChangeFace((base.player.transform.localScale.x >= 0f) ? 1 : -1);
		base.ExitWeakState(true);
		base.AnimChangeState(SpiderBossAction.StateEnum.DieQTE, 1f);
	}

	public float summonRate;

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4"
	};

	public static readonly string[] JumpSta = new string[]
	{
		"JumpForward",
		"JumpBack"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit1",
		"Hit2",
		"HitQTE"
	};

	private static readonly string[] QTESta = new string[]
	{
		"HitQTE",
		"DieQTE"
	};

	public enum StateEnum
	{
		Idle,
		Move,
		Atk1,
		Atk2,
		Atk3,
		Atk4,
		DieQTE,
		Hit1,
		Hit2,
		HitQTE,
		IdleToWeakMod,
		JumpForward,
		JumpBack,
		Null,
		WeakMod,
		WeakModToIdle
	}
}
