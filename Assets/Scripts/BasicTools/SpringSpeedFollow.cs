using System;
using UnityEngine;

namespace BasicTools
{
	public class SpringSpeedFollow : MonoBehaviour
	{
		private void Start()
		{
			if (this.followed == null)
			{
				this.followed = base.transform;
			}
		}

		private void Update()
		{
			Vector3 vector = this.followed.transform.position - base.transform.position;
			vector.Scale(this.speedScale);
			base.transform.position += vector;
			Vector3 b = vector - this.maxDeltaDistance;
			b.x = Mathf.Clamp(b.x, 0f, float.MaxValue);
			b.y = Mathf.Clamp(b.y, 0f, float.MaxValue);
			b.z = Mathf.Clamp(b.z, 0f, float.MaxValue);
			base.transform.position += b;
		}

		public Transform followed;

		public Vector3 speedScale;

		public Vector3 maxDeltaDistance;
	}
}
