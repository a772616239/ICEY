using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedFloat : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!this.variable.Value.Equals(this.compareTo.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = 0f;
			this.compareTo = 0f;
		}

		[Tooltip("The first variable to compare")]
		public SharedFloat variable;

		[Tooltip("The variable to compare to")]
		public SharedFloat compareTo;
	}
}
