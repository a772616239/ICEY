using System;
using DatabaseModel;
using ExtensionMethods;
using UnityEngine;

public class JumperAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(JumperAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(JumperAction.StateEnum.Ready, 1f);
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
		if (this.catchFollow)
		{
			base.player.position = new Vector3(this.catachPos.position.x, this.catachPos.position.y, base.player.position.z);
			Vector3 eulerAngles = this.catachPos.localRotation.eulerAngles;
			base.player.localRotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, (float)this.eAttr.faceDir * -eulerAngles.z));
		}
		if (this.IsInDefenceState() && Time.time - this.startDefenceTime >= this.defenceTime)
		{
			base.AnimChangeState(JumperAction.StateEnum.DefenseToIdle, 1f);
		}
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

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk2":
		case "Atk3":
		case "Atk3Fail":
		case "Atk3Success":
		case "Atk4":
		case "Atk5":
		case "Atk5_1":
		case "Atk5_2":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "Die":
		case "AirDie":
		case "AirDiePre":
		case "FlyToFall":
		case "GetUp":
		case "Hit1":
		case "Hit2":
		case "HitGround":
		case "HitToFly":
		case "Execute":
		case "ExecuteDie":
		case "Die2":
		case "Die3":
		case "DefenseToIdle":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Move":
		case "Ready":
		case "Fall":
		case "DieFall":
		case "HitToFly2":
		case "WeakMod":
		case "Defense":
		case "HitFall":
			this.spineAnim.Play(args.state, true, false, 1f);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (!args.nextState.IsInArray(JumperAction.AttackSta))
		{
			if (this.catchFollow)
			{
				this.catchResult = true;
				base.GetComponent<JumperAnimListener>().Atk3Success();
			}
			else
			{
				this.catchResult = false;
			}
		}
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
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
	}

	public void CatchSuccess()
	{
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(14);
		UnityEngine.Object.Instantiate<GameObject>(prefab, base.player.position, Quaternion.identity);
		this.catchFollow = true;
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(JumperAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(JumperAction.DieSta);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(JumperAction.StateEnum.Ready, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(JumperAction.StateEnum.Move, 1f);
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(JumperAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return !lastState.IsInArray(JumperAction.AttackSta) && nextState.IsInArray(JumperAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return lastState.IsInArray(JumperAction.AttackSta) && !nextState.IsInArray(JumperAction.AttackSta);
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
		base.AnimChangeState(JumperAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(JumperAction.StateEnum.Atk2, 1f);
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
		base.AnimChangeState(JumperAction.StateEnum.Atk3, 1f);
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
		base.AnimChangeState(JumperAction.StateEnum.Atk4, 1f);
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
		base.ChangeFace(dir);
		base.AnimChangeState(JumperAction.StateEnum.Atk5_1, 1f);
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
		base.AnimChangeState(JumperAction.StateEnum.Atk3, 1f);
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
		if (this.IsInWeakSta())
		{
			return;
		}
		if (!this.eAttr.isOnGround)
		{
			return;
		}
		base.Defence();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.startDefenceTime = Time.time;
		this.defenceTime = UnityEngine.Random.Range(2f, 4f);
		base.AnimChangeState(JumperAction.StateEnum.Defense, 1f);
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
		R.Audio.PlayEffect(406, new Vector3?(base.transform.position));
		base.AnimChangeState(JumperAction.StateEnum.Defense, 1f);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInDefenceState()
	{
		return this.stateMachine.currentState.IsInArray(JumperAction.DefenceSta);
	}

	[SerializeField]
	private Transform catachPos;

	public bool catchFollow;

	public bool catchResult;

	private static readonly string[] NormalSta = new string[]
	{
		"Atk3Fail",
		"Move",
		"Ready"
	};

	public static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk3Success",
		"Atk3Fail",
		"Atk5_1",
		"Atk5_2"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"FlyToFall",
		"GetUp",
		"Hit1",
		"Hit2",
		"Fall",
		"HitGround",
		"HitToFly",
		"HitToFly2",
		"HitFall"
	};

	private static readonly string[] DieSta = new string[]
	{
		"Die",
		"AirDiePre",
		"DieFall",
		"AirDie",
		"ExecuteDie",
		"Die2",
		"Die3"
	};

	private static readonly string[] DefenceSta = new string[]
	{
		"Defense",
		"DefenseHit"
	};

	public float airFric = 8f;

	public enum StateEnum
	{
		Atk1,
		Atk2,
		Atk3,
		Atk4,
		Atk5,
		Atk3Fail,
		Atk3Success,
		Die,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly,
		Move,
		Ready,
		Fall,
		HitToFly2,
		AirDiePre,
		DieFall,
		AirDie,
		WeakMod,
		Execute,
		ExecuteDie,
		Atk5_1,
		Atk5_2,
		Die2,
		Die3,
		Defense,
		DefenseHit,
		DefenseToIdle,
		HitFall
	}
}
