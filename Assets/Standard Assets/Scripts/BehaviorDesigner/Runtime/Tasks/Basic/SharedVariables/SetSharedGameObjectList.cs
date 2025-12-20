using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedGameObjectList variable to the specified object. Returns Success.")]
	public class SetSharedGameObjectList : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		[Tooltip("The value to set the SharedGameObjectList to.")]
		public SharedGameObjectList targetValue;

		[RequiredField]
		[Tooltip("The SharedGameObjectList to set")]
		public SharedGameObjectList targetVariable;
	}
}
