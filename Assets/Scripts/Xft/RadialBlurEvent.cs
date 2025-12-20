using System;
using UnityEngine;

namespace Xft
{
	public class RadialBlurEvent : CameraEffectEvent
	{
		public RadialBlurEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.RadialBlur, owner)
		{
			this.RadialBlurShader = owner.RadialBlurShader;
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

		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			Vector3 center = base.MyCamera.WorldToScreenPoint(this.m_owner.RadialBlurObj.position);
			this.Center = center;
			if (this.m_owner.RBStrengthType == MAGTYPE.Fixed)
			{
				this.m_strength = this.m_owner.RBSampleStrength;
			}
			else if (this.m_owner.RBStrengthType == MAGTYPE.Curve_OBSOLETE)
			{
				this.m_strength = this.m_owner.RBSampleStrengthCurve.Evaluate(this.m_elapsedTime);
			}
			else
			{
				this.m_strength = this.m_owner.RBSampleStrengthCurveX.Evaluate(this.m_elapsedTime);
			}
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.MyMaterial.SetFloat("_SampleDist", this.m_owner.RBSampleDist);
			this.MyMaterial.SetFloat("_SampleStrength", this.m_strength);
			Vector4 zero = Vector4.zero;
			zero.x = this.Center.x / (float)Screen.width;
			zero.y = this.Center.y / (float)Screen.height;
			this.MyMaterial.SetVector("_Center", zero);
			Graphics.Blit(source, destination, this.m_material);
		}

		protected Material m_material;

		public Shader RadialBlurShader;

		public Vector3 Center = new Vector3(0.5f, 0.5f, 0f);

		protected float m_strength;
	}
}
