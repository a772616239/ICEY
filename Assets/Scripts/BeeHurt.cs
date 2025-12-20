using System;
using UnityEngine;

public class BeeHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "Hit";
		this.defaultAirAnimName = "Hit";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.蜜蜂];
	}

	protected override void ExecuteFollow()
	{
		this.action.AnimChangeState(BeeAction.StateEnum.WeakMod, 1f);
		base.ExecuteFollow();
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.EnemyDie();
		base.NormalKill();
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(BeeAction.StateEnum.Die, 1f);
		base.Invoke("DieTimeControl", 0.12f);
	}

	protected override void ExecuteDie()
	{
		base.ExecuteDie();
		base.NormalKill();
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(BeeAction.StateEnum.Die, 1f);
		base.Invoke("DieTimeControl", 0.12f);
	}
}
