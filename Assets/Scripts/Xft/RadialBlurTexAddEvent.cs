using System;
using UnityEngine;

namespace Xft
{
	public class RadialBlurTexAddEvent : CameraEffectEvent
	{
		public RadialBlurTexAddEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.RadialBlurMask, owner)
		{
			this.RadialBlurShader = owner.RadialBlurTexAddShader;
			this.Mask = owner.RadialBlurMask;
		}

		public Material MyMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.RadialBlurShader);
					this.m_material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_material;
			}
		}

		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (!this.MyMaterial.shader.isSupported)
			{
				result = false;
			}
			return result;
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.m_material == null)
			{
				return;
			}
			this.MyMaterial.SetTexture("_Mask", this.Mask);
			this.MyMaterial.SetFloat("_SampleDist", this.m_owner.RBMaskSampleDist);
			this.MyMaterial.SetFloat("_SampleStrength", this.m_strength);
			Graphics.Blit(source, destination, this.m_material);
		}

		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			if (this.m_owner.RBMaskStrengthType == MAGTYPE.Fixed)
			{
				this.m_strength = this.m_owner.RBMaskSampleStrength;
			}
			else if (this.m_owner.RBMaskStrengthType == MAGTYPE.Curve_OBSOLETE)
			{
				this.m_strength = this.m_owner.RBMaskSampleStrengthCurve.Evaluate(this.m_elapsedTime);
			}
			else
			{
				this.m_strength = this.m_owner.RBMaskSampleStrengthCurveX.Evaluate(this.m_elapsedTime);
			}
		}

		protected float m_strength;

		protected Material m_material;

		public Shader RadialBlurShader;

		public Texture2D Mask;
	}
}
