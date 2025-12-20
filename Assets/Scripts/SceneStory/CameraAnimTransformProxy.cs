using System;
using UnityEngine;

namespace SceneStory
{
	public class CameraAnimTransformProxy : BaseBehaviour
	{
		private void OnEnable()
		{
			this.cam = Camera.main.transform.parent;
		}

		private void OnDisable()
		{
			if (!SingletonMono<CameraController>.ApplicationIsQuitting)
			{
				SingletonMono<CameraController>.Instance.IsFollowPivot = true;
			}
		}

		private void Update()
		{
			if (this.cam == null)
			{
				return;
			}
			if (SingletonMono<CameraController>.Instance != null)
			{
				SingletonMono<CameraController>.Instance.IsFollowPivot = (!this.refreshOffset && !this.refreshPos);
			}
			if (this.refreshPos)
			{
				this.cam.position = base.transform.position;
			}
			if (this.refreshOffset)
			{
				Vector3 position = this.cam.position + (this.offset - this.lastOffset);
				this.lastOffset = this.offset;
				this.cam.position = position;
			}
		}

		private Transform cam;

		private Vector3 lastOffset = Vector3.zero;

		[Header("开启该属性即可通过直接编辑Transform来同步位置")]
		public bool refreshPos;

		public Vector3 offset = Vector3.zero;

		[Header("开启该属性即可通过直接offset来同步相对位置")]
		public bool refreshOffset;
	}
}
