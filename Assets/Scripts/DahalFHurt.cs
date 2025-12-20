using System;
using GameWorld;
using UnityEngine;

public class DahalFHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this._dahal = base.GetComponent<DahalFAnim>();
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "Hit1";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.达哈尔终极版];
	}

	protected override void Update()
	{
		base.Update();
		Vector2? rollAtkFollowPos = this._rollAtkFollowPos;
		if (rollAtkFollowPos != null)
		{
			Vector3 position = base.player.transform.position;
			Vector2? rollAtkFollowPos2 = this._rollAtkFollowPos;
			Vector3 position2 = position - ((rollAtkFollowPos2 == null) ? default(Vector3) : (Vector3)rollAtkFollowPos2.GetValueOrDefault());
			position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 1f, float.PositiveInfinity);
			position2.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
			base.transform.position = position2;
			this._rollFollowTime += Time.unscaledDeltaTime;
			if (this._rollFollowTime >= 0.2f)
			{
				this._rollAtkFollowPos = null;
			}
		}
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		if (this.action.stateMachine.currentState == "QTEHit")
		{
			return;
		}
		base.PhysicAndEffect(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override bool Counterattack(int damage, bool groundOnly)
	{
		return this.currentPhase != 0 && base.Counterattack(damage, true);
	}

	public override void SetHitSpeed(Vector2 speed)
	{
		this.eAttr.timeController.SetGravity(1f);
		if (this.playerAtkName == "UpRising" || this.playerAtkName == "AtkUpRising" || this.playerAtkName == "AtkRollEnd")
		{
			this._dahal.MaxFlyHeight = 4f;
		}
		else
		{
			this._dahal.MaxFlyHeight = -1f;
		}
		base.SetHitSpeed(speed);
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	public void HitIntoSecondLevel()
	{
		base.QTEZPositionRecover();
	}

	protected override void SpAttack()
	{
		if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint)
		{
			return;
		}
		if (this.playerAtkName == "RollReady" && !this.eAttr.isOnGround)
		{
			Vector2? rollAtkFollowPos = this._rollAtkFollowPos;
			if (rollAtkFollowPos == null)
			{
				this._rollAtkFollowPos = new Vector2?((base.player.transform.position - base.transform.position) * 0.7f);
			}
			this._rollFollowTime = 0f;
		}
	}

	protected override void FlashAttackHurt()
	{
		this.ExecuteDieEffect();
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.0833333358f, 1f, base.transform.position);
		this.SetHitSpeed(Vector2.zero);
		base.GenerateCritHurtNum(base.flashAttackDamage);
		DahalAction.StateEnum stateEnum = (!this.eAttr.isOnGround) ? DahalAction.StateEnum.HitToFly3 : DahalAction.StateEnum.Hit1;
		this.action.AnimChangeState(stateEnum, 0.5f);
		base.FlashAttackHurt();
	}

	public override void QTEHurt()
	{
		base.QTEHurt();
		R.Player.Action.QTEHPRecover(false);
		base.QTEHpMinus();
	}

	public override void EnemyDie()
	{
	}

	protected override void ExecuteDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		if (this.eAttr.rankType == EnemyAttribute.RankType.BOSS)
		{
			R.Trophy.AwardTrophy(32);
		}
		R.Audio.PlayEffect(412, new Vector3?(base.transform.position));
		R.Player.Action.QTEHPRecover(this.eAttr.rankType == EnemyAttribute.RankType.BOSS);
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
		R.Effect.Generate(156, base.transform, this.center.localPosition, default(Vector3), default(Vector3), true);
		base.AddCoinAndExp();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
	}

	private DahalFAnim _dahal;

	private Vector2? _rollAtkFollowPos;

	private float _rollFollowTime;
}
