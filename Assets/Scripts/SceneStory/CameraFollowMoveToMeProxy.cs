using System;
using UnityEngine;

namespace SceneStory
{
	public class CameraFollowMoveToMeProxy : BaseBehaviour
	{
		private void OnEnable()
		{
			SingletonMono<CameraController>.Instance.CameraZoom(base.transform.position + this.offset, 0.4f, 3f);
		}

		private void OnDrawGizmosSelected()
		{
			DebugX.DrawText("目标点", base.transform.position + this.offset);
		}

		private void OnDisable()
		{
			SingletonMono<CameraController>.Instance.CameraZoomFinished();
		}

		public Vector3 offset = new Vector3(0f, 0f, -6f);
	}
}
