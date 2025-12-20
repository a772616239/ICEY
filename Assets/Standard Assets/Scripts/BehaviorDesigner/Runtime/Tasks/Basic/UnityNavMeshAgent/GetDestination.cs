using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Gets the destination of the agent in world-space units. Returns Success.")]
	public class GetDestination : Action
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
			this.storeValue.Value = this.navMeshAgent.destination;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[SharedRequired]
		[Tooltip("The NavMeshAgent destination")]
		public SharedVector3 storeValue;

		private NavMeshAgent navMeshAgent;

		private GameObject prevGameObject;
	}
}
