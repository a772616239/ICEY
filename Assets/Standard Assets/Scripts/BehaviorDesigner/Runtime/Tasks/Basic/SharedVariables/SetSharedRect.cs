using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedRect variable to the specified object. Returns Success.")]
	public class SetSharedRect : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetValue = default(Rect);
			this.targetVariable = default(Rect);
		}

		[Tooltip("The value to set the SharedRect to")]
		public SharedRect targetValue;

		[RequiredField]
		[Tooltip("The SharedRect to set")]
		public SharedRect targetVariable;
	}
}
