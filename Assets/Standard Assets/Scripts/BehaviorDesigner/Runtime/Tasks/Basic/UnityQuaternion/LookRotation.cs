using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the quaternion of a forward vector.")]
	public class LookRotation : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.LookRotation(this.forwardVector.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.forwardVector = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		[Tooltip("The forward vector")]
		public SharedVector3 forwardVector;

		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
