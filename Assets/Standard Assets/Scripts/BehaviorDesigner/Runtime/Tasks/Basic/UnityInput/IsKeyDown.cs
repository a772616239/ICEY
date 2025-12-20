using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Returns success when the specified key is pressed.")]
	public class IsKeyDown : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!Input.GetKeyDown(this.key)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = KeyCode.None;
		}

		[Tooltip("The key to test")]
		public KeyCode key;
	}
}
