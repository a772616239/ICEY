using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedQuaternion variable to the specified object. Returns Success.")]
	public class SetSharedQuaternion : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetValue = Quaternion.identity;
			this.targetVariable = Quaternion.identity;
		}

		[Tooltip("The value to set the SharedQuaternion to")]
		public SharedQuaternion targetValue;

		[RequiredField]
		[Tooltip("The SharedQuaternion to set")]
		public SharedQuaternion targetVariable;
	}
}
