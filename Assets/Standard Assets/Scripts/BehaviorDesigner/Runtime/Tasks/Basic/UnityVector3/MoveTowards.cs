using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Move from the current position to the target position.")]
	public class MoveTowards : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.MoveTowards(this.currentPosition.Value, this.targetPosition.Value, this.speed.Value * Time.deltaTime);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.currentPosition = (this.targetPosition = (this.storeResult = Vector3.zero));
			this.speed = 0f;
		}

		[Tooltip("The current position")]
		public SharedVector3 currentPosition;

		[Tooltip("The target position")]
		public SharedVector3 targetPosition;

		[Tooltip("The movement speed")]
		public SharedFloat speed;

		[Tooltip("The move resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
