using System;
using UnityEngine;

namespace Xft
{
	[ExecuteInEditMode]
	public class XftCameraShakeComp : MonoBehaviour
	{
		public Spring PositionSpring
		{
			get
			{
				return this.mPositionSpring;
			}
			set
			{
				this.mPositionSpring = value;
			}
		}

		public Spring RotationSpring
		{
			get
			{
				return this.mRotationSpring;
			}
			set
			{
				this.mRotationSpring = value;
			}
		}

		public XftEventComponent Client
		{
			get
			{
				return this.m_client;
			}
		}

		private void Awake()
		{
			base.enabled = false;
		}

		public void Reset(XftEventComponent client)
		{
			this.m_client = client;
			if (this.m_client.CameraShakeType == XCameraShakeType.Spring)
			{
				if (this.PositionSpring != null && !this.CheckDone())
				{
					base.transform.localPosition = this.mOriPosition;
					base.transform.localRotation = Quaternion.Euler(this.mOriRotation);
				}
				this.PositionSpring = new Spring(client.transform, Spring.TransformType.Position);
				this.PositionSpring.MinVelocity = 1E-05f;
				this.RotationSpring = new Spring(client.transform, Spring.TransformType.Rotation);
				this.RotationSpring.MinVelocity = 1E-05f;
				this.PositionSpring.Stiffness = new Vector3(this.m_client.PositionStifness, this.m_client.PositionStifness, this.m_client.PositionStifness);
				this.PositionSpring.Damping = Vector3.one - new Vector3(this.m_client.PositionDamping, this.m_client.PositionDamping, this.m_client.PositionDamping);
				this.RotationSpring.Stiffness = new Vector3(this.m_client.RotationStiffness, this.m_client.RotationStiffness, this.m_client.RotationStiffness);
				this.RotationSpring.Damping = Vector3.one - new Vector3(this.m_client.RotationDamping, this.m_client.RotationDamping, this.m_client.RotationDamping);
				this.m_client.transform.localPosition = base.transform.localPosition;
				this.m_client.transform.localRotation = base.transform.localRotation;
				this.PositionSpring.RefreshTransformType();
				this.RotationSpring.RefreshTransformType();
				this.m_earthQuakeTimeTemp = this.m_client.EarthQuakeTime;
				this.mLastPosition = base.transform.localPosition;
				this.mLastRotation = base.transform.localRotation.eulerAngles;
				this.mOriPosition = base.transform.localPosition;
				this.mOriRotation = base.transform.localRotation.eulerAngles;
			}
			this.Update();
		}

		private void UpdateEarthQuake()
		{
			if (this.m_client == null || !this.m_client.UseEarthQuake || this.m_earthQuakeTimeTemp <= 0f || !this.EarthQuakeToggled || this.m_client.ElapsedTime > this.m_client.EarthQuakeTime)
			{
				return;
			}
			this.m_earthQuakeTimeTemp -= 0.0166f * (60f * Time.deltaTime);
			float num;
			if (this.m_client.EarthQuakeMagTye == MAGTYPE.Fixed)
			{
				num = this.m_client.EarthQuakeMagnitude;
			}
			else if (this.m_client.EarthQuakeMagTye == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.m_client.EarthQuakeMagCurve.Evaluate(this.m_client.ElapsedTime);
			}
			else
			{
				num = this.m_client.EarthQuakeMagCurveX.Evaluate(this.m_client.ElapsedTime);
			}
			Vector3 force = Vector3.Scale(XftSmoothRandom.GetVector3Centered(1f), new Vector3(num, 0f, num)) * Mathf.Min(this.m_earthQuakeTimeTemp, 1f);
			float num2 = 0f;
			if (UnityEngine.Random.value < 0.3f)
			{
				num2 = UnityEngine.Random.Range(0f, num * 0.35f) * Mathf.Min(this.m_earthQuakeTimeTemp, 1f);
				if (this.PositionSpring.State.y >= this.PositionSpring.RestState.y)
				{
					num2 = -num2;
				}
			}
			this.PositionSpring.AddForce(force);
			this.RotationSpring.AddForce(new Vector3(0f, 0f, -force.x * 2f) * this.m_client.EarthQuakeCameraRollFactor);
			this.PositionSpring.AddForce(new Vector3(0f, num2, 0f));
		}

		public bool CheckDone()
		{
			if (this.m_client.CameraShakeType == XCameraShakeType.Spring)
			{
				if (this.PositionSpring == null || this.RotationSpring == null)
				{
					return true;
				}
				if (this.PositionSpring.Done && this.RotationSpring.Done)
				{
					return true;
				}
			}
			else if (this.m_client.ElapsedTime > this.m_client.ShakeCurveTime)
			{
				return true;
			}
			return false;
		}

		private void UpdateCurve()
		{
			float time = this.m_client.ElapsedTime / this.m_client.ShakeCurveTime;
			Vector3 a = this.mOriPosition + this.m_client.PositionForce * (this.m_client.PositionCurve.Evaluate(time) * 2f - 1f);
			Vector3 a2 = this.mOriRotation + this.m_client.RotationForce * (this.m_client.RotationCurve.Evaluate(time) * 2f - 1f);
			Vector3 b = a - this.mLastPosition;
			Vector3 b2 = a2 - this.mLastRotation;
			base.transform.localPosition += b;
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			base.transform.localRotation = Quaternion.Euler(eulerAngles + b2);
			this.mLastPosition = a;
			this.mLastRotation = a2;
		}

		private void UpdateSpring()
		{
			if (this.PositionSpring == null || this.RotationSpring == null)
			{
				return;
			}
			this.UpdateEarthQuake();
			this.PositionSpring.FixedUpdate();
			this.RotationSpring.FixedUpdate();
			Vector3 b = this.m_client.transform.localPosition - this.mLastPosition;
			Vector3 b2 = this.m_client.transform.localEulerAngles - this.mLastRotation;
			base.transform.localPosition += b;
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			base.transform.localRotation = Quaternion.Euler(eulerAngles + b2);
			this.mLastPosition = this.m_client.transform.localPosition;
			this.mLastRotation = this.m_client.transform.localEulerAngles;
		}

		private void Update()
		{
			if (this.m_client == null)
			{
				return;
			}
			if (this.m_client.CameraShakeType == XCameraShakeType.Curve)
			{
				this.UpdateCurve();
			}
			else
			{
				this.UpdateSpring();
			}
			if (this.CheckDone())
			{
				base.enabled = false;
				return;
			}
		}

		protected Spring mPositionSpring;

		protected Spring mRotationSpring;

		protected XftEventComponent m_client;

		public bool EarthQuakeToggled;

		protected float m_earthQuakeTimeTemp;

		protected Vector3 mLastPosition;

		protected Vector3 mLastRotation;

		protected Vector3 mOriPosition;

		protected Vector3 mOriRotation;
	}
}
