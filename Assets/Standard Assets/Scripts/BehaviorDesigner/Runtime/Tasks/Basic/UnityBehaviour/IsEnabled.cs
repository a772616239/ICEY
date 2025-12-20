using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
	[TaskCategory("Basic/Behaviour")]
	[TaskDescription("Returns Success if the object is enabled, otherwise Failure.")]
	public class IsEnabled : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedObject == null && !(this.specifiedObject.Value is Behaviour))
			{
				UnityEngine.Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
				return TaskStatus.Failure;
			}
			return (!(this.specifiedObject.Value as Behaviour).enabled) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			if (this.specifiedObject != null)
			{
				this.specifiedObject.Value = null;
			}
		}

		[Tooltip("The Object to use")]
		public SharedObject specifiedObject;
	}
}
