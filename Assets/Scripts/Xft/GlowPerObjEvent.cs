using System;
using UnityEngine;

namespace Xft
{
	public class GlowPerObjEvent : CameraEffectEvent
	{
		public GlowPerObjEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.GlowPerObj, owner)
		{
			this.ReplacementShader = owner.GlowPerObjReplacementShader;
			this.blendShader = owner.GlowPerObjBlendShader;
			this.downsampleShader = owner.GlowDownSampleShader;
			this.compositeShader = owner.GlowCompositeShader;
			this.blurShader = owner.GlowBlurShader;
		}

		protected Camera ShaderCamera
		{
			get
			{
				if (this.m_shaderCamera == null)
				{
					this.m_shaderCamera = new GameObject("ShaderCamera", new Type[]
					{
						typeof(Camera)
					});
					this.m_shaderCamera.GetComponent<Camera>().enabled = false;
					this.m_shaderCamera.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_shaderCamera.GetComponent<Camera>();
			}
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

		protected Material blendMaterial
		{
			get
			{
				if (this.m_blendMaterial == null)
				{
					this.m_blendMaterial = new Material(this.blendShader);
					this.m_blendMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_blendMaterial;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			if (base.MyCamera.depthTextureMode == DepthTextureMode.None)
			{
				base.MyCamera.depthTextureMode = DepthTextureMode.Depth;
			}
		}

		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
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
			return result;
		}

		public override void OnPreRender()
		{
			this.PrepareRenderTex();
			Camera shaderCamera = this.ShaderCamera;
			shaderCamera.CopyFrom(base.MyCamera);
			shaderCamera.backgroundColor = Color.black;
			shaderCamera.clearFlags = CameraClearFlags.Color;
			shaderCamera.targetTexture = this.TempRenderTex;
			shaderCamera.RenderWithShader(this.ReplacementShader, "XftEffect");
		}

		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			float t = this.m_owner.ColorCurve.Evaluate(this.m_elapsedTime);
			this.m_tint = Color.Lerp(this.m_owner.GlowColorStart, this.m_owner.GlowColorEnd, t);
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.RenderGlow(this.TempRenderTex, this.TempRenderGlow);
			this.blendMaterial.SetTexture("_GlowTex", this.TempRenderGlow);
			Graphics.Blit(source, destination, this.blendMaterial);
			this.ReleaseRenderTex();
		}

		private void ReleaseRenderTex()
		{
			if (this.TempRenderGlow != null)
			{
				RenderTexture.ReleaseTemporary(this.TempRenderGlow);
				this.TempRenderGlow = null;
			}
			if (this.TempRenderTex != null)
			{
				RenderTexture.ReleaseTemporary(this.TempRenderTex);
				this.TempRenderTex = null;
			}
		}

		private void PrepareRenderTex()
		{
			if (this.TempRenderTex == null)
			{
				this.TempRenderTex = RenderTexture.GetTemporary(base.MyCamera.pixelWidth, base.MyCamera.pixelHeight, 0);
			}
			if (this.TempRenderGlow == null)
			{
				this.TempRenderGlow = RenderTexture.GetTemporary(base.MyCamera.pixelWidth, base.MyCamera.pixelHeight, 0);
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

		private void DownSample4x(RenderTexture source, RenderTexture dest, Color tint)
		{
			this.downsampleMaterial.color = new Color(tint.r, tint.g, tint.b, tint.a / 4f);
			Graphics.Blit(source, dest, this.downsampleMaterial);
		}

		private void RenderGlow(RenderTexture source, RenderTexture destination)
		{
			float num = this.m_owner.GlowIntensity;
			int num2 = this.m_owner.GlowBlurIterations;
			float num3 = this.m_owner.GlowBlurSpread;
			num = Mathf.Clamp(num, 0f, 10f);
			num2 = Mathf.Clamp(num2, 0, 30);
			num3 = Mathf.Clamp(num3, 0.5f, 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			this.DownSample4x(source, temporary, this.m_tint);
			float num4 = Mathf.Clamp01((num - 1f) / 4f);
			this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num4);
			bool flag = true;
			for (int i = 0; i < num2; i++)
			{
				if (flag)
				{
					this.FourTapCone(temporary, temporary2, i, num3);
					temporary.DiscardContents();
				}
				else
				{
					this.FourTapCone(temporary2, temporary, i, num3);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			Graphics.Blit(source, destination);
			if (flag)
			{
				this.BlitGlow(temporary, destination, num);
			}
			else
			{
				this.BlitGlow(temporary2, destination, num);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		private void BlitGlow(RenderTexture source, RenderTexture dest, float intensity)
		{
			this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(intensity));
			Graphics.Blit(source, dest, this.compositeMaterial);
		}

		public Shader ReplacementShader;

		protected GameObject m_shaderCamera;

		protected RenderTexture TempRenderTex;

		protected RenderTexture TempRenderGlow;

		protected Color m_tint;

		public Shader compositeShader;

		private Material m_CompositeMaterial;

		public Shader blurShader;

		private Material m_BlurMaterial;

		public Shader downsampleShader;

		private Material m_DownsampleMaterial;

		public Shader blendShader;

		private Material m_blendMaterial;
	}
}
