using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskCategory("Basic/Light")]
	[TaskDescription("Sets the light's cookie size.")]
	public class SetCookieSize : Action
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
			this.light.cookieSize = this.cookieSize.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.cookieSize = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The size to set")]
		public SharedFloat cookieSize;

		private Light light;

		private GameObject prevGameObject;
	}
}
