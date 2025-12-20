using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskCategory("Basic/Light")]
	[TaskDescription("Sets the intensity of the light.")]
	public class SetIntensity : Action
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
			this.light.intensity = this.intensity.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.intensity = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The intensity to set")]
		public SharedFloat intensity;

		private Light light;

		private GameObject prevGameObject;
	}
}
