using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/8bits_gb")]
public class CameraFilterPack_FX_8bits_gb : MonoBehaviour
{
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	private void Start()
	{
		CameraFilterPack_FX_8bits_gb.ChangeBrightness = this.Brightness;
		this.SCShader = Shader.Find("CameraFilterPack/FX_8bits_gb");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			if (this.Brightness == 0f)
			{
				this.Brightness = 0.001f;
			}
			this.material.SetFloat("_Distortion", this.Brightness);
			RenderTexture temporary = RenderTexture.GetTemporary(160, 144, 0);
			Graphics.Blit(sourceTexture, temporary, this.material);
			temporary.filterMode = FilterMode.Point;
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_FX_8bits_gb.ChangeBrightness = this.Brightness;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Brightness = CameraFilterPack_FX_8bits_gb.ChangeBrightness;
		}
	}

	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	public Shader SCShader;

	private float TimeX = 1f;

	private Material SCMaterial;

	[Range(-1f, 1f)]
	public float Brightness;

	public static float ChangeBrightness;
}
