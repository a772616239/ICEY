using System;
using BehaviorDesigner.Runtime.Tasks;

public class EatingBossAIConditional
{
	[TaskCategory("Enemy/Carlos")]
	[TaskDescription("召唤判定")]
	public class SummonJudge : Conditional
	{
		private int success
		{
			get
			{
				return (R.GameData.Difficulty > 1) ? 50 : 20;
			}
		}

		public override void OnAwake()
		{
			this.action = base.GetComponent<EatingBossAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.summonRate > (float)this.success)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EatingBossAction action;
	}

	[TaskCategory("Enemy/Carlos")]
	[TaskDescription("用大招")]
	public class UseAtk5 : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EatingBossAction>();
			this.anim = base.GetComponent<EatingBossAnimEvent>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.useAtk5)
			{
				this.action.useAtk5 = false;
				this.anim.atk5Loop = ((R.GameData.Difficulty > 1) ? 8 : 5);
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EatingBossAction action;

		private EatingBossAnimEvent anim;
	}
}
