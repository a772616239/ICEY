using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskCategory("Basic/Light")]
	[TaskDescription("Stores the shadow bias of the light.")]
	public class GetShadowBias : Action
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
			this.storeValue = this.light.shadowBias;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("The shadow bias to store")]
		public SharedFloat storeValue;

		private Light light;

		private GameObject prevGameObject;
	}
}
