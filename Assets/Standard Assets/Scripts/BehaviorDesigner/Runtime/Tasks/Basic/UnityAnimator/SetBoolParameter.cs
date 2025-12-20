using System;
using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Sets the bool parameter on an animator. Returns Success.")]
	public class SetBoolParameter : Action
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
			this.hashID = Animator.StringToHash(this.paramaterName.Value);
			bool @bool = this.animator.GetBool(this.hashID);
			this.animator.SetBool(this.hashID, this.boolValue.Value);
			if (this.setOnce)
			{
				base.StartCoroutine(this.ResetValue(@bool));
			}
			return TaskStatus.Success;
		}

		public IEnumerator ResetValue(bool origVale)
		{
			yield return null;
			this.animator.SetBool(this.hashID, origVale);
			yield break;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = string.Empty;
			this.boolValue = false;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		[Tooltip("The value of the bool parameter")]
		public SharedBool boolValue;

		[Tooltip("Should the value be reverted back to its original value after it has been set?")]
		public bool setOnce;

		private int hashID;

		private Animator animator;

		private GameObject prevGameObject;
	}
}
