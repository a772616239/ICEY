using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskCategory("Basic/Animation")]
	[TaskDescription("Sets the wrap mode to the specified value. Returns Success.")]
	public class SetWrapMode : Action
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
			this.animation.wrapMode = this.wrapMode;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.wrapMode = WrapMode.Default;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("How should time beyond the playback range of the clip be treated?")]
		public WrapMode wrapMode;

		private Animation animation;

		private GameObject prevGameObject;
	}
}
