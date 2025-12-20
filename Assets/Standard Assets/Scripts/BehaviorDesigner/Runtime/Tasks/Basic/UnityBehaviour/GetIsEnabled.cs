using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
	[TaskCategory("Basic/Behaviour")]
	[TaskDescription("Stores the enabled state of the object. Returns Success.")]
	public class GetIsEnabled : Action
	{
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedObject == null && !(this.specifiedObject.Value is Behaviour))
			{
				UnityEngine.Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = (this.specifiedObject.Value as Behaviour).enabled;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			if (this.specifiedObject != null)
			{
				this.specifiedObject.Value = null;
			}
			this.storeValue = false;
		}

		[Tooltip("The Object to use")]
		public SharedObject specifiedObject;

		[Tooltip("The enabled/disabled state")]
		[RequiredField]
		public SharedBool storeValue;
	}
}
