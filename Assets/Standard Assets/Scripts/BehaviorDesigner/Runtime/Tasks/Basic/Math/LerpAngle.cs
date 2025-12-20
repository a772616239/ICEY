using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Lerp the angle by an amount.")]
	public class LerpAngle : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Mathf.LerpAngle(this.fromValue.Value, this.toValue.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromValue = (this.toValue = (this.lerpAmount = (this.storeResult = 0f)));
		}

		[Tooltip("The from value")]
		public SharedFloat fromValue;

		[Tooltip("The to value")]
		public SharedFloat toValue;

		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
