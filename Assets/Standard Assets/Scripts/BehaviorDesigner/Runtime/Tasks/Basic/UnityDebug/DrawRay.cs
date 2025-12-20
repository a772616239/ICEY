using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityDebug
{
	[TaskCategory("Basic/Debug")]
	[TaskDescription("Draws a debug ray")]
	public class DrawRay : Action
	{
		public override TaskStatus OnUpdate()
		{
			UnityEngine.Debug.DrawRay(this.start.Value, this.direction.Value, this.color.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.start = Vector3.zero;
			this.direction = Vector3.zero;
			this.color = Color.white;
		}

		[Tooltip("The position")]
		public SharedVector3 start;

		[Tooltip("The direction")]
		public SharedVector3 direction;

		[Tooltip("The color")]
		public SharedColor color = Color.white;
	}
}
