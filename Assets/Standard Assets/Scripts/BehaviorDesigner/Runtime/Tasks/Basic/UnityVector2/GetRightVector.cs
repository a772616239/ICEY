using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Stores the right vector value.")]
	public class GetRightVector : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.right;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult = Vector2.zero;
		}

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
