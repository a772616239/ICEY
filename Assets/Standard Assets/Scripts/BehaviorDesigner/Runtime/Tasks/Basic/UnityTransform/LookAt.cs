using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskCategory("Basic/Transform")]
	[TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
	public class LookAt : Action
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
			this.targetTransform.LookAt(this.worldPosition.Value, this.worldUp);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.worldPosition = Vector3.up;
			this.worldUp = Vector3.up;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Point to look at")]
		public SharedVector3 worldPosition;

		[Tooltip("Vector specifying the upward direction")]
		public Vector3 worldUp;

		private Transform targetTransform;

		private GameObject prevGameObject;
	}
}
