using System;
using System.Collections;
using UnityEngine;

public class SpiderHurt : EnemyBaseHurt
{
	protected override void Update()
	{
		base.Update();
		Vector2? atkFollowPos = this._atkFollowPos;
		if (atkFollowPos != null)
		{
			Vector3 position = base.player.transform.position;
			Vector2? atkFollowPos2 = this._atkFollowPos;
			Vector3 position2 = position - ((atkFollowPos2 == null) ? default(Vector3) : (Vector3)atkFollowPos2.GetValueOrDefault());
			position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 1f, float.PositiveInfinity);
			position2.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
			base.transform.position = position2;
			this._atkFollowTime += Time.unscaledDeltaTime;
			if (this._atkFollowTime >= this._atkFollowEnd)
			{
				this._atkFollowPos = null;
			}
		}
	}

	protected override void PlayHurtAudio()
	{
		R.Audio.PlayEffect(256, new Vector3?(base.transform.position));
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	protected override void Init()
	{
		this._spider = base.GetComponent<SpiderAnimEvent>();
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "HitToFly2";
		this.airDieAnimName = "AirDieFlyToFall";
		this.airDieHitGroundAnimName = "AirDieHitGround";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.蜘蛛];
	}

	public override void SetHitSpeed(Vector2 speed)
	{
		if (this.playerAtkName == "UpRising" || this.playerAtkName == "AtkUpRising" || this.playerAtkName == "AtkRollEnd" || this.playerAtkName == "NewExecute2_1")
		{
			this._spider.maxFlyHeight = 4.2f;
		}
		else
		{
			this._spider.maxFlyHeight = -1f;
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
			Vector2? atkFollowPos = this._atkFollowPos;
			if (atkFollowPos != null)
			{
				this._atkFollowPos = null;
			}
			return;
		}
		if (this.playerAtkName == "RollGround")
		{
			Vector2? atkFollowPos2 = this._atkFollowPos;
			if (atkFollowPos2 == null)
			{
				Transform transform = base.player.GetComponentInChildren<PlayerAtk>().transform;
				this._atkFollowPos = new Vector2?((transform.position - base.transform.position) * 0.9f);
			}
			this._atkFollowTime = 0f;
			this._atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "RollReady" || this.playerAtkName == "BladeStormReady")
		{
			Vector2? atkFollowPos3 = this._atkFollowPos;
			if (atkFollowPos3 == null)
			{
				this._atkFollowPos = new Vector2?((base.player.transform.position - base.transform.position) * 0.7f);
			}
			this._atkFollowTime = 0f;
			this._atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "Atk4")
		{
			base.StartCoroutine(this.CloseToPlayer());
			return;
		}
		this._atkFollowTime = 0f;
		this._atkFollowEnd = 0f;
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

	protected override void ExecuteFollow()
	{
	}

	protected override void ExecuteDie()
	{
		base.ExecuteDie();
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(SpiderAction.StateEnum.Die, 1f);
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		R.Audio.PlayEffect(245, new Vector3?(base.transform.position));
		base.EnemyDie();
		if (this.eAttr.isOnGround)
		{
			this.SetHitSpeed(Vector2.zero);
			base.NormalKill();
			this.action.AnimChangeState(SpiderAction.StateEnum.Die, 1f);
			base.Invoke("DieTimeControl", 0.12f);
		}
		else
		{
			base.NormalKill();
			base.DieTimeControl();
			this.action.AnimChangeState(SpiderAction.StateEnum.AirDieFlyToFall, 1f);
			base.StartCoroutine(this.DeathIEnumerator());
		}
	}

	protected override IEnumerator DeathIEnumerator()
	{
		bool deadFly = true;
		bool deadFall = false;
		while (this.eAttr.isDead)
		{
			if (deadFly && this.eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				deadFly = false;
				deadFall = true;
				this.action.AnimChangeState(SpiderAction.StateEnum.AirDieFall, 1f);
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

	private SpiderAnimEvent _spider;

	private Vector2? _atkFollowPos;

	private float _atkFollowTime;

	private float _atkFollowEnd;

	[SerializeField]
	private SkeletonAnimation closeToPlayer;
}
