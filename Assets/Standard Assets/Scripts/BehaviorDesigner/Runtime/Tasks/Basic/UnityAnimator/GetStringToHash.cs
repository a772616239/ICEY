using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Converts the state name to its corresponding hash code. Returns Success.")]
	public class GetStringToHash : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = Animator.StringToHash(this.stateName.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.stateName = string.Empty;
			this.storeValue = 0;
		}

		[Tooltip("The name of the state to convert to a hash code")]
		public SharedString stateName;

		[Tooltip("The hash value")]
		[RequiredField]
		public SharedInt storeValue;
	}
}
