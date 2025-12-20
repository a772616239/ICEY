using System;
using UnityEngine;

namespace Xft
{
	public class GlitchEvent : CameraEffectEvent
	{
		public GlitchEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.Glitch, owner)
		{
			this.m_random = new WaveRandom();
			this.Mask = owner.GlitchMask;
			this.GlitchShader = owner.GlitchShader;
		}

		public Material GlitchMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.GlitchShader);
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
			if (!this.GlitchMaterial.shader.isSupported)
			{
				result = false;
			}
			return result;
		}

		public override void Initialize()
		{
			base.Initialize();
			this.m_random.Reset();
		}

		public override void Reset()
		{
			base.Reset();
			this.m_random.Reset();
		}

		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			this.m_offset = this.m_random.GetRandom(this.m_owner.MinAmp, this.m_owner.MaxAmp, this.m_owner.MinRand, this.m_owner.MaxRand, this.m_owner.WaveLen);
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Mask == null)
			{
				return;
			}
			this.GlitchMaterial.SetVector("_displace", this.m_offset);
			this.GlitchMaterial.SetTexture("_Mask", this.Mask);
			Graphics.Blit(source, destination, this.GlitchMaterial);
		}

		protected Vector3 m_offset;

		protected WaveRandom m_random;

		protected Material m_material;

		public Shader GlitchShader;

		public Texture2D Mask;
	}
}
