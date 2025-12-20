using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskCategory("Basic/Transform")]
	[TaskDescription("Moves the transform in the direction and distance of translation. Returns Success.")]
	public class Translate : Action
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
			this.targetTransform.Translate(this.translation.Value, this.relativeTo);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.translation = Vector3.zero;
			this.relativeTo = Space.Self;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Move direction and distance")]
		public SharedVector3 translation;

		[Tooltip("Specifies which axis the rotation is relative to")]
		public Space relativeTo = Space.Self;

		private Transform targetTransform;

		private GameObject prevGameObject;
	}
}
