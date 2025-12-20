using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLayerMask
{
	[TaskCategory("Basic/LayerMask")]
	[TaskDescription("Gets the layer of a GameObject.")]
	public class GetLayer : Action
	{
		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			this.storeResult.Value = LayerMask.LayerToName(defaultGameObject.layer);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = string.Empty;
		}

		[Tooltip("The GameObject to set the layer of")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the layer to get")]
		[RequiredField]
		public SharedString storeResult;
	}
}
