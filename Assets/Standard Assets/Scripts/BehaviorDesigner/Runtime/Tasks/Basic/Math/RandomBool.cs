using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Sets a random bool value")]
	public class RandomBool : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = (UnityEngine.Random.value < 0.5f);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.storeResult.Value = false;
		}

		[Tooltip("The variable to store the result")]
		public SharedBool storeResult;
	}
}
