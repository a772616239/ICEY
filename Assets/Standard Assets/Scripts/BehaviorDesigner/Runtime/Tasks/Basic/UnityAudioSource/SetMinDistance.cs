using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
	[TaskCategory("Basic/AudioSource")]
	[TaskDescription("Sets the min distance value of the AudioSource. Returns Success.")]
	public class SetMinDistance : Action
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
			this.audioSource.minDistance = this.minDistance.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.minDistance = 1f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The min distance value of the AudioSource")]
		public SharedFloat minDistance;

		private AudioSource audioSource;

		private GameObject prevGameObject;
	}
}
