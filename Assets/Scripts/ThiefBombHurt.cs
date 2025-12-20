using System;
using UnityEngine;

public class ThiefBombHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.背弹者];
	}

	protected override void PlayHurtAudio()
	{
		int id = UnityEngine.Random.Range(251, 253);
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		normalAtkType = "NoStiff";
		airAtkType = "NoStiff";
		base.PhysicAndEffect(speed, airSpeed, normalAtkType, airAtkType);
	}

	public override void EnemyDie()
	{
		this.deadFlag = true;
		this.action.AnimChangeState(ThiefBombAction.StateEnum.Bomb, 1f);
	}
}
