using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskCategory("Basic/Animation")]
	[TaskDescription("Stores the animate physics value. Returns Success.")]
	public class GetAnimatePhysics : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				UnityEngine.Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animation.animatePhysics;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue.Value = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Are the if animations are executed in the physics loop?")]
		[RequiredField]
		public SharedBool storeValue;

		private Animation animation;

		private GameObject prevGameObject;
	}
}
