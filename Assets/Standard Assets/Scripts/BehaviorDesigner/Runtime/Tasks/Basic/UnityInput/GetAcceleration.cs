using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Stores the acceleration value.")]
	public class GetAcceleration : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Input.acceleration;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector3 storeResult;
	}
}
