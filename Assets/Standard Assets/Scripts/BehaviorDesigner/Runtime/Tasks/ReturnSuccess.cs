using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("The return success task will always return success except when the child task is running.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=39")]
	[TaskIcon("{SkinColor}ReturnSuccessIcon.png")]
	public class ReturnSuccess : Decorator
	{
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Inactive || this.executionStatus == TaskStatus.Running;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		public override TaskStatus Decorate(TaskStatus status)
		{
			if (status == TaskStatus.Failure)
			{
				return TaskStatus.Success;
			}
			return status;
		}

		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		private TaskStatus executionStatus;
	}
}
