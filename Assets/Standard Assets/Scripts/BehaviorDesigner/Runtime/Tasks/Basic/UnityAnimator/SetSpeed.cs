using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Sets the playback speed of the Animator. 1 is normal playback speed. Returns Success.")]
	public class SetSpeed : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				UnityEngine.Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.speed = this.speed.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.speed = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The playback speed of the Animator")]
		public SharedFloat speed;

		private Animator animator;

		private GameObject prevGameObject;
	}
}
