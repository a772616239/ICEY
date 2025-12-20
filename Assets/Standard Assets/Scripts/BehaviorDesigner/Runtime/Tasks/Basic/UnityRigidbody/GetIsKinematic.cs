using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Stores the is kinematic value of the Rigidbody. Returns Success.")]
	public class GetIsKinematic : Action
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
			this.storeValue.Value = this.rigidbody.isKinematic;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The is kinematic value of the Rigidbody")]
		[RequiredField]
		public SharedBool storeValue;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;
	}
}
