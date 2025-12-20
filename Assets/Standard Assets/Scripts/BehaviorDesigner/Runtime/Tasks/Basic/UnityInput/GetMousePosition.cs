using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Stores the mouse position.")]
	public class GetMousePosition : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = UnityEngine.Input.mousePosition;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult = Vector2.zero;
		}

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedVector2 storeResult;
	}
}
