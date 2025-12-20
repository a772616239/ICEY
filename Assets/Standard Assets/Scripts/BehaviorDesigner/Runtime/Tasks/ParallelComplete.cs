using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Similar to the parallel selector task, except the parallel complete task will return the child status as soon as the child returns success or failure.The child tasks are executed simultaneously.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=136")]
	[TaskIcon("{SkinColor}ParallelCompleteIcon.png")]
	public class ParallelComplete : Composite
	{
		public override void OnAwake()
		{
			this.executionStatus = new TaskStatus[this.children.Count];
		}

		public override void OnChildStarted(int childIndex)
		{
			this.currentChildIndex++;
			this.executionStatus[childIndex] = TaskStatus.Running;
		}

		public override bool CanRunParallelChildren()
		{
			return true;
		}

		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		public override bool CanExecute()
		{
			return this.currentChildIndex < this.children.Count;
		}

		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			this.executionStatus[childIndex] = childStatus;
		}

		public override void OnConditionalAbort(int childIndex)
		{
			this.currentChildIndex = 0;
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
		}

		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				if (this.executionStatus[i] == TaskStatus.Success || this.executionStatus[i] == TaskStatus.Failure)
				{
					return this.executionStatus[i];
				}
				if (this.executionStatus[i] == TaskStatus.Inactive)
				{
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Running;
		}

		public override void OnEnd()
		{
			for (int i = 0; i < this.executionStatus.Length; i++)
			{
				this.executionStatus[i] = TaskStatus.Inactive;
			}
			this.currentChildIndex = 0;
		}

		private int currentChildIndex;

		private TaskStatus[] executionStatus;
	}
}
