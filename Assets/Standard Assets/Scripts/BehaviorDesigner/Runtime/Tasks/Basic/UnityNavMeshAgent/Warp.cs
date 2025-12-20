using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Warps agent to the provided position. Returns Success.")]
	public class Warp : Action
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
			this.navMeshAgent.Warp(this.newPosition.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.newPosition = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The position to warp to")]
		public SharedVector3 newPosition;

		private NavMeshAgent navMeshAgent;

		private GameObject prevGameObject;
	}
}
