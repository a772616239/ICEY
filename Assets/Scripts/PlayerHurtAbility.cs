using System;
using System.Collections.Generic;
using Core;
using ExtensionMethods;
using GameWorld;
using LitJson;
using UnityEngine;

public class PlayerHurtAbility : CharacterState
{
	private float DamageCutOff
	{
		get
		{
			switch (R.Player.Enhancement.MaxEnergy)
			{
			case 1:
				return 0.7f;
			case 2:
				return 0.6f;
			case 3:
				return 0.5f;
			default:
				return 1f;
			}
		}
	}

	private float ShieldRecoverTime
	{
		get
		{
			return (R.GameData.Difficulty > 1) ? 10f : 5f;
		}
	}

	public override void Start()
	{
		this._spHurt = new PlayerSpHurt();
	}

	public override void OnEnable()
	{
		EventManager.RegisterEvent<PlayerHurtAtkEventArgs>("PlayerHurtAtk", new EventManager.FBEventHandler<PlayerHurtAtkEventArgs>(this.PlayeAllHurt), EventManager.ListenerQueue.Game);
	}

	public override void OnDisable()
	{
		EventManager.UnregisterEvent<PlayerHurtAtkEventArgs>("PlayerHurtAtk", new EventManager.FBEventHandler<PlayerHurtAtkEventArgs>(this.PlayeAllHurt), EventManager.ListenerQueue.Game);
	}

	public override void Update()
	{
		this.UpdateHurtId();
		this.UpdatePlayerDeath();
		this.UpdateShield();
		this.UpdateHurtGetUp();
		if (this._hurtLimit > 0f)
		{
			this._hurtLimit = Mathf.Clamp(this._hurtLimit - Time.deltaTime, 0f, float.MaxValue);
		}
	}

	private void UpdateHurtId()
	{
		this._clearRate -= 1f;
		if (this._clearRate <= 0f)
		{
			this._clearRate = 600f;
			this.hurtId.Clear();
		}
	}

	private void UpdatePlayerDeath()
	{
		if (this.pAttr.isDead && !this.DeadFlag)
		{
			this.PlayerDie();
		}
	}

	private void UpdateShield()
	{
		if (this.pAttr.currentEnergy == 0)
		{
			this._energyRecover += Time.deltaTime;
			if (this._energyRecover >= this.ShieldRecoverTime)
			{
				this.pAttr.currentEnergy = this.pAttr.maxEnergy;
				this._energyRecover = 0f;
			}
		}
	}

	private bool PlayeAllHurt(string eventName, object sender, PlayerHurtAtkEventArgs args)
	{
		if (this.pAttr.isDead)
		{
			return false;
		}
		if (this.pAttr.flashFlag && R.Player.Enhancement.FlashAttack != 0)
		{
			this.hurtId.Add(args.atkId);
			this.pab.flashAttack.FlashAttack(args.origin);
			EventManager.PostEvent<PlayerHurtAbility, AssessmentEventArgs>("Assessment", this, new AssessmentEventArgs(AssessmentEventArgs.EventType.AddFlashTime));
			return true;
		}
		if (this.Invincible)
		{
			return false;
		}
		this.HurtFeedback(args.sender.transform, args.damage, args.atkId, args.data, args.forceHurt);
		return true;
	}

