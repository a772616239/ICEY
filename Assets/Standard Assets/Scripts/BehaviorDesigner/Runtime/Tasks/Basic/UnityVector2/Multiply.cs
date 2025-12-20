using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Multiply the Vector2 by a float.")]
	public class Multiply : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value * this.multiplyBy.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector2Variable = (this.storeResult = Vector2.zero);
			this.multiplyBy = 0f;
		}

		[Tooltip("The Vector2 to multiply of")]
		public SharedVector2 vector2Variable;

		[Tooltip("The value to multiply the Vector2 of")]
		public SharedFloat multiplyBy;

		[Tooltip("The multiplication resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
