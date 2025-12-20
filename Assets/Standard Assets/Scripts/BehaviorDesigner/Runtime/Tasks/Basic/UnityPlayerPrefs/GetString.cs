using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Stores the value with the specified key from the PlayerPrefs.")]
	public class GetString : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = PlayerPrefs.GetString(this.key.Value, this.defaultValue.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = string.Empty;
			this.defaultValue = string.Empty;
			this.storeResult = string.Empty;
		}

		[Tooltip("The key to store")]
		public SharedString key;

		[Tooltip("The default value")]
		public SharedString defaultValue;

		[Tooltip("The value retrieved from the PlayerPrefs")]
		[RequiredField]
		public SharedString storeResult;
	}
}
