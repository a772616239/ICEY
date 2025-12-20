using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCircleCollider2D
{
	[TaskCategory("Basic/CircleCollider2D")]
	[TaskDescription("Stores the radius of the CircleCollider2D. Returns Success.")]
	public class GetRadius : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.circleCollider2D == null)
			{
				UnityEngine.Debug.LogWarning("CircleCollider2D is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.circleCollider2D.radius;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The radius of the CircleCollider2D")]
		[RequiredField]
		public SharedFloat storeValue;

		private CircleCollider2D circleCollider2D;

		private GameObject prevGameObject;
	}
}
