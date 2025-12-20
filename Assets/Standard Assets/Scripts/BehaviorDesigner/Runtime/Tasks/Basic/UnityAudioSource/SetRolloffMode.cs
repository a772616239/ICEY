using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
	[TaskCategory("Basic/AudioSource")]
	[TaskDescription("Sets the rolloff mode of the AudioSource. Returns Success.")]
	public class SetRolloffMode : Action
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
			this.audioSource.rolloffMode = this.rolloffMode;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The rolloff mode of the AudioSource")]
		public AudioRolloffMode rolloffMode;

		private AudioSource audioSource;

		private GameObject prevGameObject;
	}
}
