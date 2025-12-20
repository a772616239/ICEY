using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores a rotation which rotates from the first direction to the second.")]
	public class FromToRotation : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.FromToRotation(this.fromDirection.Value, this.toDirection.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromDirection = (this.toDirection = Vector3.zero);
			this.storeResult = Quaternion.identity;
		}

		[Tooltip("The from rotation")]
		public SharedVector3 fromDirection;

		[Tooltip("The to rotation")]
		public SharedVector3 toDirection;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
