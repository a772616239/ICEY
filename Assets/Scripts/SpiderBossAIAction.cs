using System;
using BehaviorDesigner.Runtime.Tasks;
using ExtensionMethods;

public class SpiderBossAIAction
{
	[TaskCategory("Enemy/SpiderBoss")]
	[TaskDescription("跳跃")]
	public class SpiderJump : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<SpiderBossAction>();
		}

		public override void OnStart()
		{
			this._action.Jump(this.back);
		}

		public override TaskStatus OnUpdate()
		{
			return (!this._action.stateMachine.currentState.IsInArray(SpiderBossAction.JumpSta)) ? TaskStatus.Success : TaskStatus.Running;
		}

		private SpiderBossAction _action;

		public bool back;
	}
}
