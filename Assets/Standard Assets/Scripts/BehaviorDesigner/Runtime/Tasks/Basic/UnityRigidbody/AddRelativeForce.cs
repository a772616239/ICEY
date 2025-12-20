using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Applies a force to the rigidbody relative to its coordinate system. Returns Success.")]
	public class AddRelativeForce : Action
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
			this.rigidbody.AddRelativeForce(this.force.Value, this.forceMode);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.force = Vector3.zero;
			this.forceMode = ForceMode.Force;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The amount of force to apply")]
		public SharedVector3 force;

		[Tooltip("The type of force")]
		public ForceMode forceMode;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;
	}
}
