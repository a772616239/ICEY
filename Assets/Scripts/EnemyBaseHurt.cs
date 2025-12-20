using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using ExtensionMethods;
using GameWorld;
using LitJson;
using UnityEngine;

public class EnemyBaseHurt : BaseBehaviour
{
	protected int flashAttackDamage
	{
		get
		{
			if (R.Player.Enhancement.FlashAttack == 0)
			{
				return 0;
			}
			return (int)((float)this.eAttr.maxHp * 0.05f * UnityEngine.Random.Range(0.95f, 1.05f));
		}
	}

	protected bool flashPercent
	{
		get
		{
			int num = UnityEngine.Random.Range(1, 100);
			int flashAttack = R.Player.Enhancement.FlashAttack;
			if (flashAttack == 1)
			{
				return num < 30;
			}
			if (flashAttack != 2)
			{
				return flashAttack == 3;
			}
			return num < 60;
		}
	}

	protected GameObject player
	{
		get
		{
			return R.Player.GameObject;
		}
	}

	protected PlayerAttribute pAttr
	{
		get
		{
			return R.Player.Attribute;
		}
	}

	public List<int> phaseHp
	{
		get
		{
			if (this._phaseHp != null && this._phaseHp.Count != 0)
			{
				return this._phaseHp;
			}
			this._phaseHp = new List<int>();
			if (this.hpPercent.Length == 0)
			{
				this.maxPhase = 1;
				this._phaseHp.Add(this.eAttr.maxHp);
				return this._phaseHp;
			}
			this.maxPhase = this.hpPercent.Length;
			if (this.eAttr.rankType != EnemyAttribute.RankType.Normal)
			{
				for (int i = 0; i < this.hpPercent.Length; i++)
				{
					this._phaseHp.Add(this.eAttr.maxHp * this.hpPercent[i] / 100);
				}
			}
			return this._phaseHp;
		}
	}

	private float speedDir
	{
		get
		{
			return Mathf.Sign(base.transform.localScale.x);
		}
	}

	protected void Awake()
	{
		this.eAttr = base.GetComponent<EnemyAttribute>();
		this.armor = base.GetComponent<EnemyArmor>();
		this.action = base.GetComponent<EnemyBaseAction>();
		this.Pivot = base.GetComponent<Pivot>();
	}

	protected void Start()
	{
		this.frameShakeOffset = this.m_frameShakeOffset;
		this.spHurtAudio = 6;
		this.chaseEnd = 0f;
		this.Init();
	}

