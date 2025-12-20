using System;
using System.Diagnostics;
using Core;
using ExtensionMethods;
using GameWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAction : BaseBehaviour
{
	public Transform executeFollow
	{
		get
		{
			return (!this.spATK.enabled) ? this.sword : this.leftHand;
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnPlayerTurnRound;

	public bool NotAllowPassSceneGate
	{
		get
		{
			return this.stateMachine.currentState.IsInArray(PlayerAction.HurtSta) || this.stateMachine.currentState.IsInArray(PlayerAction.ExecuteSta);
		}
	}

	public bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(PlayerAction.NormalSta);
	}

	private void Awake()
	{
		this.stateMachine = base.GetComponent<StateMachine>();
		this.pAttr = R.Player.Attribute;
		this.pab = base.GetComponent<PlayerAbilities>();
		this.weapon = base.GetComponent<Claymore>();
		this.msac = base.GetComponent<MultiSpineAnimationController>();
		this.listener = base.GetComponent<PlayerAnimEventListener>();
		this.StateInit();
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void Start()
	{
		PlayerAction.main = this;
		this.ChangeState(PlayerAction.StateEnum.Idle, 1f);
		this.AttributeInit();
		this.ShadowInit();
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	private void StateInit()
	{
		this.stateMachine.AddStates(typeof(PlayerAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
	}

	private void AttributeInit()
	{
		this.pAttr.ResetData();
		this.TurnRound(1);
	}

	private void ShadowInit()
	{
		this.shadow = UnityEngine.Object.Instantiate<Transform>(this.shadowPrefab);
		this.shadow.GetComponent<ShadowControl>().SetTarget(base.transform);
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
	{
		this.shadow = UnityEngine.Object.Instantiate<Transform>(this.shadowPrefab);
		this.shadow.GetComponent<ShadowControl>().SetTarget(base.transform);
		this.shadow.gameObject.SetActive(true);
		if (this.stateMachine.currentState.IsInArray(PlayerAction.HurtSta))
		{
			this.ChangeState((!this.pAttr.isOnGround) ? PlayerAction.StateEnum.Fall1 : PlayerAction.StateEnum.Idle, 1f);
		}
	}

	private void Update()
	{
		if (this.pAttr.isDead)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.NormalSta) && !this.pAttr.isOnGround)
		{
			this.ChangeState(PlayerAction.StateEnum.Fall1, 1f);
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		this.canChangeAnim = false;
		this.listener.BoxSizeRecover();
		if (args.nextState.IsInArray(PlayerAction.HurtSta))
		{
			this.weapon.AirAttackReset();
			this.listener.StopIEnumerator("FlashPositionSet");
			this.listener.isFalling = false;
			this.listener.airAtkDown = false;
			this.listener.checkFallDown = false;
		}
		if (!args.lastState.IsInArray(PlayerAction.NormalSta) && args.nextState.IsInArray(PlayerAction.NormalSta))
		{
			this.listener.PhysicReset();
			this.weapon.AirAttackReset();
			if (!base.GetComponent<Collider2D>().enabled)
			{
				base.GetComponent<Collider2D>().enabled = true;
			}
		}
		if (args.nextState == "UnderAtkHitToFly")
		{
			this.listener.hitJump = false;
			base.StartCoroutine(this.listener.HitJumpBack());
		}
		if (this.pab.hurt.DeadFlag)
		{
			base.GetComponent<ChangeSpineColor>().TurnOffAll();
		}
		else if (args.nextState.IsInArray(PlayerAction.NormalSta) || args.nextState.IsInArray(PlayerAction.JumpSta) || args.nextState.IsInArray(PlayerAction.FlySta))
		{
			base.GetComponent<ChangeSpineColor>().TurnOnBreatheLight();
		}
		else
		{
			base.GetComponent<ChangeSpineColor>().TurnOnEmission();
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		if (this.pab.hurt.DeadFlag && !args.state.IsInArray(PlayerAction.DieSta))
		{
			return;
		}
		string state = args.state;
		switch (state)
		{
		case "AirAtk1":
		case "AirAtk2":
		case "AirAtk3":
		case "AirAtk4":
		case "Atk1":
		case "Atk2":
		case "Atk3":
		case "Atk4":
		case "Atk5":
		case "Atk7":
		case "Atk8":
		case "Charge1Ready":
		case "Charge1End":
		case "Atk11":
		case "Atk12":
		case "Atk13":
		case "Atk14":
		case "Atk23":
		case "Atk15":
		case "AirAtk6":
		case "AirAtk7":
		case "RiderQTEHurt_3":
		case "AirAtkHv1":
		case "AirAtkHv2":
		case "AirAtkHv3":
		case "AirAtkHv5":
		case "AtkHv1":
		case "AtkHv2":
		case "AtkHv3":
		case "AtkUpRising":
		case "Atk16":
		case "AirAtkHv1Push":
		case "AtkHv1Push":
		case "NewExecute1_2":
		case "RiderQTEHurt_2":
		case "NewExecute2_1":
		case "NewExecute2_2":
		case "NewExecuteAir1_1":
		case "NewExecuteAir2_2":
		case "FlashAttack":
		case "AirFlashAttack":
		case "DahalAtkUpRising":
		case "BeelzebubQTEDie":
		case "QTEPush":
		case "AirChargeEnd":
		case "AirQTEPush":
		case "AtkHv4":
		case "QTECharge1Ready":
		case "QTECharge1End":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Attack, false, true, this.animSpeed);
			break;
		case "Charging1":
		case "QTECharging1":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Attack, true, false, this.animSpeed);
			break;
		case "AirShootReady":
		case "Flash2":
		case "RollJump":
		case "HitGround":
		case "HitGround2":
		case "ShootReady":
		case "Execute":
		case "ExecuteToIdle":
		case "Execute2":
		case "Execute2ToFall":
		case "Flash1":
		case "AirCallSkill":
		case "CallSkill":
		case "CallSkillAir":
		case "Shoot":
		case "AirShoot":
		case "FlashDown1":
		case "FlashDown2":
		case "FlashUp1":
		case "FlashUp2":
		case "RollReady":
		case "BladeStormReady":
		case "RollEnd":
		case "RollGround":
		case "NewExecute1_1":
		case "RiderQTEHurt_1":
		case "BeelzebubQTECatch":
		case "NewExecute2_0":
		case "NewExecuteAir1_2":
		case "NewExecuteAir2_1":
		case "AirAtkRoll":
		case "AirAtkRollReady":
		case "AtkRollEnd":
		case "AtkRollReady":
		case "FlashDown45_1":
		case "FlashDown45_2":
		case "FlashUp45_1":
		case "FlashUp45_2":
		case "RollEndFrame":
		case "RollFrameEnd":
		case "AtkFlashRollEnd":
		case "QTEHitGround":
		case "QTEHitGround2":
		case "QTERollEnd":
			this.msac.Play(args.state, PlayerAction.SkeletonType.SpAttack, false, true, this.animSpeed);
			break;
		case "Roll":
		case "BladeStorm":
		case "DahalRoll":
		case "QTERoll":
			this.msac.Play(args.state, PlayerAction.SkeletonType.SpAttack, true, false, this.animSpeed);
			break;
		case "EndAtk":
		case "Fall1":
		case "GetUp":
		case "Jump":
		case "Jump2":
		case "RunSlow":
		case "FlashGround":
		case "IdleToDefense":
		case "FallToDefenseAir":
		case "QTEEndAtk":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Normal, false, true, this.animSpeed);
			break;
		case "Fall2":
		case "Idle":
		case "Ready":
		case "Run":
		case "Defense":
		case "DefenseAir":
		case "QTEReady":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Normal, true, false, this.animSpeed);
			break;
		case "UnderAtk1":
		case "UnderAtkHitGround":
		case "UnderAtkHitToFly":
		case "UnderAtkGetUp":
		case "UnderAtkFlyToFall":
		case "UnderAtkBombKillerII":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Hurt, false, true, this.animSpeed);
			break;
		case "UnderAtkJumper":
		case "UnderAtkEat":
		case "UnderAtkHitSaw":
		case "Disappear":
			this.msac.Play(args.state, PlayerAction.SkeletonType.Hurt, true, false, this.animSpeed);
			break;
		case "UpRising":
			this.msac.Play(args.state, PlayerAction.SkeletonType.UpRising, false, true, this.animSpeed);
			break;
		case "Atk6":
		case "AirCombo":
		case "AirComboFlash":
			this.msac.Play(args.state, PlayerAction.SkeletonType.HeavyAttack, false, true, this.animSpeed);
			break;
		case "DoubleFlash":
		case "DoubleFlashAir":
			this.msac.Play(args.state, PlayerAction.SkeletonType.HeavyAttack, false, true, 1.5f);
			break;
		case "AirCharging":
			this.msac.Play(args.state, PlayerAction.SkeletonType.HeavyAttack, true, false, this.animSpeed);
			break;
		}
	}

	public void TurnRound(int dir)
	{
		Vector3 localScale = base.transform.localScale;
		if (this.OnPlayerTurnRound != null)
		{
			this.OnPlayerTurnRound(this, null);
		}
		if (dir == -1)
		{
			this.pAttr.faceDir = dir;
			localScale.x = 1f * Mathf.Abs(localScale.x);
			base.transform.localScale = localScale;
		}
		else if (dir == 1)
		{
			this.pAttr.faceDir = dir;
			localScale.x = -1f * Mathf.Abs(localScale.x);
			base.transform.localScale = localScale;
		}
	}

	public void ChangeState(PlayerAction.StateEnum sta, float speed = 1f)
	{
		this.animSpeed = speed;
		this.stateMachine.SetState(PlayerAction.State[(int)sta]);
	}

	public void ChangeState(string sta, float speed = 1f)
	{
		this.animSpeed = speed;
		this.stateMachine.SetState(sta);
	}

	public void FlashReset()
	{
	}

	public void AbsorbEnergyBall()
	{
		R.Audio.PlayEffect(43, new Vector3?(base.transform.position));
		if (this.pAttr.isInCharging && R.Player.Enhancement.Charging >= 1)
		{
			this.absorbNum++;
			this.weapon.AddChargeLevel();
		}
	}

	public static void Reborn()
	{
		EventManager.PostEvent<PlayerAction, AssessmentEventArgs>("Assessment", R.Player.Action, new AssessmentEventArgs(AssessmentEventArgs.EventType.ContinueGame));
		PlayerAttribute attribute = R.Player.Attribute;
		attribute.AllAttributeRecovery();
		R.Player.Action.pab.hurt.DeadFlag = false;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.Idle, 1f);
	}

	public static void Reset()
	{
		PlayerAttribute attribute = R.Player.Attribute;
		attribute.ResetData();
		R.Player.Action.pab.hurt.DeadFlag = false;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.Idle, 1f);
	}

	public void HPRecover(int recoverNum)
	{
		this.pAttr.currentHP += recoverNum;
	}

	private bool OnEnhancementLevelUp(string eventDefine, object sender, EnhanceArgs msg)
	{
		string name = msg.Name;
		if (name != null)
		{
			if (name == "maxHP")
			{
				this.pAttr.maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(msg.UpToLevel);
				this.pAttr.currentHP = this.pAttr.maxHP;
			}
		}
		return true;
	}

	public void QTEHPRecover(bool fullRecover = false)
	{
		Core.Input.Vibration.Vibrate(8);
		float num = 0f;
		int recover = R.Player.Enhancement.Recover;
		if (recover != 1)
		{
			if (recover != 2)
			{
				if (recover == 3)
				{
					num = 0.3f;
				}
			}
			else
			{
				num = 0.25f;
			}
		}
		else
		{
			num = 0.2f;
		}
		if (fullRecover)
		{
			this.pAttr.currentHP = this.pAttr.maxHP;
		}
		else
		{
			this.pAttr.currentHP += (int)((float)this.pAttr.maxHP * num);
		}
		this.pAttr.currentEnergy = this.pAttr.maxEnergy;
	}

	private const int LEFT = -1;

	private const int RIGHT = 1;

	private const int STOP = 0;

	private const int CURRENT = 3;

	public bool canChangeAnim;

	public StateMachine stateMachine;

	[SerializeField]
	private Transform leftHand;

	[SerializeField]
	private Transform sword;

	[SerializeField]
	private MeshRenderer spATK;

	public static PlayerAction main;

	public int tempDir;

	public Transform shadow;

	private MultiSpineAnimationController msac;

	private PlayerAnimEventListener listener;

	private PlayerAttribute pAttr;

	public PlayerAbilities pab;

	private Claymore weapon;

	[SerializeField]
	public ParticleSystem blockPartical;

	[SerializeField]
	private Transform shadowPrefab;

	public Transform hurtBox;

	private float animSpeed = 1f;

	public int absorbNum;

	private static readonly string[] State = new string[]
	{
		"EndAtk",
		"Fall1",
		"Fall2",
		"GetUp",
		"Idle",
		"Jump",
		"Jump2",
		"Ready",
		"Run",
		"RunSlow",
		"AirAtk1",
		"AirAtk2",
		"AirAtk3",
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"AirShoot",
		"AirShootReady",
		"Flash1",
		"Flash2",
		"HitGround",
		"HitGround2",
		"Shoot",
		"ShootReady",
		"UnderAtk1",
		"UnderAtkFlyToFall",
		"UnderAtkHitGround",
		"UnderAtkHitToFly",
		"Atk6",
		"Atk7",
		"Atk8",
		"UpRising",
		"UnderAtkGetUp",
		"Execute",
		"AirAtk4",
		"Charge1Ready",
		"Charging1",
		"Charge1End",
		"UnderAtkJumper",
		"UnderAtkEat",
		"RollJump",
		"UnderAtkHitSaw",
		"AirCallSkill",
		"CallSkill",
		"CallSkillAir",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk23",
		"Atk15",
		"FlashDown1",
		"FlashDown2",
		"FlashUp1",
		"FlashUp2",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
		"AirAtk6",
		"AirAtkHv1",
		"AirAtkHv2",
		"AirAtkHv3",
		"AirAtkHv4",
		"AirAtkHv5",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"AtkUpRising",
		"Atk16",
		"AtkHv1Push",
		"AirAtkHv1Push",
		"Execute2",
		"RollReady",
		"Roll",
		"RollEnd",
		"RollGround",
		"ExecuteToIdle",
		"Execute2ToFall",
		"FlashGround",
		"NewExecute1_1",
		"NewExecute1_2",
		"NewExecute2_1",
		"NewExecute2_2",
		"NewExecuteAir1_1",
		"NewExecuteAir1_2",
		"NewExecuteAir2_1",
		"NewExecuteAir2_2",
		"AtkRollReady",
		"AtkRollEnd",
		"AirAtkRollReady",
		"AirAtkRoll",
		"FlashAttack",
		"AirComboFlash",
		"AirCombo",
		"AirFlashAttack",
		"NewExecute2_0",
		"AirAtk7",
		"Disappear",
		"FlashDown45_1",
		"FlashDown45_2",
		"FlashUp45_1",
		"FlashUp45_2",
		"DahalAtkUpRising",
		"DahalRoll",
		"BeelzebubQTECatch",
		"BeelzebubQTEDie",
		"QTEPush",
		"RiderQTEHurt_1",
		"RiderQTEHurt_2",
		"RiderQTEHurt_3",
		"BladeStormReady",
		"BladeStorm",
		"AirCharging",
		"AirChargeEnd",
		"DoubleFlash",
		"DoubleFlashAir",
		"RollEndFrame",
		"RollFrameEnd",
		"AtkFlashRollEnd",
		"UnderAtkBombKillerII",
		"QTEHitGround",
		"QTEHitGround2",
		"QTERoll",
		"QTERollEnd",
		"AirQTEPush",
		"AtkHv4",
		"QTECharge1Ready",
		"QTECharging1",
		"QTECharge1End",
		"QTEEndAtk",
		"QTEReady"
	};

	public static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk6",
		"Atk7",
		"Atk8",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk23",
		"Atk15",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"Atk16",
		"AtkHv1Push",
		"AtkRollReady",
		"AtkRollEnd",
		"DoubleFlash"
	};

	public static readonly string[] FlySta = new string[]
	{
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2"
	};

	public static readonly string[] UpRisingSta = new string[]
	{
		"UpRising",
		"AtkUpRising"
	};

	public static readonly string[] JumpSta = new string[]
	{
		"Jump",
		"Jump2",
		"RollJump"
	};

	public static readonly string[] AirAttackSta = new string[]
	{
		"AirAtk1",
		"AirAtk2",
		"AirAtk3",
		"AirAtk4",
		"AirAtk6",
		"AirAtkHv1",
		"AirAtkHv2",
		"AirAtkHv3",
		"AirAtkHv4",
		"AirAtkHv5",
		"AirAtkHv1Push",
		"AirAtkRollReady",
		"AirAtkRoll",
		"DoubleFlashAir"
	};

	public static readonly string[] AirLightAttackSta = new string[]
	{
		"AirAtk1",
		"AirAtk2",
		"AirAtk6"
	};

	public static readonly string[] NormalSta = new string[]
	{
		"EndAtk",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow"
	};

	public static readonly string[] HitGroundSta = new string[]
	{
		"HitGround",
		"HitGround2",
		"RollReady",
		"Roll",
		"RollEnd",
		"RollEndFrame",
		"RollFrameEnd"
	};

	public static readonly string[] FlashAttackSta = new string[]
	{
		"Flash1",
		"FlashDown1",
		"FlashUp1",
		"FlashGround",
		"FlashDown45_1",
		"FlashDown45_2",
		"FlashUp45_1",
		"FlashUp45_2"
	};

	public static readonly string[] ChargeSta = new string[]
	{
		"Charge1Ready",
		"Charging1",
		"Charge1End",
		"AirCharging",
		"AirChargeEnd"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"UnderAtk1",
		"UnderAtkFlyToFall",
		"UnderAtkHitGround",
		"UnderAtkHitToFly",
		"UnderAtkGetUp",
		"UnderAtkJumper",
		"UnderAtkEat",
		"UnderAtkHitSaw",
		"UnderAtkBombKillerII"
	};

	public static readonly string[] SpHurtSta = new string[]
	{
		"UnderAtkJumper",
		"UnderAtkEat",
		"UnderAtkHitSaw",
		"UnderAtkBombKillerII"
	};

	public static readonly string[] DieSta = new string[]
	{
		"UnderAtkFlyToFall",
		"UnderAtkHitGround",
		"UnderAtkHitToFly"
	};

	public static readonly string[] ExecuteSta = new string[]
	{
		"Execute",
		"Execute2",
		"NewExecute1_1",
		"NewExecute1_2",
		"NewExecute2_1",
		"NewExecute2_2",
		"NewExecuteAir1_1",
		"NewExecuteAir1_2",
		"NewExecuteAir2_1",
		"NewExecuteAir2_2",
		"NewExecute2_0",
		"DahalAtkUpRising",
		"DahalRoll",
		"BeelzebubQTECatch",
		"BeelzebubQTEDie",
		"QTEPush",
		"RiderQTEHurt_1",
		"RiderQTEHurt_2",
		"RiderQTEHurt_3",
		"QTEHitGround",
		"QTEHitGround2",
		"QTERoll",
		"QTERollEnd",
		"AirQTEPush",
		"AtkHv4",
		"QTECharge1Ready",
		"QTECharging1",
		"QTECharge1End",
		"QTEEndAtk",
		"QTEReady"
	};

	public static readonly string CanRunSlow = "Run";

	public enum SkeletonType
	{
		Normal,
		Attack,
		SpAttack,
		Hurt,
		UpRising,
		HeavyAttack
	}

	public enum StateEnum
	{
		EndAtk,
		Fall1,
		Fall2,
		GetUp,
		Idle,
		Jump,
		Jump2,
		Ready,
		Run,
		RunSlow,
		AirAtk1,
		AirAtk2,
		AirAtk3,
		Atk1,
		Atk2,
		Atk3,
		Atk4,
		Atk5,
		AirShoot,
		AirShootReady,
		Flash1,
		Flash2,
		HitGround,
		HitGround2,
		Shoot,
		ShootReady,
		UnderAtk1,
		UnderAtkFlyToFall,
		UnderAtkHitGround,
		UnderAtkHitToFly,
		Atk6,
		Atk7,
		Atk8,
		UpRising,
		UnderAtkGetUp,
		Execute,
		AirAtk4,
		Charge1Ready,
		Charging1,
		Charge1End,
		UnderAtkJumper,
		UnderAtkEat,
		RollJump,
		UnderAtkHitSaw,
		AirCallSkill,
		CallSkill,
		CallSkillAir,
		Atk11,
		Atk12,
		Atk13,
		Atk14,
		Atk23,
		Atk15,
		FlashDown1,
		FlashDown2,
		FlashUp1,
		FlashUp2,
		IdleToDefense,
		Defense,
		FallToDefenseAir,
		DefenseAir,
		AirAtk6,
		AirAtkHv1,
		AirAtkHv2,
		AirAtkHv3,
		AirAtkHv4,
		AirAtkHv5,
		AtkHv1,
		AtkHv2,
		AtkHv3,
		AtkUpRising,
		Atk16,
		AtkHv1Push,
		AirAtkHv1Push,
		Execute2,
		RollReady,
		Roll,
		RollEnd,
		RollGround,
		ExecuteToIdle,
		Execute2ToFall,
		FlashGround,
		NewExecute1_1,
		NewExecute1_2,
		NewExecute2_1,
		NewExecute2_2,
		NewExecuteAir1_1,
		NewExecuteAir1_2,
		NewExecuteAir2_1,
		NewExecuteAir2_2,
		AtkRollReady,
		AtkRollEnd,
		AirAtkRollReady,
		AirAtkRoll,
		FlashAttack,
		AirComboFlash,
		AirCombo,
		AirFlashAttack,
		NewExecute2_0,
		AirAtk7,
		Disappear,
		FlashDown45_1,
		FlashDown45_2,
		FlashUp45_1,
		FlashUp45_2,
		DahalAtkUpRising,
		DahalRoll,
		BeelzebubQTECatch,
		BeelzebubQTEDie,
		QTEPush,
		RiderQTEHurt_1,
		RiderQTEHurt_2,
		RiderQTEHurt_3,
		BladeStormReady,
		BladeStorm,
		AirCharging,
		AirChargeEnd,
		DoubleFlash,
		DoubleFlashAir,
		RollEndFrame,
		RollFrameEnd,
		AtkFlashRollEnd,
		UnderAtkBombKillerII,
		QTEHitGround,
		QTEHitGround2,
		QTERoll,
		QTERollEnd,
		AirQTEPush,
		AtkHv4,
		QTECharge1Ready,
		QTECharging1,
		QTECharge1End,
		QTEEndAtk,
		QTEReady
	}
}
