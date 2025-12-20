using System;
using UnityEngine;

namespace Xft
{
	public class SimpleRotate : MonoBehaviour
	{
		private void Start()
		{
			this.OriAngleX = base.transform.rotation.x;
			this.OriAngleY = base.transform.rotation.y;
			this.OriAngleZ = base.transform.rotation.z;
		}

		private void Update()
		{
			if (this.RotateX)
			{
				this.OriAngleX += Time.deltaTime * this.RotateSpeed;
			}
			if (this.RotateY)
			{
				this.OriAngleY -= Time.deltaTime * this.RotateSpeed;
			}
			if (this.RotateZ)
			{
				this.OriAngleZ += Time.deltaTime * this.RotateSpeed;
			}
			base.transform.rotation = Quaternion.Euler(this.OriAngleX, this.OriAngleY, this.OriAngleZ);
		}

		protected float OriAngleX;

		protected float OriAngleY;

		protected float OriAngleZ;

		public float RotateSpeed = 20f;

		public bool RotateX = true;

		public bool RotateY;

		public bool RotateZ;
	}
}
