using System;
using BehaviorDesigner.Runtime.Tasks;

public class JudgesAIConditional
{
	[TaskCategory("Enemy/Judges")]
	[TaskDescription("判断攻击类型")]
	public class AttackTypeJudge : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JudgesAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.attackType == this.type)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		public JudgesAction.AttackType type;

		private JudgesAction action;
	}

	[TaskCategory("Enemy/Judges")]
	[TaskDescription("判断怪物在最终阶段")]
	public class JudgesPhaseJudge : Conditional
	{
		public override void OnAwake()
		{
			this.hurt = base.GetComponent<JudgesHurt>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.hurt.currentPhase == 4)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private JudgesHurt hurt;
	}

	[TaskCategory("Enemy/Judges")]
	[TaskDescription("召唤判定")]
	public class SummonJudge : Conditional
	{
		private int success
		{
			get
			{
				return (R.GameData.Difficulty > 1) ? 30 : 40;
			}
		}

		public override void OnAwake()
		{
			this.action = base.GetComponent<JudgesAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.summonRate > (float)this.success)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private JudgesAction action;
	}
}
