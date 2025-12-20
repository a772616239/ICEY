using System;
using System.Collections;
using UnityEngine;

public class AirRobotHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "NoStiff";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.妖姬];
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(UnityEngine.Random.Range(90, 92), new Vector3?(base.transform.position));
		}
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		Vector2 vector = new Vector2((float)(-3 * this.eAttr.faceDir), 0f);
		base.HitIntoWeakState(vector, vector, "NoStiff", "NoStiff");
	}

	protected override void ExecuteFollow()
	{
	}

	protected override void ExecuteDie()
	{
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.EnemyDie();
		this.action.AnimChangeState(AirRobotAction.StateEnum.HitFall, 1f);
		base.Invoke("DieFall", 0.36f);
		base.StartCoroutine(this.DeathIEnumerator());
		base.DieTimeControl();
		base.NormalKill();
	}

	protected override IEnumerator DeathIEnumerator()
	{
		bool deadFall = true;
		while (this.eAttr.isDead)
		{
			if (deadFall && this.action.stateMachine.currentState == "Fall" && this.eAttr.isOnGround)
			{
				deadFall = false;
				this.action.AnimChangeState(AirRobotAction.StateEnum.HitGround, 1f);
				base.Invoke("RealDie", 0.5f);
			}
			yield return null;
		}
		yield break;
	}

	private void DieFall()
	{
		this.action.AnimChangeState(AirRobotAction.StateEnum.Hit2, 1f);
	}

	private void RealDie()
	{
		base.GetComponent<AirRobotAnimListener>().GetUp();
	}
}
