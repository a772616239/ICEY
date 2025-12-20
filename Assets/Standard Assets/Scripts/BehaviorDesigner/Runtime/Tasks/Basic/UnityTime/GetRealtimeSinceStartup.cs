using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskCategory("Basic/Time")]
	[TaskDescription("Returns the real time in seconds since the game started.")]
	public class GetRealtimeSinceStartup : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.realtimeSinceStartup;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult.Value = 0f;
		}

		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
