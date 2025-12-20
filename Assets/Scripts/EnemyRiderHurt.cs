using System;
using GameWorld;
using UnityEngine;

public class EnemyRiderHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "Hit1";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.骑兵];
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 aieSpeed, string normalAtkType, string airAtkType)
	{
		if (this.action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.PhysicAndEffect(speed, aieSpeed, normalAtkType, airAtkType);
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
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
		this.action.AnimChangeState(RiderAction.StateEnum.Hit1, 0.5f);
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
		R.Player.Action.QTEHPRecover(false);
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
		base.AddCoinAndExp();
		this.SpDieEffect();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
	}

	private void SpDieEffect()
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.headPrefab, this.headPos.position, this.headPos.localRotation);
		Vector3 localScale = transform.localScale;
		localScale.x *= Mathf.Sign(base.transform.localScale.x);
		transform.localScale = localScale;
		Vector2 velocity = new Vector2(Mathf.Sign(base.transform.localScale.x) * 5f, 20f);
		transform.GetComponent<Rigidbody2D>().velocity = velocity;
		transform.GetComponent<EnemyArm>().SetAngularSpeed(1000f);
	}

	[SerializeField]
	private Transform headPos;

	[SerializeField]
	private Transform headPrefab;
}
