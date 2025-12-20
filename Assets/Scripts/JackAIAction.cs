using System;
using BehaviorDesigner.Runtime.Tasks;

public class JackAIAction
{
	[TaskCategory("Enemy/Jack")]
	[TaskDescription("闪避")]
	public class JackSideStep : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<JackAction>();
		}

		public override void OnStart()
		{
			this._action.SideStep();
		}

		public override TaskStatus OnUpdate()
		{
			return (!(this._action.stateMachine.currentState == "MoveAway")) ? TaskStatus.Success : TaskStatus.Running;
		}

		private JackAction _action;
	}

	[TaskCategory("Enemy/Jack")]
	[TaskDescription("杰克跳跃")]
	public class JackJump : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<JackAction>();
		}

		public override void OnStart()
		{
			this._action.Jump();
		}

		public override TaskStatus OnUpdate()
		{
			return (!(this._action.stateMachine.currentState == "Jump")) ? TaskStatus.Success : TaskStatus.Running;
		}

		private JackAction _action;
	}
}
