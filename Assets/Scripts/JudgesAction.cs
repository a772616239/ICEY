using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class JudgesAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map7;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(JudgesAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		base.AnimChangeState(JudgesAction.StateEnum.Idle, 1f);
		this.attackType = JudgesAction.AttackType.Saws;
		this.summonRate = 30f;
	}

	protected override void Update()
	{
		this.summonRate += Time.deltaTime;
		if (this.IsInDefenceState() && Time.time - this.startDefenceTime >= this.defenceTime)
		{
			base.AnimChangeState(JudgesAction.StateEnum.DefenseToIdle, 1f);
		}
		base.Update();
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (JudgesAction._003C_003Ef__switch_0024map7 == null)
			{
				JudgesAction._003C_003Ef__switch_0024map7 = new Dictionary<string, int>(21)
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
						"Atk4_1",
						0
					},
					{
						"Atk4_2",
						0
					},
					{
						"Atk4_3",
						0
					},
					{
						"Atk5",
						0
					},
					{
						"Atk6",
						0
					},
					{
						"Atk7",
						0
					},
					{
						"DefenseState",
						0
					},
					{
						"DefenseToIdle",
						0
					},
					{
						"IdleToDefense",
						0
					},
					{
						"HitQTE1",
						0
					},
					{
						"HitQTE2",
						0
					},
					{
						"HitQTE3",
						0
					},
					{
						"HitQTE4",
						0
					},
					{
						"DieQTE",
						0
					},
					{
						"IdleToWeak",
						0
					},
					{
						"WeakToIdle",
						0
					},
					{
						"WeakMod",
						1
					},
					{
						"Idle",
						1
					}
				};
			}
			int num;
			if (JudgesAction._003C_003Ef__switch_0024map7.TryGetValue(state, out num))
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
		this.summonRate = 0f;
		if (this.eAttr.isDead)
		{
			return;
		}
		if ((R.Enemy.Count > 2 && R.GameData.Difficulty <= 1) || (R.Enemy.Count > 3 && R.GameData.Difficulty > 1))
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(JudgesAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(JudgesAction.StateEnum.Atk2, 1f);
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
		base.AnimChangeState(JudgesAction.StateEnum.Atk3, 1f);
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
		base.AnimChangeState(JudgesAction.StateEnum.Atk4_1, 1f);
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
		base.AnimChangeState(JudgesAction.StateEnum.Atk5, 1f);
	}

	public override void Attack6(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(JudgesAction.StateEnum.Atk6, 1f);
	}

	public override void Attack7(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(JudgesAction.StateEnum.Atk7, 1f);
	}

	public override void Defence()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInDefenceState())
		{
			return;
		}
		if (!this.eAttr.isOnGround)
		{
			return;
		}
		if (this.IsInWeakSta())
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(JudgesAction.QTEState))
		{
			return;
		}
		base.Defence();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.startDefenceTime = Time.time;
		this.defenceTime = UnityEngine.Random.Range(2f, 4f);
		base.AnimChangeState(JudgesAction.StateEnum.IdleToDefense, 1f);
	}

	public override void DefenceSuccess()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.eAttr.isOnGround)
		{
			return;
		}
		if (this.IsInWeakSta())
		{
			return;
		}
		base.AnimChangeState(JudgesAction.StateEnum.DefenseState, 1f);
	}

	public override void AnimMove()
	{
	}

	public override void AnimReady()
	{
		base.AnimChangeState(JudgesAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState == "Idle" && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(JudgesAction.AttackSta);
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
		return nextState.IsInArray(JudgesAction.AttackSta) && !lastState.IsInArray(JudgesAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(JudgesAction.AttackSta) && lastState.IsInArray(JudgesAction.AttackSta);
	}

	public override void AnimQTEHurt()
	{
		base.ExitWeakState(true);
		JudgesHurt component = base.GetComponent<JudgesHurt>();
		switch (component.currentPhase)
		{
		case 0:
			base.AnimChangeState(JudgesAction.StateEnum.HitQTE1, 1f);
			break;
		case 1:
			base.AnimChangeState(JudgesAction.StateEnum.HitQTE2, 1f);
			break;
		case 2:
			base.AnimChangeState(JudgesAction.StateEnum.HitQTE3, 1f);
			break;
		case 3:
			this.attackType = ((UnityEngine.Random.Range(0, 2) != 0) ? JudgesAction.AttackType.Laser : JudgesAction.AttackType.Saws);
			base.AnimChangeState(JudgesAction.StateEnum.HitQTE4, 1f);
			break;
		}
	}

	public override void AnimExecute()
	{
		base.ExitWeakState(true);
		base.AnimChangeState(JudgesAction.StateEnum.DieQTE, 1f);
	}

	public float summonRate;

	public JudgesAction.AttackType attackType;

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4_1",
		"Atk4_2",
		"Atk4_3",
		"Atk5",
		"Atk6",
		"Atk7"
	};

	public static readonly string[] QTEState = new string[]
	{
		"HitQTE1",
		"HitQTE2",
		"HitQTE3",
		"HitQTE4",
		"DieQTE"
	};

	public enum AttackType
	{
		Saws,
		Laser,
		HitGround
	}

	public enum StateEnum
	{
		Idle,
		Atk1,
		Atk2,
		Atk3,
		Atk4_1,
		Atk4_2,
		Atk4_3,
		Atk5,
		Atk6,
		Atk7,
		IdleToDefense,
		DefenseState,
		DefenseToIdle,
		WeakMod,
		HitQTE1,
		HitQTE2,
		HitQTE3,
		HitQTE4,
		DieQTE,
		IdleToWeak,
		WeakToIdle
	}
}
