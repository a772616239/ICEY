using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskCategory("Basic/Light")]
	[TaskDescription("Sets the type of the light.")]
	public class SetType : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.light = defaultGameObject.GetComponent<Light>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.light == null)
			{
				UnityEngine.Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			this.light.type = this.type;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The type to set")]
		public LightType type;

		private Light light;

		private GameObject prevGameObject;
	}
}
