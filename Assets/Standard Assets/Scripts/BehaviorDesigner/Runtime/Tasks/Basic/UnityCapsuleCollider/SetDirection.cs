using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCapsuleCollider
{
	[TaskCategory("Basic/CapsuleCollider")]
	[TaskDescription("Sets the direction of the CapsuleCollider. Returns Success.")]
	public class SetDirection : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.capsuleCollider = defaultGameObject.GetComponent<CapsuleCollider>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.capsuleCollider == null)
			{
				UnityEngine.Debug.LogWarning("CapsuleCollider is null");
				return TaskStatus.Failure;
			}
			this.capsuleCollider.direction = this.direction.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.direction = 0;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The direction of the CapsuleCollider")]
		public SharedInt direction;

		private CapsuleCollider capsuleCollider;

		private GameObject prevGameObject;
	}
}
