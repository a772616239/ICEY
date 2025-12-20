using System;
using BehaviorDesigner.Runtime.Tasks;
using ExtensionMethods;

public class SpiderAIAction
{
	[TaskDescription("蜘蛛跳跃")]
	[TaskCategory("Enemy/Spider")]
	public class SpiderJump : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<SpiderAction>();
		}

		public override void OnStart()
		{
			this.action.Jump(this.back);
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.stateMachine.currentState.IsInArray(SpiderAction.JumpSta)) ? TaskStatus.Success : TaskStatus.Running;
		}

		public bool back;

		private SpiderAction action;
	}
}
