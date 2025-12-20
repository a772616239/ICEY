using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=25")]
	[TaskIcon("{SkinColor}SequenceIcon.png")]
	public class Sequence : Composite
	{
		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count && this.executionStatus != TaskStatus.Failure;
		}

		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.currentChildIndex++;
			this.executionStatus = childStatus;
		}

		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = childIndex;
			this.executionStatus = TaskStatus.Inactive;
		}

		public override void OnEnd()
		{
			this.executionStatus = TaskStatus.Inactive;
			this.currentChildIndex = 0;
		}

		private int currentChildIndex;

		private TaskStatus executionStatus;
	}
}
