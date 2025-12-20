using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Clamps the float between two values.")]
	public class FloatClamp : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.floatVariable.Value = Mathf.Clamp(this.floatVariable.Value, this.minValue.Value, this.maxValue.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.floatVariable = (this.minValue = (this.maxValue = 0f));
		}

		[Tooltip("The float to clamp")]
		public SharedFloat floatVariable;

		[Tooltip("The maximum value of the float")]
		public SharedFloat minValue;

		[Tooltip("The maximum value of the float")]
		public SharedFloat maxValue;
	}
}
