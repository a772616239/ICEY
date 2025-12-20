using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnitySphereCollider
{
	[TaskCategory("Basic/SphereCollider")]
	[TaskDescription("Stores the radius of the SphereCollider. Returns Success.")]
	public class GetRadius : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.sphereCollider = defaultGameObject.GetComponent<SphereCollider>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.sphereCollider == null)
			{
				UnityEngine.Debug.LogWarning("SphereCollider is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.sphereCollider.radius;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The radius of the SphereCollider")]
		[RequiredField]
		public SharedFloat storeValue;

		private SphereCollider sphereCollider;

		private GameObject prevGameObject;
	}
}
