using System;

public class PlayerAbilities : BaseBehaviour
{
	private void Awake()
	{
		this.stateMachine = R.Player.StateMachine;
		this.stateMachine.OnEnter += this.OnStateMachineStateEnter;
		this.stateMachine.OnExit += this.OnStateMachineStateExit;
		this.stateMachine.OnTransfer += this.OnStateMachineStateTransfer;
		this.states = new CharacterState[13];
		this.states[0] = this.move;
		this.states[1] = this.attack;
		this.states[2] = this.jump;
		this.states[3] = this.charge;
		this.states[4] = this.execute;
		this.states[5] = this.hitGround;
		this.states[6] = this.upRising;
		this.states[7] = this.jumpDown;
		this.states[8] = this.flash;
		this.states[9] = this.skill;
		this.states[10] = this.flashAttack;
		this.states[11] = this.hurt;
		this.states[12] = this.chase;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].Init();
		}
	}

	private void Start()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].Start();
		}
	}

	private void Update()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].Update();
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnEnable();
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnDisable();
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnDestroy();
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].FixedUpdate();
		}
	}

	public virtual void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnStateMachineStateTransfer(sender, args);
		}
	}

	public virtual void OnStateMachineStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnStateMachineStateEnter(sender, args);
		}
	}

	public virtual void OnStateMachineStateExit(object sender, StateMachine.StateEventArgs args)
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].OnStateMachineStateExit(sender, args);
		}
	}

	public PlayerMoveAbility move = new PlayerMoveAbility();

	public PlayerAttackAbility attack = new PlayerAttackAbility();

	public PlayerJumpAbility jump = new PlayerJumpAbility();

	public PlayerChargingAbility charge = new PlayerChargingAbility();

	public PlayerExecuteAbility execute = new PlayerExecuteAbility();

	public PlayerHitGroundAbility hitGround = new PlayerHitGroundAbility();

	public PlayerUpRisingAbility upRising = new PlayerUpRisingAbility();

	public PlayerJumpDownAbility jumpDown = new PlayerJumpDownAbility();

	public PlayerFlashAbility flash = new PlayerFlashAbility();

	public PlayerSkillAbility skill = new PlayerSkillAbility();

	public PlayerFlashAttackAbility flashAttack = new PlayerFlashAttackAbility();

	public PlayerHurtAbility hurt = new PlayerHurtAbility();

	public PlayerChaseAbility chase = new PlayerChaseAbility();

	protected StateMachine stateMachine;

	private CharacterState[] states;
}
