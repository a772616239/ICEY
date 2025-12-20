using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskCategory("Basic/String")]
	[TaskDescription("Sets the variable string to the value string.")]
	public class SetString : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.variable.Value = this.value.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = string.Empty;
			this.value = string.Empty;
		}

		[Tooltip("The target string")]
		[RequiredField]
		public SharedString variable;

		[Tooltip("The value string")]
		public SharedString value;
	}
}
