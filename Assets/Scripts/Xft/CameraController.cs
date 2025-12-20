using System;
using UnityEngine;

namespace Xft
{
	public class CameraController : MonoBehaviour
	{
		private void Start()
		{
			this.mDistance = Vector3.Distance(base.transform.position, this.Target.position);
			this.mCurDistance = this.mDistance;
			this.mDesiredDistance = this.mDistance;
			this.mCurrentRotation = base.transform.rotation;
			this.mDesiredRotation = base.transform.rotation;
			this.mDegX = base.transform.rotation.eulerAngles.y;
			this.mDegY = base.transform.rotation.eulerAngles.x;
		}

		private float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		private void LateUpdate()
		{
			base.transform.LookAt(this.Target);
			if (Input.GetMouseButton(0))
			{
				this.mDegX += UnityEngine.Input.GetAxis("Mouse X") * this.RotateSpeed * Time.deltaTime;
				this.mDegY -= UnityEngine.Input.GetAxis("Mouse Y") * this.RotateSpeed * Time.deltaTime;
				this.mDegY = this.ClampAngle(this.mDegY, (float)this.RotateYMin, (float)this.RotateYMax);
				this.mDesiredRotation = Quaternion.Euler(this.mDegY, this.mDegX, 0f);
				this.mCurrentRotation = base.transform.rotation;
				Quaternion rotation = Quaternion.Lerp(this.mCurrentRotation, this.mDesiredRotation, Time.deltaTime * this.ZoomDampening);
				base.transform.rotation = rotation;
			}
			else if (Input.GetMouseButton(1))
			{
				this.Target.rotation = base.transform.rotation;
				this.Target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * this.panSpeed);
				this.Target.Translate(base.transform.up * -Input.GetAxis("Mouse Y") * this.panSpeed, Space.World);
			}
			this.mDesiredDistance -= UnityEngine.Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * (float)this.ZoomRate * Mathf.Abs(this.mDesiredDistance);
			this.mCurDistance = Mathf.Lerp(this.mCurDistance, this.mDesiredDistance, Time.deltaTime * this.ZoomDampening);
			Vector3 position = this.Target.position - base.transform.rotation * Vector3.forward * this.mCurDistance;
			base.transform.position = position;
		}

		public Transform Target;

		public int ZoomRate = 40;

		public float ZoomDampening = 5f;

		public float RotateSpeed = 200f;

		public int RotateYMin = -80;

		public int RotateYMax = 80;

		public float panSpeed = 0.3f;

		protected float mDistance;

		protected float mCurDistance;

		protected float mDesiredDistance;

		protected Quaternion mCurrentRotation;

		protected Quaternion mDesiredRotation;

		protected float mDegX;

		protected float mDegY;
	}
}
