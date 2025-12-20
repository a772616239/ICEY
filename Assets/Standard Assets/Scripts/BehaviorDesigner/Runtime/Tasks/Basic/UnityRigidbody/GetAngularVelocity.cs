using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Stores the angular velocity of the Rigidbody. Returns Success.")]
	public class GetAngularVelocity : Action
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
			this.storeValue.Value = this.rigidbody.angularVelocity;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The angular velocity of the Rigidbody")]
		[RequiredField]
		public SharedVector3 storeValue;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;
	}
}
