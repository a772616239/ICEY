using System;
using UnityEngine;

namespace BasicTools
{
	public class FollowGameObject : MonoBehaviour
	{
		private void Start()
		{
		}

		private void LateUpdate()
		{
			if (this.Target == null)
			{
				return;
			}
			Vector3 b = this.Target.position + this.Offset - base.transform.position;
			b = new Vector3(b.x * this.Ignore.x, b.y * this.Ignore.y, b.z * this.Ignore.z);
			base.transform.position += b;
		}

		public FollowGameObject Init(Transform target, Vector3 offset, Vector3 ignore)
		{
			this.Target = target;
			this.Offset = offset;
			this.Ignore = ignore;
			return this;
		}

		private Transform Target;

		private Vector3 Offset;

		private Vector3 Ignore = Vector3.one;

		public static class IgnoreParam
		{
			public static readonly Vector3 X = new Vector3(0f, 1f, 1f);

			public static readonly Vector3 Y = new Vector3(1f, 0f, 1f);

			public static readonly Vector3 Z = new Vector3(1f, 1f, 0f);

			public static readonly Vector3 XY = new Vector3(0f, 1f, 0f);

			public static readonly Vector3 YZ = new Vector3(1f, 0f, 0f);

			public static readonly Vector3 XZ = new Vector3(0f, 1f, 0f);
		}
	}
}
