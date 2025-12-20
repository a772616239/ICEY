using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Lerps between two quaternions.")]
	public class Lerp : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Lerp(this.fromQuaternion.Value, this.toQuaternion.Value, this.amount.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.fromQuaternion = (this.toQuaternion = (this.storeResult = Quaternion.identity));
			this.amount = 0f;
		}

		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		[Tooltip("The amount to lerp")]
		public SharedFloat amount;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
