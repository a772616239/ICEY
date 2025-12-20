using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedGameObject variable to the specified object. Returns Success.")]
	public class SetSharedGameObject : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = ((!(this.targetValue.Value != null) && !this.valueCanBeNull.Value) ? this.gameObject : this.targetValue.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.valueCanBeNull = false;
			this.targetValue = null;
			this.targetVariable = null;
		}

		[Tooltip("The value to set the SharedGameObject to. If null the variable will be set to the current GameObject")]
		public SharedGameObject targetValue;

		[RequiredField]
		[Tooltip("The SharedGameObject to set")]
		public SharedGameObject targetVariable;

		[Tooltip("Can the target value be null?")]
		public SharedBool valueCanBeNull;
	}
}
