using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
	[TaskCategory("Basic/GameObject")]
	[TaskDescription("Finds a GameObject by name. Returns Success.")]
	public class Find : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = GameObject.Find(this.gameObjectName.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.gameObjectName = null;
			this.storeValue = null;
		}

		[Tooltip("The GameObject name to find")]
		public SharedString gameObjectName;

		[Tooltip("The object found by name")]
		[RequiredField]
		public SharedGameObject storeValue;
	}
}
