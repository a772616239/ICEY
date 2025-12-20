using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityDebug
{
	[TaskDescription("LogFormat is analgous to UnityEngine.Debug.LogFormat().\nIt takes format string, substitutes arguments supplied a '{0-4}' and returns success.\nAny fields or arguments not supplied are ignored.It can be used for debugging.")]
	[TaskIcon("{SkinColor}LogIcon.png")]
	public class LogFormat : Action
	{
		public override TaskStatus OnUpdate()
		{
			object[] args = this.buildParamsArray();
			if (this.logError.Value)
			{
				UnityEngine.Debug.LogErrorFormat(this.textFormat.Value, args);
			}
			else
			{
				UnityEngine.Debug.LogFormat(this.textFormat.Value, args);
			}
			return TaskStatus.Success;
		}

		private object[] buildParamsArray()
		{
			object[] array;
			if (this.isValid(this.arg3))
			{
				array = new object[]
				{
					null,
					null,
					null,
					this.arg3.GetValue()
				};
				array[2] = this.arg2.GetValue();
				array[1] = this.arg1.GetValue();
				array[0] = this.arg0.GetValue();
			}
			else if (this.isValid(this.arg2))
			{
				array = new object[]
				{
					null,
					null,
					this.arg2.GetValue()
				};
				array[1] = this.arg1.GetValue();
				array[0] = this.arg0.GetValue();
			}
			else if (this.isValid(this.arg1))
			{
				array = new object[]
				{
					null,
					this.arg1.GetValue()
				};
				array[0] = this.arg0.GetValue();
			}
			else
			{
				if (!this.isValid(this.arg0))
				{
					return null;
				}
				array = new object[]
				{
					this.arg0.GetValue()
				};
			}
			return array;
		}

		private bool isValid(SharedVariable sv)
		{
			return sv != null && !sv.IsNone;
		}

		public override void OnReset()
		{
			this.textFormat = string.Empty;
			this.logError = false;
			this.arg0 = null;
			this.arg1 = null;
			this.arg2 = null;
			this.arg3 = null;
		}

		[Tooltip("Text format with {0}, {1}, etc")]
		public SharedString textFormat;

		[Tooltip("Is this text an error?")]
		public SharedBool logError;

		public SharedVariable arg0;

		public SharedVariable arg1;

		public SharedVariable arg2;

		public SharedVariable arg3;
	}
}
