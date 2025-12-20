using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Stores the Vector2 value of the Vector3.")]
	public class GetVector2 : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeResult = Vector2.zero;
		}

		[Tooltip("The Vector3 to get the Vector2 value of")]
		public SharedVector3 vector3Variable;

		[Tooltip("The Vector2 value")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
