using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskCategory("Basic/Light")]
	[TaskDescription("Sets the cookie of the light.")]
	public class SetCookie : Action
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
			this.light.cookie = this.cookie;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.cookie = null;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The cookie to set")]
		public Texture2D cookie;

		private Light light;

		private GameObject prevGameObject;
	}
}
