using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the rotation which rotates the specified degrees around the specified axis.")]
	public class AngleAxis : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.AngleAxis(this.degrees.Value, this.axis.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.degrees = 0f;
			this.axis = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		[Tooltip("The number of degrees")]
		public SharedFloat degrees;

		[Tooltip("The axis direction")]
		public SharedVector3 axis;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
