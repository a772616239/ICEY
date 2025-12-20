using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("The until success task will keep executing its child task until the child task returns success.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=42")]
	[TaskIcon("{SkinColor}UntilSuccessIcon.png")]
	public class UntilSuccess : Decorator
	{
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Failure || this.executionStatus == TaskStatus.Inactive;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionStatus = childStatus;
		}

		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
		}

		private TaskStatus executionStatus;
	}
}
