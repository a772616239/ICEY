using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityInput
{
	[TaskCategory("Basic/Input")]
	[TaskDescription("Returns success when the specified button is pressed.")]
	public class IsButtonDown : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!Input.GetButtonDown(this.buttonName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.buttonName = "Fire1";
		}

		[Tooltip("The name of the button")]
		public SharedString buttonName;
	}
}
