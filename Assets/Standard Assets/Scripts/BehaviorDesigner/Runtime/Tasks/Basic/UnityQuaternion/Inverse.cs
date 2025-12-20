using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the inverse of the specified quaternion.")]
	public class Inverse : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Inverse(this.targetQuaternion.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetQuaternion = (this.storeResult = Quaternion.identity);
		}

		[Tooltip("The target quaternion")]
		public SharedQuaternion targetQuaternion;

		[Tooltip("The stored quaternion")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
