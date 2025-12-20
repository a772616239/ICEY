using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLayerMask
{
	[TaskCategory("Basic/LayerMask")]
	[TaskDescription("Sets the layer of a GameObject.")]
	public class SetLayer : Action
	{
		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			defaultGameObject.layer = LayerMask.NameToLayer(this.layerName.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.layerName = "Default";
		}

		[Tooltip("The GameObject to set the layer of")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the layer to set")]
		public SharedString layerName = "Default";
	}
}
