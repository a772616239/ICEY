using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskCategory("Basic/Time")]
	[TaskDescription("Returns the scale at which time is passing.")]
	public class GetTimeScale : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.timeScale;
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
