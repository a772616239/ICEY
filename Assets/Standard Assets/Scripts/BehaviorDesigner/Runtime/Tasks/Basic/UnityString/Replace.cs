using System;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityString
{
	[TaskCategory("Basic/String")]
	[TaskDescription("Replaces a string with the new string")]
	public class Replace : Action
	{
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.targetString.Value.Replace(this.oldString.Value, this.newString.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetString = string.Empty;
			this.oldString = string.Empty;
			this.newString = string.Empty;
			this.storeResult = string.Empty;
		}

		[Tooltip("The target string")]
		public SharedString targetString;

		[Tooltip("The old replace")]
		public SharedString oldString;

		[Tooltip("The new string")]
		public SharedString newString;

		[Tooltip("The stored result")]
		[RequiredField]
		public SharedString storeResult;
	}
}
