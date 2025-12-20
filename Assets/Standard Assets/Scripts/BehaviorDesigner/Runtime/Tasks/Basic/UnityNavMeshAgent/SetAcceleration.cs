using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Sets the maximum acceleration of an agent as it follows a path, given in units / sec^2. Returns Success.")]
	public class SetAcceleration : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.navMeshAgent = defaultGameObject.GetComponent<NavMeshAgent>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.navMeshAgent == null)
			{
				UnityEngine.Debug.LogWarning("NavMeshAgent is null");
				return TaskStatus.Failure;
			}
			this.navMeshAgent.acceleration = this.acceleration.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.acceleration = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The NavMeshAgent acceleration")]
		public SharedFloat acceleration;

		private NavMeshAgent navMeshAgent;

		private GameObject prevGameObject;
	}
}
