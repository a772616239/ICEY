using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedGameObjectList : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			if (this.variable.Value == null && this.compareTo.Value != null)
			{
				return TaskStatus.Failure;
			}
			if (this.variable.Value == null && this.compareTo.Value == null)
			{
				return TaskStatus.Success;
			}
			if (this.variable.Value.Count != this.compareTo.Value.Count)
			{
				return TaskStatus.Failure;
			}
			for (int i = 0; i < this.variable.Value.Count; i++)
			{
				if (this.variable.Value[i] != this.compareTo.Value[i])
				{
					return TaskStatus.Failure;
				}
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = null;
			this.compareTo = null;
		}

		[Tooltip("The first variable to compare")]
		public SharedGameObjectList variable;

		[Tooltip("The variable to compare to")]
		public SharedGameObjectList compareTo;
	}
}
