using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnitySphereCollider
{
	[TaskCategory("Basic/SphereCollider")]
	[TaskDescription("Sets the center of the SphereCollider. Returns Success.")]
	public class SetCenter : Action
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
			this.sphereCollider.center = this.center.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.center = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The center of the SphereCollider")]
		public SharedVector3 center;

		private SphereCollider sphereCollider;

		private GameObject prevGameObject;
	}
}
