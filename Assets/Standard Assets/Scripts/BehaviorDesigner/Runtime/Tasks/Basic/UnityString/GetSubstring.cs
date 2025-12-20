using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskCategory("Basic/String")]
	[TaskDescription("Stores a substring of the target string")]
	public class GetSubstring : Action
	{
		public override TaskStatus OnUpdate()
		{
			if (this.length.Value != -1)
			{
				this.storeResult.Value = this.targetString.Value.Substring(this.startIndex.Value, this.length.Value);
			}
			else
			{
				this.storeResult.Value = this.targetString.Value.Substring(this.startIndex.Value);
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetString = string.Empty;
			this.startIndex = 0;
			this.length = -1;
			this.storeResult = string.Empty;
		}

		[Tooltip("The target string")]
		public SharedString targetString;

		[Tooltip("The start substring index")]
		public SharedInt startIndex = 0;

		[Tooltip("The length of the substring. Don't use if -1")]
		public SharedInt length = -1;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
