using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
	[TaskCategory("Basic/AudioSource")]
	[TaskDescription("Stores the time value of the AudioSource. Returns Success.")]
	public class GetTime : Action
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
			this.storeValue.Value = this.audioSource.time;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The time value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		private AudioSource audioSource;

		private GameObject prevGameObject;
	}
}
