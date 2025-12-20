using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedVector3 : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!this.variable.Value.Equals(this.compareTo.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = Vector3.zero;
			this.compareTo = Vector3.zero;
		}

		[Tooltip("The first variable to compare")]
		public SharedVector3 variable;

		[Tooltip("The variable to compare to")]
		public SharedVector3 compareTo;
	}
}
