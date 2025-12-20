using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the angle in degrees between two rotations.")]
	public class Angle : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Angle(this.firstRotation.Value, this.secondRotation.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.firstRotation = (this.secondRotation = Quaternion.identity);
			this.storeResult = 0f;
		}

		[Tooltip("The first rotation")]
		public SharedQuaternion firstRotation;

		[Tooltip("The second rotation")]
		public SharedQuaternion secondRotation;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
