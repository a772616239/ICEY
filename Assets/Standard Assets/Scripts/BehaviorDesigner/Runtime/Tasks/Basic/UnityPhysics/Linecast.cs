using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics
{
	[TaskCategory("Basic/Physics")]
	[TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=117")]
	public class Linecast : Action
	{
		public override TaskStatus OnUpdate()
		{
			return (!Physics.Linecast(this.startPosition.Value, this.endPosition.Value, this.layerMask)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.startPosition = Vector3.zero;
			this.endPosition = Vector3.zero;
			this.layerMask = -1;
		}

		[Tooltip("The starting position of the linecast")]
		private SharedVector3 startPosition;

		[Tooltip("The ending position of the linecast")]
		private SharedVector3 endPosition;

		[Tooltip("Selectively ignore colliders.")]
		public LayerMask layerMask = -1;
	}
}
