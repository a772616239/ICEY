using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskCategory("Basic/Time")]
	[TaskDescription("Returns the time in seconds it took to complete the last frame.")]
	public class GetDeltaTime : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.deltaTime;
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
