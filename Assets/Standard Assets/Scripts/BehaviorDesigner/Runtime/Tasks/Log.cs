using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=16")]
	[TaskIcon("{SkinColor}LogIcon.png")]
	public class Log : Action
	{
		public override TaskStatus OnUpdate()
		{
			if (this.logError.Value)
			{
				UnityEngine.Debug.LogError(this.text);
			}
			else
			{
				UnityEngine.Debug.Log(this.text);
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.text = string.Empty;
			this.logError = false;
		}

		[Tooltip("Text to output to the log")]
		public SharedString text;

		[Tooltip("Is this text an error?")]
		public SharedBool logError;
	}
}
