using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	public class SetInt : Action
	{
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetInt(this.key.Value, this.value.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = string.Empty;
			this.value = 0;
		}

		[Tooltip("The key to store")]
		public SharedString key;

		[Tooltip("The value to set")]
		public SharedInt value;
	}
}
