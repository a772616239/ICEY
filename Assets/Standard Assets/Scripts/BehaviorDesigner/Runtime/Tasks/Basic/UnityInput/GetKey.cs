using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Stores the pressed state of the specified key.")]
	public class GetKey : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = UnityEngine.Input.GetKey(this.key);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = KeyCode.None;
			this.storeResult = false;
		}

		[Tooltip("The key to test.")]
		public KeyCode key;

		[RequiredField]
		[Tooltip("The stored result")]
		public SharedBool storeResult;
	}
}
