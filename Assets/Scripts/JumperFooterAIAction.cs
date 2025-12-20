using System;
using BehaviorDesigner.Runtime.Tasks;

public class JumperFooterAIAction
{
	[TaskDescription("防御")]
	[TaskCategory("Enemy/JumperFooter")]
	public class Defense : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JumperFooterAction>();
		}

		public override void OnStart()
		{
			this.action.Defence();
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		private JumperFooterAction action;
	}

	[TaskCategory("Enemy/JumperFooter")]
	[TaskDescription("跳")]
	public class Jump : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JumperFooterAction>();
		}

		public override void OnStart()
		{
			this.action.FaceToPlayer();
			this.action.Jump();
		}

		public override TaskStatus OnUpdate()
		{
			JumperFooterAction.StateEnum stateEnum = EnumTools.ToEnum<JumperFooterAction.StateEnum>(this.action.stateMachine.currentState, false);
			if (stateEnum == JumperFooterAction.StateEnum.Jump)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		private JumperFooterAction action;
	}
}
