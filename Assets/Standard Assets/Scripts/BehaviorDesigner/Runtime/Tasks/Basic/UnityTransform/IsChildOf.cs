using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskCategory("Basic/Transform")]
	[TaskDescription("Returns Success if the transform is a child of the specified GameObject.")]
	public class IsChildOf : Conditional
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
			return (!this.targetTransform.IsChildOf(this.transformName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.transformName = null;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The interested transform")]
		public SharedTransform transformName;

		private Transform targetTransform;

		private GameObject prevGameObject;
	}
}
