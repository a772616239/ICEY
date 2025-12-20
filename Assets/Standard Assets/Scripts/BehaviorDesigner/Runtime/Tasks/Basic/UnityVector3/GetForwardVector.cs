using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Stores the forward vector value.")]
	public class GetForwardVector : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.forward;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult = Vector3.zero;
		}

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
