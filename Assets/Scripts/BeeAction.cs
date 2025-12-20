using System;
using ExtensionMethods;
using GameWorld;
using UnityEngine;

public class BeeAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(BeeAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(BeeAction.StateEnum.Idle, 1f);
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject) + UnityEngine.Random.Range(3f, 4f);
		base.transform.position = position;
	}

	protected override void Update()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		this.eAttr.stiffTime = Mathf.Clamp(this.eAttr.stiffTime - Time.deltaTime, 0f, float.PositiveInfinity);
		if (this.QTE)
		{
			this.QTE.transform.localScale = ((base.transform.localScale.x >= 0f) ? Vector3.one : new Vector3(-1f, 1f, 1f));
		}
		if (!this.IsInNormalState())
		{
			this.isAutoMoveing = false;
		}
		if (this.isAutoMoveing && !SingletonMono<WorldTime>.Instance.IsFrozen)
		{
			float y = 0f;
			float num = Mathf.Abs(this.eAttr.moveSpeed * Time.deltaTime);
			if (this.eAttr.iCanFly)
			{
				y = num * Mathf.Tan(this.moveAngle * 0.0174532924f);
			}
			Vector3 position = base.transform.position += new Vector3(num * (float)this.eAttr.faceDir, y, 0f);
			float groundHeight = LayerManager.YNum.GetGroundHeight(base.gameObject);
			position.y = Mathf.Clamp(position.y, groundHeight + this.minFlyHeight, groundHeight + this.maxFlyHeight);
			base.transform.position = position;
		}
		if (this.stateMachine.currentState == "Idle")
		{
			base.FaceToPlayer();
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk1End":
		case "Atk1Ready":
		case "Atk2":
		case "Atk2End":
		case "Atk2Ready":
		case "Atk3End":
		case "Atk3Ready":
		case "Atk3_1":
		case "Atk3_2":
		case "Atk3_3":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "Hit":
		case "MoveAway":
		case "Idle2":
		case "Die":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Idle":
		case "Move":
		case "WeakMod":
			this.spineAnim.Play(args.state, true, false, 1f);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (args.nextState == "Hit")
		{
			EventManager.PostEvent<GameObject>("DestroyEffect", base.gameObject);
			base.GetComponent<BeeAnimListener>().beeLaserHalo.gameObject.SetActive(false);
		}
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			base.GetComponent<BeeAnimListener>().bone.mode = SkeletonUtilityBone.Mode.Follow;
			this.eAttr.paBody = false;
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
		base.AnimChangeState(BeeAction.StateEnum.Atk1Ready, 1f);
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
		base.AnimChangeState(BeeAction.StateEnum.Atk2Ready, 1f);
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
		base.AnimChangeState(BeeAction.StateEnum.Atk3Ready, 1f);
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
		base.ChangeFace(dir);
		base.AnimChangeState(BeeAction.StateEnum.Atk2Ready, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.FaceToPlayer();
		base.AnimChangeState(BeeAction.StateEnum.Idle, 1f);
	}

	public override void Idle2()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.FaceToPlayer();
		base.AnimChangeState(BeeAction.StateEnum.Idle2, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(BeeAction.StateEnum.Idle, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(BeeAction.StateEnum.Move, 1f);
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
		if (this.IsInWeakSta())
		{
			return;
		}
		base.SideStep();
		base.FaceToPlayer();
		base.AnimChangeState(BeeAction.StateEnum.MoveAway, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(BeeAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state == "Die";
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(BeeAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return this.stateMachine.currentState.IsInArray(BeeAction.AttackSta) && !this.stateMachine.currentState.IsInArray(BeeAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !this.stateMachine.currentState.IsInArray(BeeAction.AttackSta) && this.stateMachine.currentState.IsInArray(BeeAction.AttackSta);
	}

	public override bool IsInDefenceState()
	{
		return false;
	}

	public override bool IsInSideStepState()
	{
		return this.stateMachine.currentState == "MoveAway";
	}

	public float moveAngle;

	public float minFlyHeight;

	public float maxFlyHeight;

	public bool atk4Flag;

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Idle2",
		"Move"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk1End",
		"Atk1Ready",
		"Atk2",
		"Atk2End",
		"Atk2Ready",
		"Atk3_1",
		"Atk3_2",
		"Atk3_3",
		"Atk3End",
		"Atk3Ready",
		"Atk4_1",
		"Atk4_2",
		"Atk4End",
		"Atk4Ready"
	};

	public enum StateEnum
	{
		Atk1,
		Atk1End,
		Atk1Ready,
		Atk2,
		Atk2End,
		Atk2Ready,
		Atk3_1,
		Atk3_2,
		Atk3_3,
		Atk3End,
		Atk3Ready,
		Die,
		Hit,
		Idle,
		Idle2,
		Move,
		MoveAway,
		WeakMod
	}
}
