using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Stores the right vector value.")]
	public class GetRightVector : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.right;
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
