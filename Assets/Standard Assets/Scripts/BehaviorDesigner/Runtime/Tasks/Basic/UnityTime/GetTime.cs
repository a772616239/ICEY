using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskCategory("Basic/Time")]
	[TaskDescription("Returns the time in second since the start of the game.")]
	public class GetTime : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Time.time;
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
