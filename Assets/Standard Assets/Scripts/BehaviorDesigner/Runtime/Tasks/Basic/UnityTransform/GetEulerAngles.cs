using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskCategory("Basic/Transform")]
	[TaskDescription("Stores the euler angles of the Transform. Returns Success.")]
	public class GetEulerAngles : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.targetTransform = defaultGameObject.GetComponent<Transform>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.targetTransform == null)
			{
				UnityEngine.Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.targetTransform.eulerAngles;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The euler angles of the Transform")]
		[RequiredField]
		public SharedVector3 storeValue;

		private Transform targetTransform;

		private GameObject prevGameObject;
	}
}
