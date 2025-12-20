using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Deletes the specified key from the PlayerPrefs.")]
	public class DeleteKey : Action
	{
		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.DeleteKey(this.key.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = string.Empty;
		}

		[Tooltip("The key to delete")]
		public SharedString key;
	}
}
