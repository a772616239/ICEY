using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/8bits")]
public class CameraFilterPack_FX_8bits : MonoBehaviour
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
		CameraFilterPack_FX_8bits.ChangeBrightness = this.Brightness;
		CameraFilterPack_FX_8bits.ChangeResolutionX = this.ResolutionX;
		CameraFilterPack_FX_8bits.ChangeResolutionY = this.ResolutionY;
		this.SCShader = Shader.Find("CameraFilterPack/FX_8bits");
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
			RenderTexture temporary = RenderTexture.GetTemporary(this.ResolutionX, this.ResolutionY, 0);
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
		CameraFilterPack_FX_8bits.ChangeBrightness = this.Brightness;
		CameraFilterPack_FX_8bits.ChangeResolutionX = this.ResolutionX;
		CameraFilterPack_FX_8bits.ChangeResolutionY = this.ResolutionY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Brightness = CameraFilterPack_FX_8bits.ChangeBrightness;
			this.ResolutionX = CameraFilterPack_FX_8bits.ChangeResolutionX;
			this.ResolutionY = CameraFilterPack_FX_8bits.ChangeResolutionY;
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

	[Range(80f, 640f)]
	public int ResolutionX = 160;

	[Range(60f, 480f)]
	public int ResolutionY = 240;

	public static float ChangeBrightness;

	public static int ChangeResolutionX;

	public static int ChangeResolutionY;
}
