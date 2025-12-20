using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the dot product between two rotations.")]
	public class Dot : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Dot(this.leftRotation.Value, this.rightRotation.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.leftRotation = (this.rightRotation = Quaternion.identity);
			this.storeResult = 0f;
		}

		[Tooltip("The first rotation")]
		public SharedQuaternion leftRotation;

		[Tooltip("The second rotation")]
		public SharedQuaternion rightRotation;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
