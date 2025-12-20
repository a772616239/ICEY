using System;
using BehaviorDesigner.Runtime.Tasks;

public class JumperAIConditional
{
	[TaskCategory("Enemy/Jumper")]
	[TaskDescription("判断抓取成功")]
	public class CatchJudge : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JumperAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.catchResult)
			{
				this.action.catchResult = false;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private JumperAction action;
	}
}
