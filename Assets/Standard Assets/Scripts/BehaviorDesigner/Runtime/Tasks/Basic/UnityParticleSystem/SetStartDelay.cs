using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskCategory("Basic/ParticleSystem")]
	[TaskDescription("Sets the start delay of the Particle System.")]
	public class SetStartDelay : Action
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
            var temp = this.particleSystem.main;

            temp.startDelay = this.startDelay.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.startDelay = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The start delay of the ParticleSystem")]
		public SharedFloat startDelay;

		private ParticleSystem particleSystem;

		private GameObject prevGameObject;
	}
}
