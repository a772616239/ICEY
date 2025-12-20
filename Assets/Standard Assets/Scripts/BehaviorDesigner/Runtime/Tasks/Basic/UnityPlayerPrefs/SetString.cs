using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	public class SetString : Action
	{
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetString(this.key.Value, this.value.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = string.Empty;
			this.value = string.Empty;
		}

		[Tooltip("The key to store")]
		public SharedString key;

		[Tooltip("The value to set")]
		public SharedString value;
	}
}
