using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
	[TaskCategory("Basic/GameObject")]
	[TaskDescription("Finds a GameObject by tag. Returns Success.")]
	public class FindGameObjectsWithTag : Action
	{
		public override TaskStatus OnUpdate()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.tag.Value);
			for (int i = 0; i < array.Length; i++)
			{
				this.storeValue.Value.Add(array[i]);
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.tag.Value = null;
			this.storeValue.Value = null;
		}

		[Tooltip("The tag of the GameObject to find")]
		public SharedString tag;

		[Tooltip("The objects found by name")]
		[RequiredField]
		public SharedGameObjectList storeValue;
	}
}
