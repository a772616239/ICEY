using System;
using System.Collections;
using UnityEngine;

public class BladeGunHurt : EnemyBaseHurt
{
	protected override void Update()
	{
		base.Update();
		Vector2? vector = this.atkFollowPos;
		if (vector != null)
		{
			Vector3 position = base.player.transform.position;
			Vector2? vector2 = this.atkFollowPos;
			Vector3 position2 = position - ((vector2 == null) ? default(Vector3) : (Vector3)vector2.GetValueOrDefault());
			position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 1f, float.PositiveInfinity);
			position2.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
			base.transform.position = position2;
			this.atkFollowTime += Time.unscaledDeltaTime;
			if (this.atkFollowTime >= this.atkFollowEnd)
			{
				this.atkFollowPos = null;
			}
		}
	}

	protected override void Init()
	{
		this.anim = base.GetComponent<BladeGunAnimEvent>();
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "HitToFly2";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.霰弹利刃];
	}

	public override void SetHitSpeed(Vector2 speed)
	{
		if (this.playerAtkName == "UpRising" || this.playerAtkName == "AtkUpRising" || this.playerAtkName == "AtkRollEnd" || this.playerAtkName == "NewExecute2_1")
		{
			this.anim.maxFlyHeight = 4.5f;
		}
		else
		{
			this.anim.maxFlyHeight = -1f;
		}
		base.SetHitSpeed(speed);
	}

	protected override void SpAttack()
	{
		if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint)
		{
			return;
		}
		if (this.playerAtkName == "RollEnd")
		{
			Vector2? vector = this.atkFollowPos;
			if (vector != null)
			{
				this.atkFollowPos = null;
			}
			return;
		}
		if (this.playerAtkName == "RollGround")
		{
			Vector2? vector2 = this.atkFollowPos;
			if (vector2 == null)
			{
				Transform transform = base.player.GetComponentInChildren<PlayerAtk>().transform;
				this.atkFollowPos = new Vector2?((transform.position - base.transform.position) * 0.9f);
			}
			this.atkFollowTime = 0f;
			this.atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "RollReady" || this.playerAtkName == "BladeStormReady")
		{
			Vector2? vector3 = this.atkFollowPos;
			if (vector3 == null)
			{
				this.atkFollowPos = new Vector2?((base.player.transform.position - base.transform.position) * 0.7f);
			}
			this.atkFollowTime = 0f;
			this.atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "Atk4")
		{
			base.StartCoroutine(this.CloseToPlayer());
			return;
		}
		this.atkFollowTime = 0f;
		this.atkFollowEnd = 0f;
	}

	private IEnumerator CloseToPlayer()
	{
		this.closeToPlayer.state.SetAnimation(0, "Show", false);
		this.closeToPlayer.skeleton.SetToSetupPose();
		Vector3 endPos = base.player.transform.position + Vector3.right * (float)base.pAttr.faceDir * UnityEngine.Random.Range(0.8f, 1.5f);
		endPos.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
		Vector3 startPos = base.transform.position;
		for (int i = 0; i < 5; i++)
		{
			yield return new WaitForFixedUpdate();
		}
		for (int j = 0; j < 6; j++)
		{
			base.transform.position = Vector3.Lerp(startPos, endPos, (float)j / 5f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	protected override void ExecuteFollow()
	{
	}

	protected override void ExecuteDie()
	{
		base.ExecuteDie();
		this.eAttr.timeController.SetGravity(1f);
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(BladeGunAction.StateEnum.Die, 1f);
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.EnemyDie();
		this.SetHitSpeed(Vector2.zero);
		base.NormalKill();
		this.action.AnimChangeState(BladeGunAction.StateEnum.Die, 1f);
		base.Invoke("DieTimeControl", 0.12f);
	}

	private BladeGunAnimEvent anim;

	private Vector2? atkFollowPos;

	private float atkFollowTime;

	private float atkFollowEnd;

	[SerializeField]
	private SkeletonAnimation closeToPlayer;
}
