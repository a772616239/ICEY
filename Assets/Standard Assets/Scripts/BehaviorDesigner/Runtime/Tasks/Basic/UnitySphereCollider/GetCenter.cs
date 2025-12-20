using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnitySphereCollider
{
	[TaskCategory("Basic/SphereCollider")]
	[TaskDescription("Stores the center of the SphereCollider. Returns Success.")]
	public class GetCenter : Action
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
			this.storeValue.Value = this.sphereCollider.center;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeValue = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The center of the SphereCollider")]
		[RequiredField]
		public SharedVector3 storeValue;

		private SphereCollider sphereCollider;

		private GameObject prevGameObject;
	}
}
