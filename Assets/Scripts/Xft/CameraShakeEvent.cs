using System;

namespace Xft
{
	public class CameraShakeEvent : XftEvent
	{
		public CameraShakeEvent(XftEventComponent owner) : base(XEventType.CameraShake, owner)
		{
		}

		public override void Initialize()
		{
			this.ToggleCameraShakeComponent(true);
		}

		public override void Reset()
		{
			base.Reset();
		}

		public override void OnBegin()
		{
			base.OnBegin();
			this.m_cameraShake.Reset(this.m_owner);
			if (this.m_owner.CameraShakeType == XCameraShakeType.Spring)
			{
				this.m_cameraShake.PositionSpring.AddForce(this.m_owner.PositionForce);
				this.m_cameraShake.RotationSpring.AddForce(this.m_owner.RotationForce);
				this.m_cameraShake.EarthQuakeToggled = this.m_owner.UseEarthQuake;
			}
			this.m_cameraShake.enabled = true;
		}

		protected void ToggleCameraShakeComponent(bool flag)
		{
			this.m_cameraShake = base.MyCamera.gameObject.GetComponent<XftCameraShakeComp>();
			if (this.m_cameraShake == null)
			{
				this.m_cameraShake = base.MyCamera.gameObject.AddComponent<XftCameraShakeComp>();
			}
			this.m_cameraShake.enabled = flag;
		}

		protected XftCameraShakeComp m_cameraShake;
	}
}
