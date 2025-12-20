using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedVector4 : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!this.variable.Value.Equals(this.compareTo.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = Vector4.zero;
			this.compareTo = Vector4.zero;
		}

		[Tooltip("The first variable to compare")]
		public SharedVector4 variable;

		[Tooltip("The variable to compare to")]
		public SharedVector4 compareTo;
	}
}
