using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Returns the distance between two Vector2s.")]
	public class Distance : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.Distance(this.firstVector2.Value, this.secondVector2.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.firstVector2 = (this.secondVector2 = Vector2.zero);
			this.storeResult = 0f;
		}

		[Tooltip("The first Vector2")]
		public SharedVector2 firstVector2;

		[Tooltip("The second Vector2")]
		public SharedVector2 secondVector2;

		[Tooltip("The distance")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
