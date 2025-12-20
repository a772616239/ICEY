using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskCategory("Basic/ParticleSystem")]
	[TaskDescription("Sets if the Particle System should loop.")]
	public class SetLoop : Action
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

            temp.loop = this.loop.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.loop = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Should the ParticleSystem loop?")]
		public SharedBool loop;

		private ParticleSystem particleSystem;

		private GameObject prevGameObject;
	}
}
