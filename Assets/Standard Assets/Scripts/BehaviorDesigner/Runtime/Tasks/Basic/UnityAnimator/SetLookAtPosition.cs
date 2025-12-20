using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Sets the look at position. Returns Success.")]
	public class SetLookAtPosition : Action
	{
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
			this.positionSet = false;
		}

		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				UnityEngine.Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			return (!this.positionSet) ? TaskStatus.Running : TaskStatus.Success;
		}

		public override void OnAnimatorIK()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.SetLookAtPosition(this.position.Value);
			this.positionSet = true;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.position = Vector3.zero;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The position to lookAt")]
		public SharedVector3 position;

		private Animator animator;

		private GameObject prevGameObject;

		private bool positionSet;
	}
}
