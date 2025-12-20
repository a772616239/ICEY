using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Lerp the Vector2 by an amount.")]
	public class Lerp : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.Lerp(this.fromVector2.Value, this.toVector2.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromVector2 = (this.toVector2 = (this.storeResult = Vector2.zero));
			this.lerpAmount = 0f;
		}

		[Tooltip("The from value")]
		public SharedVector2 fromVector2;

		[Tooltip("The to value")]
		public SharedVector2 toVector2;

		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
