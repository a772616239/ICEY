using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBoxCollider2D
{
	[TaskCategory("Basic/BoxCollider2D")]
	[TaskDescription("Stores the size of the BoxCollider2D. Returns Success.")]
	public class GetSize : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.boxCollider2D = defaultGameObject.GetComponent<BoxCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.boxCollider2D == null)
			{
				UnityEngine.Debug.LogWarning("BoxCollider2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.boxCollider2D.size;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector2.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The size of the BoxCollider2D")]
		[RequiredField]
		public SharedVector2 storeValue;

		private BoxCollider2D boxCollider2D;

		private GameObject prevGameObject;
	}
}
