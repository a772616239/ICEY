using System;
using UnityEngine;

namespace Xft
{
	public class CameraEffectEvent : XftEvent, IComparable<CameraEffectEvent>
	{
		public CameraEffectEvent(CameraEffectEvent.EType etype, XftEventComponent owner) : base(XEventType.CameraEffect, owner)
		{
			this.m_effectType = etype;
		}

		public int CompareTo(CameraEffectEvent otherObj)
		{
			return this.m_owner.CameraEffectPriority.CompareTo(otherObj.m_owner.CameraEffectPriority);
		}

		public XftCameraEffectComp CameraComp
		{
			get
			{
				this.m_comp = base.MyCamera.gameObject.GetComponent<XftCameraEffectComp>();
				if (this.m_comp == null)
				{
					this.m_comp = base.MyCamera.gameObject.AddComponent<XftCameraEffectComp>();
				}
				return this.m_comp;
			}
		}

		public CameraEffectEvent.EType EffectType
		{
			get
			{
				return this.m_effectType;
			}
		}

		public override void Initialize()
		{
			if (!this.CheckSupport())
			{
				UnityEngine.Debug.LogWarning("camera effect is not supported on this device:" + this.m_effectType);
				this.m_supported = false;
				return;
			}
		}

		public override void OnBegin()
		{
			if (!this.m_supported)
			{
				return;
			}
			base.OnBegin();
			this.CameraComp.AddEvent(this);
		}

		public override void Reset()
		{
			base.Reset();
			this.CameraComp.ResetEvent(this);
		}

		public virtual void ToggleCameraComponent(bool flag)
		{
		}

		public virtual bool CheckSupport()
		{
			return true;
		}

		public virtual void OnPreRender()
		{
		}

		public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}

		protected CameraEffectEvent.EType m_effectType;

		protected XftCameraEffectComp m_comp;

		protected bool m_supported = true;

		public enum EType
		{
			RadialBlur,
			RadialBlurMask,
			Glow,
			GlowPerObj,
			ColorInverse,
			Glitch
		}
	}
}
