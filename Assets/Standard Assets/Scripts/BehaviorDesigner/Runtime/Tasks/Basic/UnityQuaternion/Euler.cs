using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the quaternion of a euler vector.")]
	public class Euler : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Euler(this.eulerVector.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.eulerVector = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		[Tooltip("The euler vector")]
		public SharedVector3 eulerVector;

		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
