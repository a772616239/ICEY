using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskCategory("Basic/String")]
	[TaskDescription("Creates a string from multiple other strings.")]
	public class BuildString : Action
	{
		public override TaskStatus OnUpdate()
		{
			for (int i = 0; i < this.source.Length; i++)
			{
				SharedString sharedString = this.storeResult;
				sharedString.Value += this.source[i];
			}
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
