using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Returns success if the specified parameter is controlled by an additional curve on an animation.")]
	public class IsParameterControlledByCurve : Conditional
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				UnityEngine.Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			return (!this.animator.IsParameterControlledByCurve(this.paramaterName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = string.Empty;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		private Animator animator;

		private GameObject prevGameObject;
	}
}
