using System;
using BehaviorDesigner.Runtime.Tasks;

public class BombKillerAIConditional
{
	[TaskDescription("判断血量")]
	[TaskCategory("Enemy/BombKiller")]
	public class HpJudge : Conditional
	{
		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.eAttr.currentHp < this.eAttr.maxHp * this.percent / 100)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		public int percent;

		private EnemyAttribute eAttr;
	}
}
