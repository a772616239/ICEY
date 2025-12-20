using System;
using UnityEngine;

namespace Xft
{
	public class GlowEvent : CameraEffectEvent
	{
		public GlowEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.Glow, owner)
		{
			this.downsampleShader = owner.GlowDownSampleShader;
			this.compositeShader = owner.GlowCompositeShader;
			this.blurShader = owner.GlowBlurShader;
		}

		protected Material compositeMaterial
		{
			get
			{
				if (this.m_CompositeMaterial == null)
				{
					this.m_CompositeMaterial = new Material(this.compositeShader);
					this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_CompositeMaterial;
			}
		}

		protected Material blurMaterial
		{
			get
			{
				if (this.m_BlurMaterial == null)
				{
					this.m_BlurMaterial = new Material(this.blurShader);
					this.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_BlurMaterial;
			}
		}

		protected Material downsampleMaterial
		{
			get
			{
				if (this.m_DownsampleMaterial == null)
				{
					this.m_DownsampleMaterial = new Material(this.downsampleShader);
					this.m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_DownsampleMaterial;
			}
		}

		private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration, float spread)
		{
			float num = 0.5f + (float)iteration * spread;
			Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
			{
				new Vector2(num, num),
				new Vector2(-num, num),
				new Vector2(num, -num),
				new Vector2(-num, -num)
			});
		}

		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			this.downsampleMaterial.color = new Color(this.m_glowTint.r, this.m_glowTint.g, this.m_glowTint.b, this.m_glowTint.a / 4f);
			Graphics.Blit(source, dest, this.downsampleMaterial);
		}

		private void BlitGlow(RenderTexture source, RenderTexture dest, float intensity)
		{
			this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(intensity));
			Graphics.Blit(source, dest, this.compositeMaterial);
		}

		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (this.downsampleShader == null)
			{
				UnityEngine.Debug.Log("No downsample shader assigned! Disabling glow.");
				result = false;
			}
			else
			{
				if (!this.blurMaterial.shader.isSupported)
				{
					result = false;
				}
				if (!this.compositeMaterial.shader.isSupported)
				{
					result = false;
				}
				if (!this.downsampleMaterial.shader.isSupported)
				{
					result = false;
				}
			}
			return result;
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int num = this.m_owner.GlowBlurIterations;
			float num2 = this.m_owner.GlowBlurSpread;
			float num3 = this.m_owner.GlowIntensity;
			num3 = Mathf.Clamp(num3, 0f, 10f);
			num = Mathf.Clamp(num, 0, 30);
			num2 = Mathf.Clamp(num2, 0.5f, 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			this.DownSample4x(source, temporary);
			float num4 = Mathf.Clamp01((num3 - 1f) / 4f);
			this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num4);
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				if (flag)
				{
					this.FourTapCone(temporary, temporary2, i, num2);
					temporary.DiscardContents();
				}
				else
				{
					this.FourTapCone(temporary2, temporary, i, num2);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			Graphics.Blit(source, destination);
			if (flag)
			{
				this.BlitGlow(temporary, destination, num3);
			}
			else
			{
				this.BlitGlow(temporary2, destination, num3);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			float t = this.m_owner.ColorCurve.Evaluate(this.m_elapsedTime);
			this.m_glowTint = Color.Lerp(this.m_owner.GlowColorStart, this.m_owner.GlowColorEnd, t);
		}

		protected Color m_glowTint = new Color(1f, 1f, 1f, 1f);

		public Shader compositeShader;

		private Material m_CompositeMaterial;

		public Shader blurShader;

		private Material m_BlurMaterial;

		public Shader downsampleShader;

		private Material m_DownsampleMaterial;
	}
}
