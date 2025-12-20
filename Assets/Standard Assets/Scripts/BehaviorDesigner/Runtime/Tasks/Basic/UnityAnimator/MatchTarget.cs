using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress. Returns Success.")]
	public class MatchTarget : Action
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
			this.animator.MatchTarget(this.matchPosition.Value, this.matchRotation.Value, this.targetBodyPart, new MatchTargetWeightMask(this.weightMaskPosition, this.weightMaskRotation), this.startNormalizedTime, this.targetNormalizedTime);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.matchPosition = Vector3.zero;
			this.matchRotation = Quaternion.identity;
			this.targetBodyPart = AvatarTarget.Root;
			this.weightMaskPosition = Vector3.zero;
			this.weightMaskRotation = 0f;
			this.startNormalizedTime = 0f;
			this.targetNormalizedTime = 1f;
		}

		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The position we want the body part to reach")]
		public SharedVector3 matchPosition;

		[Tooltip("The rotation in which we want the body part to be")]
		public SharedQuaternion matchRotation;

		[Tooltip("The body part that is involved in the match")]
		public AvatarTarget targetBodyPart;

		[Tooltip("Weights for matching position")]
		public Vector3 weightMaskPosition;

		[Tooltip("Weights for matching rotation")]
		public float weightMaskRotation;

		[Tooltip("Start time within the animation clip")]
		public float startNormalizedTime;

		[Tooltip("End time within the animation clip")]
		public float targetNormalizedTime = 1f;

		private Animator animator;

		private GameObject prevGameObject;
	}
}
