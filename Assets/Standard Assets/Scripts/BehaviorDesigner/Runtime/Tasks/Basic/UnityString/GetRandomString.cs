using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskCategory("Basic/String")]
	[TaskDescription("Randomly selects a string from the array of strings.")]
	public class GetRandomString : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.source[UnityEngine.Random.Range(0, this.source.Length)].Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.source = null;
			this.storeResult = null;
		}

		[Tooltip("The array of strings")]
		public SharedString[] source;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
