using System;
using System.Collections;
using UnityEngine;

public class JumperHurt : EnemyBaseHurt
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
			position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 0.1f, float.PositiveInfinity);
			position2.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
			base.transform.position = position2;
			this._atkFollowTime += Time.unscaledDeltaTime;
			if (this._atkFollowTime >= this._atkFollowEnd)
			{
				this._atkFollowPos = null;
			}
		}
	}

	protected override void Init()
	{
		this._jumper = base.GetComponent<JumperAnimListener>();
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "HitToFly2";
		this.airDieAnimName = "Fall";
		this.airDieHitGroundAnimName = "AirDie";
		this.flyToFallAnimName = "AirDiePre";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.跳跃者];
	}

	public override void SetHitSpeed(Vector2 speed)
	{
		if (this.playerAtkName == "UpRising" || this.playerAtkName == "AtkUpRising" || this.playerAtkName == "AtkRollEnd" || this.playerAtkName == "NewExecute2_1")
		{
			this._jumper.MaxFlyHeight = 4.5f;
		}
		else
		{
			this._jumper.MaxFlyHeight = -1f;
		}
		base.SetHitSpeed(speed);
	}

	protected override void ExecuteFollow()
	{
		this.action.AnimChangeState(JumperAction.StateEnum.Execute, 1f);
		base.ExecuteFollow();
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(397, new Vector3?(base.transform.position));
		}
	}

	protected override void SpAttack()
	{
		if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint)
		{
			return;
		}
		base.SpAttack();
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
				Vector2 a = base.player.transform.position - base.transform.position;
				this._atkFollowPos = new Vector2?(a * 0.7f);
			}
			this._atkFollowTime = 0f;
			this._atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "Atk4")
		{
			base.StartCoroutine(this.CloseToPlayer());
		}
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

	protected override void ExecuteDie()
	{
		base.ExecuteDie();
		R.Audio.PlayEffect(484, new Vector3?(base.transform.position));
		this.eAttr.timeController.SetGravity(1f);
		this.SetHitSpeed(Vector2.zero);
		this.SpDieEffect(this.eAttr.isOnGround);
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.EnemyDie();
		R.Audio.PlayEffect(484, new Vector3?(base.transform.position));
		if (this.eAttr.isOnGround)
		{
			base.NormalKill();
			this.SetHitSpeed(Vector2.zero);
			if (this.playerAtkName == "Charge1EndLevel1" || this.playerAtkName == "Charge1EndLevel2" || this.playerAtkName == "Charge1EndLevel3")
			{
				this.action.AnimChangeState(JumperAction.StateEnum.Die2, 1f);
			}
			else
			{
				this.action.AnimChangeState(JumperAction.StateEnum.Die, 1f);
			}
			base.Invoke("DieTimeControl", 0.12f);
		}
		else
		{
			Vector2 speed = this.eAttr.timeController.GetCurrentSpeed() + new Vector2(Mathf.Sign(base.transform.position.x - base.player.transform.position.x) * 1f, 4f);
			base.NormalKill();
			base.DieTimeControl();
			this.eAttr.timeController.SetSpeed(speed);
			base.StartCoroutine(this.DeathIEnumerator());
		}
	}

	private void SpDieEffect(bool isGround)
	{
		Transform transform = R.Effect.Generate(227, null, this.center.position, Vector3.zero, default(Vector3), true);
		transform.localScale = base.transform.localScale;
		Transform transform2 = R.Effect.Generate(228, null, this.center.position, Vector3.zero, default(Vector3), true);
		transform2.localScale = base.transform.localScale;
		this.action.AnimChangeState(JumperAction.StateEnum.Die3, 1f);
		string playerAtkName = this.playerAtkName;
		if (playerAtkName != null)
		{
			if (!(playerAtkName == "NewExecute1_2") && !(playerAtkName == "NewExecute2_2") && !(playerAtkName == "NewExecuteAir2_2"))
			{
				if (playerAtkName == "NewExecuteAir1_2")
				{
					Vector2 speed = new Vector2(Mathf.Sign(base.transform.localScale.x) * 2f, 15f);
					Vector2 speed2 = new Vector2(Mathf.Sign(base.transform.localScale.x) * -2f, 15f);
					transform.GetComponent<JumperPart>().Init(isGround, speed);
					transform2.GetComponent<JumperPart>().Init(isGround, speed2);
				}
			}
			else
			{
				Vector2 speed = new Vector2(Mathf.Sign(base.transform.localScale.x) * 2f, 15f);
				Vector2 speed2 = new Vector2(Mathf.Sign(base.transform.localScale.x) * -2f, -5f);
				transform.GetComponent<JumperPart>().Init(isGround, speed);
				transform2.GetComponent<JumperPart>().Init(isGround, speed2);
			}
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
				this.action.AnimChangeState(this.flyToFallAnimName, 1f);
			}
			if (deadFall && this.eAttr.isOnGround)
			{
				deadFall = false;
				this.action.AnimChangeState(this.airDieHitGroundAnimName, 1f);
				base.Invoke("RealDie", 0.5f);
			}
			yield return null;
		}
		yield break;
	}

	private void RealDie()
	{
		base.GetComponent<JumperAnimListener>().GetUp();
	}

	private JumperAnimListener _jumper;

	private Vector2? _atkFollowPos;

	private float _atkFollowTime;

	private float _atkFollowEnd;

	[SerializeField]
	private SkeletonAnimation closeToPlayer;
}
