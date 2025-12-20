using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Rotates the Rigidbody to the specified rotation. Returns Success.")]
	public class MoveRotation : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.rigidbody = defaultGameObject.GetComponent<Rigidbody>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.rigidbody == null)
			{
				UnityEngine.Debug.LogWarning("Rigidbody is null");
				return TaskStatus.Failure;
			}
			this.rigidbody.MoveRotation(this.rotation.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rotation = Quaternion.identity;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The new rotation of the Rigidbody")]
		public SharedQuaternion rotation;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;
	}
}
