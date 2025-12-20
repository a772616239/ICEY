using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCapsuleCollider
{
	[TaskCategory("Basic/CapsuleCollider")]
	[TaskDescription("Gets the height of the CapsuleCollider. Returns Success.")]
	public class GetHeight : Action
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
			this.storeValue.Value = this.capsuleCollider.height;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The height of the CapsuleCollider")]
		[RequiredField]
		public SharedFloat storeValue;

		private CapsuleCollider capsuleCollider;

		private GameObject prevGameObject;
	}
}
