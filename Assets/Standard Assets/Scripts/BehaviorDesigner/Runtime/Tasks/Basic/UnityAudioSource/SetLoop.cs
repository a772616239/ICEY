using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
	[TaskCategory("Basic/AudioSource")]
	[TaskDescription("Sets the loop value of the AudioSource. Returns Success.")]
	public class SetLoop : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				UnityEngine.Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.loop = this.loop.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.loop = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The loop value of the AudioSource")]
		public SharedBool loop;

		private AudioSource audioSource;

		private GameObject prevGameObject;
	}
}
