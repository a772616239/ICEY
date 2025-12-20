using System;
using UnityEngine;

namespace Distort
{
	public class DistortEffectCamera : MonoBehaviour
	{
		private void Start()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.rt;
			Graphics.Blit(source, this.rt);
			RenderTexture.active = active;
			this.RenderMaterial.SetTexture("_MainTex", this.rt);
			Graphics.Blit(source, destination);
		}

		public Material RenderMaterial;

		public RenderTexture rt;
	}
}
