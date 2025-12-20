using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskCategory("Basic/ParticleSystem")]
	[TaskDescription("Stores if the Particle System is emitting particles.")]
	public class GetEnableEmission : Action
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
            
            var _temp= particleSystem.emission;
            this.storeResult.Value = _temp.enabled;
            return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Is the Particle System emitting particles?")]
		[RequiredField]
		public SharedBool storeResult;

		private ParticleSystem particleSystem;

		private GameObject prevGameObject;
	}
}
