using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Stores the Vector3 value of the Vector2.")]
	public class GetVector3 : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector3Variable.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector3Variable = Vector2.zero;
			this.storeResult = Vector3.zero;
		}

		[Tooltip("The Vector2 to get the Vector3 value of")]
		public SharedVector2 vector3Variable;

		[Tooltip("The Vector3 value")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
