using System;
using UnityEngine;

public class TrinityHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "NoStiff";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.崔妮蒂];
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	protected override void SpAttack()
	{
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
		R.Trophy.AwardTrophy(31);
		base.EnemyDie();
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(TrinityAction.StateEnum.ExecuteDie, 1f);
	}
}
