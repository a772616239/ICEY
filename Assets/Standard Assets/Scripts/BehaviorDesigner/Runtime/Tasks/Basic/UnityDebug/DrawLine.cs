using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityDebug
{
	[TaskCategory("Basic/Debug")]
	[TaskDescription("Draws a debug line")]
	public class DrawLine : Action
	{
		public override TaskStatus OnUpdate()
		{
			UnityEngine.Debug.DrawLine(this.start.Value, this.end.Value, this.color.Value, this.duration.Value, this.depthTest.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.start = Vector3.zero;
			this.end = Vector3.zero;
			this.color = Color.white;
			this.duration = 0f;
			this.depthTest = true;
		}

		[Tooltip("The start position")]
		public SharedVector3 start;

		[Tooltip("The end position")]
		public SharedVector3 end;

		[Tooltip("The color")]
		public SharedColor color = Color.white;

		[Tooltip("Duration the line will be visible for in seconds.\nDefault: 0 means 1 frame.")]
		public SharedFloat duration;

		[Tooltip("Whether the line should show through world geometry.")]
		public SharedBool depthTest = true;
	}
}
