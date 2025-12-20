using System;
using BehaviorDesigner.Runtime.Tasks;

public class SpiderBossAIConditional
{
	[TaskCategory("Enemy/SpiderBoss")]
	[TaskDescription("召唤判定")]
	public class SummonJudge : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<SpiderBossAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.summonRate >= 10f)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private SpiderBossAction action;
	}
}
