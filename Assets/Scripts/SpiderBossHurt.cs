using System;
using GameWorld;
using UnityEngine;

public class SpiderBossHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.愚笨蜘蛛];
	}

	protected override void PlayHurtAudio()
	{
		R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		if (this.action.IsInWeakSta())
		{
			return;
		}
		if (this.action.stateMachine.currentState == "Hit2")
		{
			return;
		}
		if (this.action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.PhysicAndEffect(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		normalAtkType = "IdleToWeakMod";
		airAtkType = "IdleToWeakMod";
		base.HitIntoWeakState(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void FlashAttackHurt()
	{
		this.ExecuteDieEffect();
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.0833333358f, 1f, base.transform.position);
		R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
		this.SetHitSpeed(Vector2.zero);
		base.GenerateCritHurtNum(base.flashAttackDamage);
		this.action.AnimChangeState(HammerAction.StateEnum.Hit1, 0.5f);
		base.FlashAttackHurt();
	}

	public override void QTEHurt()
	{
		base.QTEHurt();
		R.Player.Action.QTEHPRecover(false);
		base.QTEZPositionRecover();
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
			R.Trophy.AwardTrophy(27);
		}
		R.Player.Action.QTEHPRecover(this.eAttr.rankType == EnemyAttribute.RankType.BOSS);
		R.Audio.PlayEffect(412, new Vector3?(base.transform.position));
		R.Audio.PlayEffect(UnityEngine.Random.Range(105, 108), new Vector3?(base.transform.position));
		this.deadFlag = true;
		this.eAttr.currentHp = 0;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.stiffTime = 0f;
		this.eAttr.timeController.SetGravity(1f);
		R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
		EventManager.PostEvent<EnemyAttribute>("EnemyKilled", this.eAttr);
		this.action.WeakEffectDisappear("Null");
		R.Effect.Generate(91, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(49, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(14, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
		base.AddCoinAndExp();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
		this.DieEffect();
		for (int i = 0; i < R.Enemy.EnemyAttributes.Count; i++)
		{
			R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseAction>().KillSelf();
		}
	}

	private void DieEffect()
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.corePrefab, this.center.position, Quaternion.identity);
		transform.GetComponent<Rigidbody2D>().velocity = new Vector2((float)(-2 * this.eAttr.faceDir), 10f);
		transform.GetComponent<EnemyArm>().SetAngularSpeed((float)(100 * this.eAttr.faceDir));
		Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.headPrefab, this.headPos.position, Quaternion.identity);
		transform2.GetComponent<Rigidbody2D>().velocity = new Vector2((float)(2 * this.eAttr.faceDir), 10f);
		transform2.GetComponent<EnemyArm>().SetAngularSpeed((float)(100 * -(float)this.eAttr.faceDir));
	}

	[SerializeField]
	private Transform headPos;

	[SerializeField]
	private Transform headPrefab;

	[SerializeField]
	private Transform corePrefab;
}
