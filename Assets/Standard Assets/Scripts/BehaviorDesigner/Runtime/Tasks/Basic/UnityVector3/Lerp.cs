using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Lerp the Vector3 by an amount.")]
	public class Lerp : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Lerp(this.fromVector3.Value, this.toVector3.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromVector3 = (this.toVector3 = (this.storeResult = Vector3.zero));
			this.lerpAmount = 0f;
		}

		[Tooltip("The from value")]
		public SharedVector3 fromVector3;

		[Tooltip("The to value")]
		public SharedVector3 toVector3;

		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
