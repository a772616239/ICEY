using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Stores the dot product of two Vector3 values.")]
	public class Dot : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Dot(this.leftHandSide.Value, this.rightHandSide.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.leftHandSide = (this.rightHandSide = Vector3.zero);
			this.storeResult = 0f;
		}

		[Tooltip("The left hand side of the dot product")]
		public SharedVector3 leftHandSide;

		[Tooltip("The right hand side of the dot product")]
		public SharedVector3 rightHandSide;

		[Tooltip("The dot product result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
