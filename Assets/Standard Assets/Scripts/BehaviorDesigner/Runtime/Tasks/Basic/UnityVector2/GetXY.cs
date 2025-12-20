using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Stores the X and Y values of the Vector2.")]
	public class GetXY : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeX.Value = this.vector2Variable.Value.x;
			this.storeY.Value = this.vector2Variable.Value.y;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeX = (this.storeY = 0f);
		}

		[Tooltip("The Vector2 to get the values of")]
		public SharedVector2 vector2Variable;

		[Tooltip("The X value")]
		[RequiredField]
		public SharedFloat storeX;

		[Tooltip("The Y value")]
		[RequiredField]
		public SharedFloat storeY;
	}
}
