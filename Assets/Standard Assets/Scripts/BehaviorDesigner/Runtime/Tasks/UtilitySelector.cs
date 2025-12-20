using System;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("The utility selector task evaluates the child tasks using Utility Theory AI. The child task can override the GetUtility method and return the utility value at that particular time. The task with the highest utility value will be selected and the existing running task will be aborted. The utility selector task reevaluates its children every tick.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=109")]
	[TaskIcon("{SkinColor}UtilitySelectorIcon.png")]
	public class UtilitySelector : Composite
	{
		public override void OnStart()
		{
			this.highestUtility = float.MinValue;
			this.availableChildren.Clear();
			for (int i = 0; i < this.children.Count; i++)
			{
				float utility = this.children[i].GetUtility();
				if (utility > this.highestUtility)
				{
					this.highestUtility = utility;
					this.currentChildIndex = i;
				}
				this.availableChildren.Add(i);
			}
		}

		public override int CurrentChildIndex()
		{
			return this.currentChildIndex;
		}

		public override void OnChildStarted(int childIndex)
		{
			this.executionStatus = TaskStatus.Running;
		}

		public override bool CanExecute()
		{
			return this.executionStatus != TaskStatus.Success && this.executionStatus != TaskStatus.Running && !this.reevaluating && this.availableChildren.Count > 0;
		}

		public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
		{
			if (childStatus != TaskStatus.Inactive && childStatus != TaskStatus.Running)
			{
				this.executionStatus = childStatus;
				if (this.executionStatus == TaskStatus.Failure)
				{
					this.availableChildren.Remove(childIndex);
					this.highestUtility = float.MinValue;
					for (int i = 0; i < this.availableChildren.Count; i++)
					{
						float utility = this.children[this.availableChildren[i]].GetUtility();
						if (utility > this.highestUtility)
						{
							this.highestUtility = utility;
							this.currentChildIndex = this.availableChildren[i];
						}
					}
				}
			}
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

		public override TaskStatus OverrideStatus(TaskStatus status)
		{
			return this.executionStatus;
		}

		public override bool CanRunParallelChildren()
		{
			return true;
		}

		public override bool CanReevaluate()
		{
			return true;
		}

		public override bool OnReevaluationStarted()
		{
			if (this.executionStatus == TaskStatus.Inactive)
			{
				return false;
			}
			this.reevaluating = true;
			return true;
		}

		public override void OnReevaluationEnded(TaskStatus status)
		{
			this.reevaluating = false;
			int num = this.currentChildIndex;
			this.highestUtility = float.MinValue;
			for (int i = 0; i < this.availableChildren.Count; i++)
			{
				float utility = this.children[this.availableChildren[i]].GetUtility();
				if (utility > this.highestUtility)
				{
					this.highestUtility = utility;
					this.currentChildIndex = this.availableChildren[i];
				}
			}
			if (num != this.currentChildIndex)
			{
				BehaviorManager.instance.Interrupt(base.Owner, this.children[num], this);
				this.executionStatus = TaskStatus.Inactive;
			}
		}

		private int currentChildIndex;

		private float highestUtility;

		private TaskStatus executionStatus;

		private bool reevaluating;

		private List<int> availableChildren = new List<int>();
	}
}