	private void HurtFeedback(Transform enemy, int damage, int atkId, JsonData atkData, bool forceHurt)
	{
		if (this.HurtFilter(atkId))
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.FlashAttackSta))
		{
			EventManager.PostEvent<PlayerHurtAbility, AssessmentEventArgs>("Assessment", this, new AssessmentEventArgs(AssessmentEventArgs.EventType.AddFlashTime));
			return;
		}
		if (atkData == null)
		{
			UnityEngine.Debug.Log(enemy.name + "  " + enemy.GetComponent<EnemyBaseAction>().stateMachine.currentState);
			return;
		}
		this._broken = false;
		this.hurtId.Add(atkId);
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(6, WorldTime.FrozenArgs.FrozenType.Player, true);
		SingletonMono<CameraController>.Instance.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		damage = (int)((float)damage * atkData.Get<float>("damagePercent", 1f));
		this._energyRecover = 0f;
		int dir = this.JudgeDir(enemy);
		if (this.pAttr.currentEnergy > 0 && !this.pac.stateMachine.currentState.IsInArray(PlayerAction.SpHurtSta) && this.HurtInDefense(enemy, atkData.Get<int>("shieldDamage", 3), ref damage))
		{
			return;
		}
		if (this._spHurt.DaoRoll(enemy))
		{
			if (enemy.GetComponent<DaoAction>() != null)
			{
				enemy.GetComponent<DaoAction>().RollToIdle();
			}
			if (enemy.GetComponent<DaoPaoAction>() != null)
			{
				enemy.GetComponent<DaoPaoAction>().RollToIdle();
			}
		}
		if (this._spHurt.CanBeJumperCatach(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkJumper");
			enemy.GetComponent<JumperAction>().CatchSuccess();
			return;
		}
		if (this._spHurt.CanBeJumperFooterCatch(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkJumper");
			enemy.GetComponent<JumperFooterAction>().Attack2Success();
			return;
		}
		if (this._spHurt.CanBeBeelzebubEat(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkEat");
			enemy.GetComponent<BeelzebubAction>().EatAttack();
			return;
		}
		if (this._spHurt.CanEatingBossEat(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkEat");
			enemy.GetComponent<EatingBossAction>().Attack2Success();
			return;
		}
		if (this._spHurt.CanBeBeelzebubSaw(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkHitSaw");
			enemy.GetComponent<BeelzebubAction>().SawAttack();
			return;
		}
		if (this._spHurt.CanBombKillerCatch(enemy))
		{
			this.HurtStiffInit(dir);
			this.HurtState("UnderAtkBombKillerII");
			enemy.GetComponent<BombKillerAction>().Atk1Success();
			return;
		}
		if (this._spHurt.JumperCatachSuccess(enemy, forceHurt) || this._spHurt.BombKillerAtkSuccess(enemy, forceHurt) || this._spHurt.JumperFooterCatchSuccess(enemy, forceHurt) || this._spHurt.BeelzebubSpAttackSuccess(enemy, forceHurt) || this._spHurt.EatingBossEatSuccess(enemy, forceHurt))
		{
			this.HurtStiff(dir, atkData, enemy);
		}
		this.HpMinus(damage);
		if (this.pAttr.isDead)
		{
			return;
		}
		if (this.pac.stateMachine.currentState.IsInArray(PlayerAction.SpHurtSta))
		{
			return;
		}
		this.HurtStiff(dir, atkData, enemy);
	}

	private void HurtStiff(int dir, JsonData atkData, Transform enemy)
	{
		R.Player.Rigidbody2D.gravityScale = 1f;
		float num = 0f;
		float y = 0f;
		float num2 = 3f;
		float y2 = 5f;
		string text = "UnderAtk1";
		string text2 = "UnderAtkHitToFly";
		if (atkData != null)
		{
			num = (float)atkData.Get<int>("xSpeed", 0);
			y = (float)atkData.Get<int>("ySpeed", 0);
			num2 = (float)atkData.Get<int>("airXSpeed", 3);
			y2 = (float)atkData.Get<int>("airYSpeed", 5);
			text = atkData.Get<string>("animName", "UnderAtk1");
		}
		if (this._broken || (this._hurtTimes >= 2 && this._hurtLimit > 0f))
		{
			text = text2;
			num = 3f;
			y = 20f;
			num2 = 3f;
			y2 = 15f;
		}
		R.Effect.Generate(216, null, default(Vector3), default(Vector3), default(Vector3), true);
		this.HurtStiffInit(dir);
		Vector2 vector = new Vector2(num * this.pac.transform.localScale.x, y);
		Vector2 vector2 = new Vector2(num2 * this.pac.transform.localScale.x, y2);
		R.Player.TimeController.SetSpeed((!this.pAttr.isOnGround) ? vector2 : vector);
		Core.Input.Vibration.Vibrate(1);
		this.HurtState((!this.pAttr.isOnGround) ? text2 : text);
		this.HitEffect(dir, enemy);
	}

	private void PlayerDie()
	{
		this.DeadFlag = true;
		R.Player.Rigidbody2D.gravityScale = 1f;
		R.Camera.Controller.CameraBloom(0.5f, 1f);
		Vector2 speed = new Vector2((float)(this.pAttr.faceDir * -7), 15f);
		R.Player.TimeController.SetSpeed(speed);
		this.pac.ChangeState(PlayerAction.StateEnum.UnderAtkHitToFly, 1f);
		base.StartCoroutine(this.listener.PlayerDieEnumerator());
	}

	private void ShadowControl(bool show)
	{
		if (!this.pac.shadow)
		{
			return;
		}
		if (this.pac.shadow.gameObject.activeSelf != show)
		{
			this.pac.shadow.gameObject.SetActive(show);
		}
	}

	private bool HurtFilter(int atkId)
	{
		return this.hurtId.Contains(atkId) || this.pac.stateMachine.currentState.IsInArray(PlayerAction.ExecuteSta);
	}

	private void HurtStiffInit(int dir)
	{
		this.listener.checkFallDown = false;
		this.listener.isFalling = false;
		this.listener.airAtkDown = false;
		this.listener.checkHitGround = false;
		this.listener.flyHitFlag = false;
		this.listener.flyHitGround = false;
		this.listener.StopIEnumerator("FlashPositionSet");
		this.pac.TurnRound(dir);
		this.pac.pab.charge.CancelCharge();
	}

	private int JudgeDir(Transform enemy)
	{
		if (enemy == null)
		{
			return this.pAttr.faceDir;
		}
		EnemyAttribute component = enemy.GetComponent<EnemyAttribute>();
		if (component != null)
		{
			return component.faceDir * -1;
		}
		Rigidbody2D component2 = enemy.GetComponent<Rigidbody2D>();
		if (component2 != null)
		{
			return (component2.velocity.x >= 0f) ? -1 : 1;
		}
		return (enemy.position.x - this.pac.transform.position.x <= 0f) ? -1 : 1;
	}

	private void HpMinus(int damage)
	{
		if (damage == 0)
		{
			return;
		}
		this.pAttr.currentHP -= damage;
	}

	private bool HurtInDefense(Transform enemy, int shieldDamage, ref int damage)
	{
		shieldDamage *= ((R.GameData.Difficulty > 1) ? 2 : 1);
		if (R.GameData.Difficulty == 3)
		{
			shieldDamage *= 100;
		}
		damage = Mathf.Clamp((int)((float)damage * this.DamageCutOff), 1, int.MaxValue);
		this.pAttr.currentEnergy = this.pAttr.currentEnergy - shieldDamage;
		this.HitEffect(this.pAttr.faceDir, enemy);
		if (this.pAttr.currentEnergy > 0)
		{
			this.HitShidleEffect(158, 186);
			this.HpMinus(damage);
			Core.Input.Vibration.Vibrate(1);
			return true;
		}
		this.HitShidleEffect(161, 191);
		Core.Input.Vibration.Vibrate(3);
		this._broken = true;
		return false;
	}

	private void HitShidleEffect(int effectId, int soundId)
	{
		Vector3 position = this.pac.transform.position - this.pAttr.bounds.center;
		position.Set(position.x, position.y + 2f, position.z);
		R.Effect.Generate(effectId, this.pac.transform, position, default(Vector3), default(Vector3), true);
		R.Audio.PlayEffect(soundId, new Vector3?(this.pac.transform.position));
	}

	private void HitEffect(int dir, Transform enemy)
	{
		R.Effect.Generate((enemy.position.x - this.pac.transform.position.x <= 0f) ? 76 : 80, null, this.pac.transform.position + new Vector3((float)dir * 0.2f, 1.3f, 0f), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(70, null, this.pac.transform.position + new Vector3((float)dir * 0.2f, 1.3f, 0f), Vector3.zero, default(Vector3), true);
	}

	private void HurtState(string sta)
	{
		EventManager.PostEvent<GameObject>("PlayerHurt", this.pac.gameObject);
		this.pac.ChangeState(sta, 1f);
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.lastState == "Disappear" || args.lastState == "UnderAtkEat")
		{
			this.ShadowControl(true);
		}
		if (args.nextState == "Disappear" || args.nextState == "UnderAtkEat")
		{
			this.ShadowControl(false);
		}
		if (args.lastState == "UnderAtkGetUp")
		{
			this.Invincible = true;
			this._getUpInvincibleTime = WorldTime.SecondToFrame(0.2f);
		}
		if (args.nextState == "UnderAtkHitToFly")
		{
			this._hurtTimes = 0;
			this._hurtLimit = 0f;
		}
		if (args.nextState == "UnderAtk1")
		{
			this._hurtTimes++;
			this._hurtLimit = 0.5f;
		}
	}

	private void UpdateHurtGetUp()
	{
		if (this._getUpInvincibleTime > 0)
		{
			this._getUpInvincibleTime--;
			if (this._getUpInvincibleTime <= 0)
			{
				this.Invincible = false;
			}
		}
	}

	public bool DeadFlag;

	private List<int> hurtId = new List<int>();

	private float _clearRate;

	private PlayerSpHurt _spHurt;

	private bool _broken;

	private float _energyRecover;

	private int _hurtTimes;

	private float _hurtLimit;

	public bool Invincible;

	private int _getUpInvincibleTime;
}
