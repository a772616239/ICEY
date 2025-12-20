using System;
using UnityEngine;

namespace Xft
{
	public class ColorInverseEvent : CameraEffectEvent
	{
		public ColorInverseEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.ColorInverse, owner)
		{
			this.ColorInverseShader = owner.ColorInverseShader;
		}

		public Material MyMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.ColorInverseShader);
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
			this.m_strength = this.m_owner.CIStrengthCurve.Evaluate(this.m_elapsedTime);
		}

		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.MyMaterial.SetFloat("_Strength", this.m_strength);
			Graphics.Blit(source, destination, this.MyMaterial);
		}

		protected float m_strength;

		public Shader ColorInverseShader;

		protected Material m_material;
	}
}
