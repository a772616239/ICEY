using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Retruns success if the specified key exists.")]
	public class HasKey : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (!PlayerPrefs.HasKey(this.key.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.key = string.Empty;
		}

		[Tooltip("The key to check")]
		public SharedString key;
	}
}
