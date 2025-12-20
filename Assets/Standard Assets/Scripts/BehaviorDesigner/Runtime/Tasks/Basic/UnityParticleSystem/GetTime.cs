using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskCategory("Basic/ParticleSystem")]
	[TaskDescription("Stores the time of the Particle System.")]
	public class GetTime : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.particleSystem == null)
			{
				UnityEngine.Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			this.storeResult.Value = this.particleSystem.time;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The time of the ParticleSystem")]
		[RequiredField]
		public SharedFloat storeResult;

		private ParticleSystem particleSystem;

		private GameObject prevGameObject;
	}
}
