using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("The until failure task will keep executing its child task until the child task returns failure.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=41")]
	[TaskIcon("{SkinColor}UntilFailureIcon.png")]
	public class UntilFailure : Decorator
	{
		public override bool CanExecute()
		{
			return this.executionStatus == TaskStatus.Success || this.executionStatus == TaskStatus.Inactive;
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
