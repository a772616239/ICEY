using System;
using BehaviorDesigner.Runtime.Tasks;

public class JumperFooterAIConditional
{
	[TaskCategory("Enemy/JumperFooter")]
	[TaskDescription("判断抓取成功")]
	public class CatchJudge : Conditional
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<JumperFooterAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.Atk2Result)
			{
				return TaskStatus.Failure;
			}
			this._action.Atk2Result = false;
			return TaskStatus.Success;
		}

		private JumperFooterAction _action;
	}
}
