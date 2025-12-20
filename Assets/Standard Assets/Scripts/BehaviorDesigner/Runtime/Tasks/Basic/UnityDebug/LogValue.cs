using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityDebug
{
	[TaskCategory("Basic/Debug")]
	[TaskDescription("Log a variable value.")]
	public class LogValue : Action
	{
		public override TaskStatus OnUpdate()
		{
			UnityEngine.Debug.Log(this.variable.Value.value.GetValue());
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.variable = null;
		}

		[Tooltip("The variable to output")]
		public SharedGenericVariable variable;
	}
}
