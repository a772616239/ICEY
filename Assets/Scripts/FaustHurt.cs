using System;
using GameWorld;
using UnityEngine;

public class FaustHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "NoStiff";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.黄衣老者];
	}

	protected override void SpAttack()
	{
	}

	protected override void HpMinus(int num)
	{
		base.HpMinus(1);
	}

	public override void EnemyDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.EnemyDie();
		this.SetHitSpeed(Vector2.zero);
		this.action.AnimChangeState(FaustAction.StateEnum.Die, 1f);
	}

	public override void NormalHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, int atkId, HurtCheck.BodyType body, Vector2 hurtPos)
	{
		base.NormalHurt(atkData, atkId, body, hurtPos);
		this.action.AnimChangeState(FaustAction.StateEnum.DisAppear, 1f);
	}
}
