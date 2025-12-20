using System;
using BehaviorDesigner.Runtime.Tasks;

public class DahalAIConditional
{
	[TaskCategory("Enemy/Dahal")]
	[TaskDescription("判断达哈尔阶段")]
	public class DahalLevel : Conditional
	{
		public override void OnAwake()
		{
			this.hurt = base.GetComponent<DahalHurt>();
		}

		public override TaskStatus OnUpdate()
		{
			return (this.hurt.currentPhase != 2) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private DahalHurt hurt;
	}

	[TaskCategory("Enemy/Dahal")]
	[TaskDescription("判断使用三角扫射")]
	public class DahalAtk1Used : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<DahalAction>();
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.Atk1Used) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private DahalAction action;
	}

	[TaskCategory("Enemy/Dahal")]
	[TaskDescription("判断使用上天攻击")]
	public class DahalAtk11Used : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<DahalAction>();
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.Atk10Uesd) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private DahalAction action;
	}

	[TaskCategory("Enemy/Dahal")]
	[TaskDescription("判断使用地面连击")]
	public class DahalAtk8Used : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<DahalAction>();
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.Atk8Uesd) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private DahalAction action;
	}

	[TaskCategory("Enemy/Dahal")]
	[TaskDescription("Dahal一阶段随机")]
	public class DahalRandomState : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<DahalAction>();
		}

		public override TaskStatus OnUpdate()
		{
			bool flag = this.action.Atk10Uesd && this.action.Atk1Used && this.action.Atk8Uesd;
			return (!flag) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private DahalAction action;
	}
}
