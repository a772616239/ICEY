using System;
using BehaviorDesigner.Runtime.Tasks;

public class DahalFAIConditional : BaseBehaviour
{
	[TaskCategory("Enemy/DahalF")]
	[TaskDescription("用大招")]
	public class UseFullScreenShoot : Conditional
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<DahalFAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.UseAtk2)
			{
				return TaskStatus.Failure;
			}
			this._action.UseAtk2 = false;
			return TaskStatus.Success;
		}

		private DahalFAction _action;
	}
}
