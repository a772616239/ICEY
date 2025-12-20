using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskCategory("Basic/Animation")]
	[TaskDescription("Blends the animation. Returns Success.")]
	public class Blend : Action
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
			this.animation.Blend(this.animationName.Value, this.targetWeight, this.fadeLength);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName = string.Empty;
			this.targetWeight = 1f;
			this.fadeLength = 0.3f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the animation")]
		public SharedString animationName;

		[Tooltip("The weight the animation should blend to")]
		public float targetWeight = 1f;

		[Tooltip("The amount of time it takes to blend")]
		public float fadeLength = 0.3f;

		private Animation animation;

		private GameObject prevGameObject;
	}
}
