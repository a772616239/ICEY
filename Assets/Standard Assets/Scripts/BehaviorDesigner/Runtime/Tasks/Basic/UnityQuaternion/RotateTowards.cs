using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the quaternion after a rotation.")]
	public class RotateTowards : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.RotateTowards(this.fromQuaternion.Value, this.toQuaternion.Value, this.maxDeltaDegrees.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromQuaternion = (this.toQuaternion = (this.storeResult = Quaternion.identity));
			this.maxDeltaDegrees = 0f;
		}

		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		[Tooltip("The maximum degrees delta")]
		public SharedFloat maxDeltaDegrees;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
