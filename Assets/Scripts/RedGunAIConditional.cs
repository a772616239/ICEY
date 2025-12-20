using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RedGunAIConditional
{
	[TaskCategory("Enemy/RedGun")]
	[TaskDescription("在主角两个身位外")]
	public class InAttackRange : Conditional
	{
		private Transform player
		{
			get
			{
				return R.Player.Transform;
			}
		}

		public override void OnAwake()
		{
			this.ai = base.GetComponent<EnemyAIAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			EnemyAIAttribute.ExpectationParam expectationParam = this.ai.attackExpectations[0];
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num > expectationParam.distance + expectationParam.range)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute ai;
	}
}
