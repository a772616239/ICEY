using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityQuaternion
{
	[TaskCategory("Basic/Quaternion")]
	[TaskDescription("Stores the quaternion identity.")]
	public class Identity : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.identity;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult = Quaternion.identity;
		}

		[Tooltip("The identity")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
