using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskCategory("Basic/Animation")]
	[TaskDescription("Returns Success if the animation is currently playing.")]
	public class IsPlaying : Conditional
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
			if (string.IsNullOrEmpty(this.animationName.Value))
			{
				return (!this.animation.isPlaying) ? TaskStatus.Failure : TaskStatus.Success;
			}
			return (!this.animation.IsPlaying(this.animationName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = string.Empty;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the animation")]
		public SharedString animationName;

		private Animation animation;

		private GameObject prevGameObject;
	}
}
