using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Sets the freeze rotation value of the Rigidbody. Returns Success.")]
	public class SetFreezeRotation : Action
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
			this.rigidbody.freezeRotation = this.freezeRotation.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.freezeRotation = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The freeze rotation value of the Rigidbody")]
		public SharedBool freezeRotation;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;
	}
}
