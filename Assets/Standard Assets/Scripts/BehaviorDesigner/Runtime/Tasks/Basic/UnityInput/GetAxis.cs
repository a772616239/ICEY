using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Stores the value of the specified axis and stores it in a float.")]
	public class GetAxis : Action
	{
		public override TaskStatus OnUpdate()
		{
			float num = UnityEngine.Input.GetAxis(this.axisName.Value);
			if (!this.multiplier.IsNone)
			{
				num *= this.multiplier.Value;
			}
			this.storeResult.Value = num;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.axisName = string.Empty;
			this.multiplier = 1f;
			this.storeResult = 0f;
		}

		[Tooltip("The name of the axis")]
		public SharedString axisName;

		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
		public SharedFloat multiplier;

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedFloat storeResult;
	}
}