	protected void OnEnable()
	{
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurt), EventManager.ListenerQueue.Game);
	}

	protected void OnDisable()
	{
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurt), EventManager.ListenerQueue.Game);
	}

	private void OnDestroy()
	{
		this.QTECameraFinish();
	}

	protected virtual void Update()
	{
		this.UpdateEnemyDie();
		this.UpdateExecuteFollow();
		if (this.deadFlag)
		{
			return;
		}
		this.UpdateChase();
	}

	private void UpdateEnemyDie()
	{
		if (this.eAttr.isDead && !this.deadFlag && this.eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			this.EnemyDie();
		}
	}

	private void UpdateExecuteFollow()
	{
		if (this.eAttr.followLeftHand)
		{
			Vector3 position = R.Player.Action.executeFollow.position + new Vector3((float)this.eAttr.faceDir * this.centerOffset.x, this.centerOffset.y, this.centerOffset.z);
			position.z = LayerManager.ZNum.Fx;
			base.transform.position = position;
		}
	}

	private void UpdateChase()
	{
		if (this.eAttr.canBeChased)
		{
			this.chaseEnd += Time.unscaledDeltaTime;
			if (this.chaseEnd >= 1.3f)
			{
				this.ChaseEnd();
			}
		}
	}

	protected virtual void Init()
	{
	}

	protected void QTEZPositionRecover()
	{
		Vector3 position = base.transform.position;
		position.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
		base.transform.position = position;
	}

	private bool EnemyHurt(string eventName, object sender, EnemyHurtAtkEventArgs args)
	{
		if (args.hurted != base.gameObject)
		{
			return false;
		}
		switch (args.hurtType)
		{
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Normal:
			this.NormalHurt(args.attackData, args.attackId, args.body, args.hurtPos);
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.ExecuteFollow:
			this.ExecuteFollow();
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Execute:
			this.Execute(args.attackData.atkName, true);
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt:
			this.QTEHurt();
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Flash:
			this.FlashAttackHurt();
			break;
		}
		return true;
	}

	public IEnumerator ClipShake(int frame)
	{
		for (int i = 0; i < frame; i++)
		{
			if (i % 2 == 0)
			{
				this.frameShakeBody.localPosition += Vector3.right * this.frameShakeOffset;
			}
			else
			{
				this.frameShakeBody.localPosition -= Vector3.right * this.frameShakeOffset;
			}
			yield return null;
		}
		this.frameShakeBody.localPosition = Vector3.zero;
		yield break;
	}

	protected void TimeFrozenAndCameraShake(int frozenFrame, int frameShakeFrame, int shakeType, int shakeFrame, float shakeOffset)
	{
		WorldTime.FrozenArgs.FrozenType type = (!(this.playerAtkName == "HitGround")) ? WorldTime.FrozenArgs.FrozenType.All : WorldTime.FrozenArgs.FrozenType.Enemy;
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(frozenFrame, type, true);
		base.StopCoroutine("ClipShake");
		base.StartCoroutine(this.ClipShake(frameShakeFrame));
		if (shakeType != 0)
		{
			if (shakeType != 1)
			{
				if (shakeType == 2)
				{
					R.Camera.Controller.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Vertical, false);
				}
			}
			else
			{
				R.Camera.Controller.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Horizon, false);
			}
		}
		else
		{
			R.Camera.Controller.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Rect, false);
		}
	}

	public virtual void SetHitSpeed(Vector2 speed)
	{
		if (this.eAttr.isDead && speed.y > 0f)
		{
			return;
		}
		this.eAttr.timeController.SetSpeed(speed);
	}

	protected void PlayHurtAnim(string normalSta, string airSta, Vector2 speed, Vector2 airSpeed)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.eAttr.iCanFly)
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.action.AnimChangeState((!this.eAttr.isOnGround) ? airSta : normalSta, 1f);
		this.SetHitSpeed((!this.eAttr.isOnGround) ? airSpeed : speed);
	}

	public void HitEffect(Vector3 pos, string atkName = "Atk1")
	{
		R.Effect.Generate(1, base.transform, pos, new Vector3(0f, 0f, 0f), default(Vector3), true);
		R.Effect.Generate(71, base.transform, pos + Vector3.right * (float)this.pAttr.faceDir * 0.5f, new Vector3(0f, (float)((this.pAttr.faceDir <= 0) ? 0 : 180), UnityEngine.Random.Range(-90f, 0f)), default(Vector3), true);
		R.Effect.Generate(151, base.transform, pos, new Vector3(0f, 0f, 0f), default(Vector3), true);
		if (atkName.IsInArray(PlayerAtkType.HeavyEffectAttack))
		{
			R.Effect.Generate(156, base.transform, pos, default(Vector3), default(Vector3), true);
		}
	}

	private void HitIntoWeakEffect(Vector3 pos, string atkName)
	{
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(48, 0.5f);
		this.HitEffect(pos, atkName);
	}

	protected virtual void PlayHurtAudio()
	{
		R.Audio.PlayEffect(UnityEngine.Random.Range(129, 132), new Vector3?(base.transform.position));
	}

	protected bool PlaySpHurtAudio()
	{
		if (!this.SoundJudge())
		{
			return true;
		}
		bool flag = UnityEngine.Random.Range(0, 100) < this.spHurtAudio;
		if (flag)
		{
			this.spHurtAudio = 6;
			return true;
		}
		this.spHurtAudio += 6;
		return false;
	}

	private bool SoundJudge()
	{
		int enemyType = (int)this.eAttr.baseData.enemyType;
		switch (enemyType)
		{
		case 1:
		case 2:
		case 3:
			break;
		default:
			switch (enemyType)
			{
			case 22:
			case 23:
			case 26:
				break;
			default:
				switch (enemyType)
				{
				case 15:
				case 18:
					break;
				default:
					if (enemyType != 33 && enemyType != 37)
					{
						return false;
					}
					break;
				}
				break;
			}
			break;
		}
		return true;
	}

	protected virtual void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		if (this.playerAtkName != "Charge1EndLevel1" && !this.playerAtkName.IsInArray(PlayerAction.ExecuteSta))
		{
			if (this.eAttr.paBody)
			{
				return;
			}
			if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint && !this.defecnceBreak)
			{
				return;
			}
		}
		this.defecnceBreak = false;
		if (this.eAttr.isOnGround && normalAtkType != "NoStiff")
		{
			this.eAttr.stiffTime = 1f;
			this.PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
		}
		else if (!this.eAttr.isOnGround && airAtkType != "NoStiff")
		{
			this.eAttr.stiffTime = 1f;
			this.PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
		}
		else
		{
			this.eAttr.stiffTime = 0f;
		}
	}

	public void QTECameraStart()
	{
		if (!this.qteCameraEffectOn)
		{
			this.qteCameraEffectOn = true;
		}
	}

	public void QTECameraFinish()
	{
		if (this.qteCameraEffectOn)
		{
			this.qteCameraEffectOn = false;
		}
	}

	protected bool BloodWeak()
	{
		return this.HpInWeak() && !this.eAttr.isDead && !this.action.IsInWeakSta() && this.eAttr.accpectExecute;
	}

	protected bool HpInWeak()
	{
		int num = 0;
		for (int i = 0; i < this.currentPhase; i++)
		{
			num += this.GetPhaseHp(i);
		}
		num += (int)((float)this.GetPhaseHp(this.currentPhase) * ((this.eAttr.rankType != EnemyAttribute.RankType.Normal) ? 0.9f : 0.7f));
		return this.eAttr.currentHp < this.eAttr.maxHp - num;
	}

	public void GenerateCritHurtNum(int damage)
	{
		if (damage == 0)
		{
			return;
		}
		this.HpMinus(damage);
	}

	protected virtual void HpMinus(int num)
	{
		if (this.eAttr.rankType != EnemyAttribute.RankType.Normal)
		{
			this.eAttr.currentHp = Mathf.Clamp(this.eAttr.currentHp - num, this.MinLockedHp(), int.MaxValue);
			return;
		}
		this.eAttr.currentHp -= num;
	}

	protected void QTEHpMinus()
	{
		int num = 0;
		for (int i = 0; i < this.currentPhase; i++)
		{
			num += this.GetPhaseHp(i);
		}
		this.eAttr.currentHp = this.eAttr.maxHp - num;
	}

	protected int MinLockedHp()
	{
		int num = 0;
		for (int i = 0; i < this.currentPhase + 1; i++)
		{
			int phaseHp = this.GetPhaseHp(i);
			num += phaseHp;
		}
		return this.eAttr.maxHp - num + 1;
	}

	protected void HpRecover()
	{
		int num = 0;
		for (int i = 0; i < this.currentPhase; i++)
		{
			num += this.GetPhaseHp(i);
		}
		num += (int)((float)this.GetPhaseHp(this.currentPhase) * 0.7f);
		this.eAttr.currentHp = this.eAttr.maxHp - num;
	}

	protected virtual bool Counterattack(int damage, bool groundOnly)
	{
		return HurtDataTools.Counterattack(damage, groundOnly, ref this.actionInterrupt, ref this.eAttr, ref this.action);
	}

	protected void AddActionInterruptPoint(int damage, string atkName)
	{
		HurtDataTools.AddActionInterruptPoint(damage, atkName, ref this.eAttr, ref this.actionInterrupt);
	}

	protected bool CalculateMonsterDefence(int damage)
	{
		return HurtDataTools.CalculateMonsterDefence(damage, ref this.defenceTrigger, ref this.action, ref this.eAttr);
	}

	protected bool CalculateMonsterSideStep(int damage)
	{
		return HurtDataTools.CalculateMonsterSideStep(damage, ref this.sideStepTrigger, ref this.action, ref this.eAttr);
	}

	protected int GetPhaseHp(int phase)
	{
		if (phase < 0 || phase > this.phaseHp.Count)
		{
			return 0;
		}
		return this.phaseHp[phase];
	}

	public void StopFollowLeftHand()
	{
		this.eAttr.followLeftHand = false;
	}

	protected virtual void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		this.ChaseEnd();
		this.action.EnterWeakState();
		this.eAttr.stiffTime = 1f;
		if (this.eAttr.isOnGround && normalAtkType != "NoStiff")
		{
			this.action.FaceToPlayer();
			this.PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
		}
		else if (!this.eAttr.isOnGround && airAtkType != "NoStiff")
		{
			this.action.FaceToPlayer();
			this.PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
		}
		if (!base.IsInvoking("ExitWeak"))
		{
			this.exitWeakPhase = this.currentPhase;
			base.Invoke("ExitWeak", 5f);
		}
	}

	protected virtual void ExitWeak()
	{
		if (!this.action.IsInWeakSta() || this.exitWeakPhase != this.currentPhase || this.eAttr.willBeExecute)
		{
			return;
		}
		this.HpRecover();
		this.action.ExitWeakState(false);
	}

	public virtual void NormalHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, int atkId, HurtCheck.BodyType body, Vector2 hurtPos)
	{
		if (this.hurtId >= atkId || !this.hurtData.Contains(atkData.atkName))
		{
			return;
		}
		this.GetHurt(atkId);
		EnemyBaseHurt.HurtAttribute hurtAttribute = new EnemyBaseHurt.HurtAttribute(this.hurtData[atkData.atkName], this.defaultAnimName, this.defaultAirAnimName);
		this.SpeedAdjust(atkData.atkName, ref hurtAttribute);
		this.playerAtkName = atkData.atkName;
		Vector2 speed = new Vector2(hurtAttribute.xSpeed, hurtAttribute.ySpeed);
		Vector2 airSpeed = new Vector2(hurtAttribute.airXSpeed, hurtAttribute.airYSpeed);
		Vector3 pos = (Vector3)hurtPos - base.transform.position;
		int finalDamage = PlayerDamageCalculate.GetFinalDamage(atkData.damagePercent, HurtDataTools.GetAtkLevel(this.playerAtkName), this.playerAtkName);
		if (this.eAttr.isArmorBroken)
		{
			this.AddActionInterruptPoint(finalDamage, this.playerAtkName);
			bool flag = this.CalDefense(finalDamage);
			bool flag2 = this.CalSideStep(finalDamage);
			this.canCounterattack = this.Counterattack(finalDamage, this.eAttr.isOnGround);
			if (atkData.joystickShakeNum > 0)
			{
				Core.Input.Vibration.Vibrate(atkData.joystickShakeNum);
			}
			if (this.canCounterattack)
			{
				this.PlayHurtAudio();
				this.DoCounterAttack(atkData, pos);
				this.GenerateCritHurtNum(finalDamage);
				this.DoWeak(pos, speed, airSpeed, hurtAttribute, atkData);
				return;
			}
			this.CanDefenseOrSideStep(ref flag, ref flag2, ref finalDamage);
			if (this.InDefense(flag, atkData, pos))
			{
				return;
			}
			flag = false;
			this.NotInDefense();
			speed.x *= this.speedDir;
			airSpeed.x *= this.speedDir;
			if (!this.eAttr.isDead)
			{
				this.GenerateCritHurtNum(finalDamage);
			}
			if (this.DoWeak(pos, speed, airSpeed, hurtAttribute, atkData))
			{
				return;
			}
			if (!flag2)
			{
				this.HitEffect(pos, atkData.atkName);
			}
			if (!flag && !flag2)
			{
				this.DoHurt(atkData, speed, airSpeed, hurtAttribute);
			}
		}
		else
		{
			this.armor.HitArmor(finalDamage, atkData.atkName);
		}
	}

	private void GetHurt(int atkId)
	{
		this.hurtId = atkId;
		if (this.atkBox != null)
		{
			this.atkBox.localScale = Vector3.zero;
		}
	}

	private void SpeedAdjust(string name, ref EnemyBaseHurt.HurtAttribute hurtAttr)
	{
		if ((name == "UpRising" || name == "AtkUpRising") && hurtAttr.airYSpeed > 0f)
		{
			float num = Mathf.Clamp(Physics2D.Raycast(base.transform.position, -Vector2.up, 100f, LayerManager.GroundMask).distance, 0f, float.PositiveInfinity);
			hurtAttr.airYSpeed = Mathf.Lerp(hurtAttr.ySpeed, 6f, num / 4f);
		}
	}

	private bool CalDefense(int hitNumber)
	{
		bool flag = this.CalculateMonsterDefence(hitNumber);
		if (flag)
		{
			this.eAttr.dynamicDefence = (int)((float)this.eAttr.baseDefence * UnityEngine.Random.Range(0.7f, 1.3f));
			this.defenceTrigger = 0f;
		}
		return flag;
	}

	private bool CalSideStep(int hitNumber)
	{
		bool flag = this.CalculateMonsterSideStep(hitNumber);
		if (flag)
		{
			this.eAttr.dynamicSideStep = (int)((float)this.eAttr.baseSideStep * UnityEngine.Random.Range(0.7f, 1.3f));
			this.sideStepTrigger = 0f;
		}
		return flag;
	}

	private void DoCounterAttack(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
	{
		this.TimeFrozenAndCameraShake(atkData.frozenFrame, atkData.shakeFrame, atkData.shakeType, atkData.camShakeFrame, atkData.shakeStrength);
		this.HitEffect(pos, atkData.atkName);
		this.eAttr.currentDefence = 0;
		this.eAttr.currentSideStep = 0;
		int dir = InputSetting.JudgeDir(base.transform.position.x, this.player.transform.position.x);
		this.action.CounterAttack(dir);
	}

	private void CanDefenseOrSideStep(ref bool defence, ref bool sideStep, ref int hitNumber)
	{
		if (defence || sideStep)
		{
			if (defence && sideStep)
			{
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					this.eAttr.currentSideStep /= 2;
					this.action.Defence();
					sideStep = false;
				}
				else
				{
					this.eAttr.currentDefence /= 2;
					this.action.SideStep();
					defence = false;
					hitNumber = 0;
				}
			}
			else if (defence)
			{
				this.action.Defence();
			}
			else
			{
				this.action.SideStep();
				hitNumber = 0;
			}
		}
	}

	private bool InDefense(bool defence, EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
	{
		if (defence)
		{
			this.DoDefenseSucces(atkData, pos);
			return true;
		}
		if (!this.action.IsInDefenceState())
		{
			return false;
		}
		if (!this.playerAtkName.IsInArray(PlayerAtkType.BreakDefense))
		{
			this.DoDefenseSucces(atkData, pos);
			return true;
		}
		this.DoFefenseBreak();
		return false;
	}

	private void DoDefenseSucces(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
	{
		R.Effect.Generate(157, base.transform.Find("HurtBox"), default(Vector3), default(Vector3), default(Vector3), true);
		this.action.FaceToPlayer();
		this.action.DefenceSuccess();
		this.HitEffect(pos, atkData.atkName);
	}

	private void NotInDefense()
	{
		if (this.canChangeFace && !this.eAttr.paBody && this.eAttr.currentActionInterruptPoint >= this.eAttr.actionInterruptPoint)
		{
			int dir = (R.Player.Attribute.faceDir != 1) ? 1 : -1;
			this.action.ChangeFace(dir);
		}
	}

	private void DoFefenseBreak()
	{
		R.Audio.PlayEffect(192, new Vector3?(base.transform.position));
		R.Effect.Generate(160, base.transform.Find("HurtBox"), default(Vector3), default(Vector3), default(Vector3), true);
		Core.Input.Vibration.Vibrate(4);
		this.eAttr.currentActionInterruptPoint = 0;
		this.defecnceBreak = true;
		this.eAttr.paBody = false;
	}

	private bool DoWeak(Vector3 pos, Vector2 speed, Vector2 airSpeed, EnemyBaseHurt.HurtAttribute hurtAttr, EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData)
	{
		if (this.BloodWeak())
		{
			this.HitIntoWeakState(speed, airSpeed, hurtAttr.normalAtkType, hurtAttr.airAtkType);
			this.HitIntoWeakEffect(pos, atkData.atkName);
			return true;
		}
		return false;
	}

	private void DoHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector2 speed, Vector2 airSpeed, EnemyBaseHurt.HurtAttribute hurtAttr)
	{
		this.PlayHurtAudio();
		if (!this.eAttr.isDead && HurtDataTools.ChaseAttack())
		{
			this.currentChaseTime = 0;
			this.ChaseStart();
		}
		this.SpAttack();
		this.PhysicAndEffect(speed, airSpeed, hurtAttr.normalAtkType, hurtAttr.airAtkType);
		this.TimeFrozenAndCameraShake(atkData.frozenFrame, atkData.shakeFrame, atkData.shakeType, atkData.camShakeFrame, atkData.shakeStrength);
	}

	protected virtual void FlashAttackHurt()
	{
		HurtDataTools.FlashHPRecover();
		if (this.eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			this.Execute("NewExecute1_2", false);
		}
		else if (this.flashPercent)
		{
			R.Effect.Generate(127, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		}
		Core.Input.Vibration.Vibrate(4);
		if (this.BloodWeak())
		{
			this.action.EnterWeakState();
			if (!base.IsInvoking("ExitWeak"))
			{
				base.Invoke("ExitWeak", 5f);
			}
		}
	}

	public virtual void QTEHurt()
	{
		if (this.eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			return;
		}
		this.eAttr.willBeExecute = false;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.timeController.SetGravity(1f);
		this.SetHitSpeed(Vector2.zero);
		this.action.hurtBox.gameObject.SetActive(true);
		this.currentPhase = Mathf.Clamp(this.currentPhase + 1, 0, this.maxPhase);
		this.ExecuteDieEffect();
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.2f);
		R.Camera.Controller.CameraShake(0.5f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		R.Effect.Generate(127, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
	}

	protected virtual void ExecuteFollow()
	{
		int dir = (this.player.transform.localScale.x >= 0f) ? 1 : -1;
		this.action.ChangeFace(dir);
		this.action.WeakEffectDisappear("RollEnd");
		this.eAttr.followLeftHand = true;
		this.eAttr.willBeExecute = true;
		this.eAttr.checkHitGround = false;
		this.eAttr.flyToFall = false;
		this.eAttr.isFlyingUp = false;
		this.SetHitSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.StartCoroutine(this.ClipShake(17));
	}

	protected virtual void SpAttack()
	{
		if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint)
		{
			return;
		}
	}

	public void ChaseStart()
	{
		if (this.action.IsInWeakSta())
		{
			return;
		}
		if (this.player.GetComponent<PlayerAbilities>().flashAttack.CheckEnemy(base.gameObject))
		{
			return;
		}
		if (this.currentChaseTime > 2)
		{
			return;
		}
		this.currentChaseTime++;
		this.chaseEnd = 0f;
		this.eAttr.canBeChased = true;
		if (this._chaseCoroutine != null)
		{
			base.StopCoroutine(this._chaseCoroutine);
		}
		this._chaseCoroutine = base.StartCoroutine(this.ChaseCoroutine());
	}

	private IEnumerator ChaseCoroutine()
	{
		yield return new WaitForSeconds(0.3f);
		this.ChaseAttack();
		yield return new WaitForSeconds(0.3f);
		this.ChaseAttack();
		this.ChaseEnd();
		yield break;
	}

	private void ChaseAttack()
	{
		Vector3 position = base.transform.position;
		R.Audio.PlayEffect(UnityEngine.Random.Range(18, 21), new Vector3?(base.transform.position));
		position.y = Mathf.Clamp(position.y, LayerManager.YNum.GetGroundHeight(base.gameObject), float.MaxValue);
		Transform transform = R.Effect.Generate(182, null, position, default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x = (float)((UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1);
		transform.localScale = localScale;
	}

	public void ChaseEnd()
	{
		this.chaseEnd = 0f;
		this.currentChaseTime = 0;
		this.eAttr.canBeChased = false;
	}

	protected void Execute(string playerState, bool chargeUp = true)
	{
		this.eAttr.willBeExecute = false;
		this.eAttr.inWeakState = false;
		this.action.hurtBox.gameObject.SetActive(true);
		this.playerAtkName = playerState;
		this.ExecuteDie();
		if (chargeUp || this.flashPercent)
		{
			R.Effect.Generate(127, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		}
	}

	protected virtual void ExecuteDie()
	{
		R.Audio.PlayEffect(UnityEngine.Random.Range(105, 108), new Vector3?(base.transform.position));
		this.deadFlag = true;
		this.eAttr.currentHp = 0;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.stiffTime = 0f;
		this.eAttr.timeController.SetGravity(1f);
		EventManager.PostEvent<EnemyAttribute>("EnemyKilled", this.eAttr);
		this.action.WeakEffectDisappear("Null");
		R.Effect.Generate(91, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(49, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(14, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
		this.AddCoinAndExp();
		this.ExecuteDieEffect();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.13333334f, 1f, base.transform.position);
	}

	public virtual void EnemyDie()
	{
		if (this.eAttr.rankType != EnemyAttribute.RankType.Normal)
		{
			return;
		}
		R.Audio.PlayEffect(UnityEngine.Random.Range(105, 108), new Vector3?(base.transform.position));
		this.deadFlag = true;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.stiffTime = 0f;
		this.eAttr.timeController.SetGravity(1f);
		EventManager.PostEvent<EnemyAttribute>("EnemyKilled", this.eAttr);
		this.action.WeakEffectDisappear("Null");
		base.StartCoroutine(this.GenerateEnergyBall());
		this.AddCoinAndExp();
	}

	private IEnumerator GenerateEnergyBall()
	{
		yield return new WaitForSeconds(0.8f);
		R.Effect.Generate(91, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		yield break;
	}

	protected void AddCoinAndExp()
	{
		R.Equipment.CoinNum += this.eAttr.dropCoins;
	}

	protected void DieTimeControl()
	{
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(25, WorldTime.FrozenArgs.FrozenType.Enemy, true);
		R.Camera.Controller.CameraShake(0.416666657f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		if (base.gameObject.activeSelf)
		{
			base.StartCoroutine(this.ClipShake(12));
		}
	}

	public void NormalKill()
	{
		R.Audio.PlayEffect(24, new Vector3?(base.transform.position));
		R.Camera.Controller.CameraBloom(0.25f, 0f);
		R.Effect.Generate(49, base.transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(9, base.transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), new Vector3(0f, 0f, 0f), default(Vector3), true);
		R.Effect.Generate(14, base.transform, new Vector3(0f, 1.2f, -0.1f), new Vector3(0f, 0f, 0f), default(Vector3), true);
	}

	protected virtual void ExecuteDieEffect()
	{
		R.Effect.Generate(156, null, this.center.position, default(Vector3), default(Vector3), true);
	}

	protected virtual IEnumerator DeathIEnumerator()
	{
		bool deadFly = true;
		bool deadFall = false;
		while (this.eAttr.isDead)
		{
			if (deadFly && this.eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				deadFly = false;
				deadFall = true;
				this.action.AnimChangeState(this.airDieAnimName, 1f);
			}
			if (deadFall && this.eAttr.isOnGround)
			{
				deadFall = false;
				this.action.AnimChangeState(this.airDieHitGroundAnimName, 1f);
			}
			yield return null;
		}
		yield break;
	}

	public bool canCounterattack;

	public Transform center;

	public int currentPhase;

	public const int maxChaseTime = 2;

	public int currentChaseTime;

	protected int hurtId;

	protected bool deadFlag;

	protected string defaultAnimName;

	protected string defaultAirAnimName;

	protected string airDieAnimName;

	protected string airDieHitGroundAnimName;

	protected string flyToFallAnimName;

	protected Pivot Pivot;

	protected EnemyAttribute eAttr;

	protected EnemyBaseAction action;

	protected JsonData hurtData;

	protected string playerAtkName;

	protected bool actionInterrupt;

	protected bool defecnceBreak;

	protected int spHurtAudio;

	[SerializeField]
	private List<int> _phaseHp;

	protected int maxPhase;

	[SerializeField]
	private Transform atkBox;

	[SerializeField]
	private bool canChangeFace = true;

	[SerializeField]
	private Transform frameShakeBody;

	private EnemyArmor armor;

	[SerializeField]
	private Vector3 centerOffset;

	[SerializeField]
	[Header("左右抖动幅度")]
	private float m_frameShakeOffset;

	[SerializeField]
	private int[] hpPercent;

	private float frameShakeOffset;

	private float defenceTrigger;

	private float sideStepTrigger;

	private float chaseEnd;

	private int exitWeakPhase;

	private bool qteCameraEffectOn;

	private Coroutine _chaseCoroutine;

	private class HurtAttribute
	{
		public HurtAttribute(JsonData hurt, string defaultAnimName, string defaultAirAnimName)
		{
			this.xSpeed = hurt.Get<float>("xSpeed", 0f);
			this.ySpeed = hurt.Get<float>("ySpeed", 0f);
			this.airXSpeed = hurt.Get<float>("airXSpeed", 0f);
			this.airYSpeed = hurt.Get<float>("airYSpeed", 0f);
			this.normalAtkType = hurt.Get<string>("normalAtkType", defaultAnimName);
			this.airAtkType = hurt.Get<string>("airAtkType", defaultAirAnimName);
		}

		public float xSpeed;

		public float ySpeed;

		public float airXSpeed;

		public float airYSpeed;

		public string normalAtkType;

		public string airAtkType;
	}
}
