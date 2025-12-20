using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class StickerAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map9;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(StickerAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(StickerAction.StateEnum.Idle, 1f);
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (StickerAction._003C_003Ef__switch_0024map9 == null)
			{
				StickerAction._003C_003Ef__switch_0024map9 = new Dictionary<string, int>(15)
				{
					{
						"Atk1",
						0
					},
					{
						"Atk2Ready",
						0
					},
					{
						"Atk2",
						0
					},
					{
						"Atk2End",
						0
					},
					{
						"Atk3",
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
						"IdleToWeakMod",
						0
					},
					{
						"Jump",
						0
					},
					{
						"WeakModToIdle",
						0
					},
					{
						"HitQTE",
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
			if (StickerAction._003C_003Ef__switch_0024map9.TryGetValue(state, out num))
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
		if (args.lastState == "Jump" || args.lastState == "Atk3")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "WeakMod")
		{
			R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
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
		base.AnimChangeState(StickerAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(StickerAction.StateEnum.Atk2Ready, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.ChangeFace(dir);
		base.AnimChangeState(StickerAction.StateEnum.Atk3, 1f);
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
		if (this.stateMachine.currentState.IsInArray(StickerAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		base.AnimChangeState(StickerAction.StateEnum.Atk1, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(StickerAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(StickerAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(StickerAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(StickerAction.AttackSta);
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
		return nextState.IsInArray(StickerAction.AttackSta) && !lastState.IsInArray(StickerAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(StickerAction.AttackSta) && lastState.IsInArray(StickerAction.AttackSta);
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
		base.AnimChangeState(StickerAction.StateEnum.HitQTE, 1f);
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
		base.AnimChangeState(StickerAction.StateEnum.DieQTE, 1f);
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Jump",
		"Move"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2Ready",
		"Atk2",
		"Atk2End",
		"Atk3"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit1",
		"Hit2",
		"HitQTE"
	};

	public static readonly string[] QTESta = new string[]
	{
		"DieQTE",
		"HitQTE"
	};

	public enum StateEnum
	{
		Idle,
		Jump,
		Move,
		Atk1,
		Atk2Ready,
		Atk2,
		Atk2End,
		Atk3,
		DieQTE,
		Hit1,
		Hit2,
		IdleToWeakMod,
		WeakMod,
		WeakModToIdle,
		HitQTE
	}
}
