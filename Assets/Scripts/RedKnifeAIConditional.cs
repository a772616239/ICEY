using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RedKnifeAIConditional
{
	[TaskCategory("Enemy/RedKnife")]
	[TaskDescription("在跳劈距离外")]
	public class OutJumpAtkRange : Conditional
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
			this.jumpAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[2];
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num > this.jumpAtk.range + this.jumpAtk.distance)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute.ExpectationParam jumpAtk;
	}

	[TaskCategory("Enemy/RedKnife")]
	[TaskDescription("砍击到跳劈距离")]
	public class OutCutAtkRange : Conditional
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
			this.jumpAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[2];
			this.cutAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[1];
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num <= this.jumpAtk.range + this.jumpAtk.distance && num > this.cutAtk.distance + this.cutAtk.range)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute.ExpectationParam jumpAtk;

		private EnemyAIAttribute.ExpectationParam cutAtk;
	}

	[TaskDescription("脚踢到砍击距离")]
	[TaskCategory("Enemy/RedKnife")]
	public class OutKickAtkRange : Conditional
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
			this.kickAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[0];
			this.cutAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[1];
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num <= this.cutAtk.range + this.cutAtk.distance && num > this.kickAtk.distance + this.kickAtk.range)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute.ExpectationParam cutAtk;

		private EnemyAIAttribute.ExpectationParam kickAtk;
	}

	[TaskCategory("Enemy/RedKnife")]
	[TaskDescription("脚踢距离内")]
	public class WithinKickAtkRange : Conditional
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
			this.kickAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[0];
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num <= this.kickAtk.distance + this.kickAtk.range)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute.ExpectationParam kickAtk;
	}
}
