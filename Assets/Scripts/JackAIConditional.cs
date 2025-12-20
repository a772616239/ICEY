using System;
using BehaviorDesigner.Runtime.Tasks;

public class JackAIConditional
{
	[TaskCategory("Enemy/Jack")]
	[TaskDescription("用大招")]
	public class UseFullScreenShoot : Conditional
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<JackAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.UseAtk5)
			{
				return TaskStatus.Failure;
			}
			this._action.UseAtk5 = false;
			return TaskStatus.Success;
		}

		private JackAction _action;
	}
}
