using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Clamps the magnitude of the Vector2.")]
	public class ClampMagnitude : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.ClampMagnitude(this.vector2Variable.Value, this.maxLength.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector2Variable = (this.storeResult = Vector2.zero);
			this.maxLength = 0f;
		}

		[Tooltip("The Vector2 to clamp the magnitude of")]
		public SharedVector2 vector2Variable;

		[Tooltip("The max length of the magnitude")]
		public SharedFloat maxLength;

		[Tooltip("The clamp magnitude resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
