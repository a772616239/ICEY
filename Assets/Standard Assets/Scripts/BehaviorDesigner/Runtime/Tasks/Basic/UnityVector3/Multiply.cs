using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Multiply the Vector3 by a float.")]
	public class Multiply : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value * this.multiplyBy.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector3Variable = (this.storeResult = Vector3.zero);
			this.multiplyBy = 0f;
		}

		[Tooltip("The Vector3 to multiply of")]
		public SharedVector3 vector3Variable;

		[Tooltip("The value to multiply the Vector3 of")]
		public SharedFloat multiplyBy;

		[Tooltip("The multiplication resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
