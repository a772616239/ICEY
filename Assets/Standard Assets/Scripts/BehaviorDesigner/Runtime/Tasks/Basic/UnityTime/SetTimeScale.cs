using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTime
{
	[TaskCategory("Basic/Time")]
	[TaskDescription("Sets the scale at which time is passing.")]
	public class SetTimeScale : Action
	{
		public override TaskStatus OnUpdate()
		{
			Time.timeScale = this.timeScale.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.timeScale.Value = 0f;
		}

		[Tooltip("The timescale")]
		public SharedFloat timeScale;
	}
}
