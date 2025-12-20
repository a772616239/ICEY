using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Clamps the magnitude of the Vector3.")]
	public class ClampMagnitude : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.ClampMagnitude(this.vector3Variable.Value, this.maxLength.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.vector3Variable = (this.storeResult = Vector3.zero);
			this.maxLength = 0f;
		}

		[Tooltip("The Vector3 to clamp the magnitude of")]
		public SharedVector3 vector3Variable;

		[Tooltip("The max length of the magnitude")]
		public SharedFloat maxLength;

		[Tooltip("The clamp magnitude resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
