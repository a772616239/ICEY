using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Normalize the Vector2.")]
	public class Normalize : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value.normalized;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector2Variable = (this.storeResult = Vector2.zero);
		}

		[Tooltip("The Vector2 to normalize")]
		public SharedVector2 vector2Variable;

		[Tooltip("The normalized resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
