using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskCategory("Basic/Transform")]
	[TaskDescription("Sets the parent of the Transform. Returns Success.")]
	public class SetParent : Action
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
			this.targetTransform.parent = this.parent.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.parent = null;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The parent of the Transform")]
		public SharedTransform parent;

		private Transform targetTransform;

		private GameObject prevGameObject;
	}
}
