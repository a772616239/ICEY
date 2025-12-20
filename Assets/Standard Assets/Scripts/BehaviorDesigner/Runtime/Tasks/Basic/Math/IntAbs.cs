using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Stores the absolute value of the int.")]
	public class IntAbs : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.intVariable.Value = Mathf.Abs(this.intVariable.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.intVariable = 0;
		}

		[Tooltip("The int to return the absolute value of")]
		public SharedInt intVariable;
	}
}
