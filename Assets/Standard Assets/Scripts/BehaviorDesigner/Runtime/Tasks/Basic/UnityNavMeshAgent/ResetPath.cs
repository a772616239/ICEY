using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Clears the current path. Returns Success.")]
	public class ResetPath : Action
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
			this.navMeshAgent.ResetPath();
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		private NavMeshAgent navMeshAgent;

		private GameObject prevGameObject;
	}
}
