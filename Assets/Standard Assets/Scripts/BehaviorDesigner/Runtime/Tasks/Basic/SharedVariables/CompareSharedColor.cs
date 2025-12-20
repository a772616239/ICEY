using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedColor : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!this.variable.Value.Equals(this.compareTo.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = Color.black;
			this.compareTo = Color.black;
		}

		[Tooltip("The first variable to compare")]
		public SharedColor variable;

		[Tooltip("The variable to compare to")]
		public SharedColor compareTo;
	}
}
