using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskCategory("Basic/SharedVariable")]
	[TaskDescription("Sets the SharedTransformList values from the Transforms. Returns Success.")]
	public class SharedTransformsToTransformList : Action
	{
		public override void OnAwake()
		{
			this.storedTransformList.Value = new List<Transform>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.transforms == null || this.transforms.Length == 0)
			{
				return TaskStatus.Failure;
			}
			this.storedTransformList.Value.Clear();
			for (int i = 0; i < this.transforms.Length; i++)
			{
				this.storedTransformList.Value.Add(this.transforms[i].Value);
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.transforms = null;
			this.storedTransformList = null;
		}

		[Tooltip("The Transforms value")]
		public SharedTransform[] transforms;

		[RequiredField]
		[Tooltip("The SharedTransformList to set")]
		public SharedTransformList storedTransformList;
	}
}
