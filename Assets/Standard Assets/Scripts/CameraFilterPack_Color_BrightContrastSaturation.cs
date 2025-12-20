using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/BrightContrastSaturation")]
public class CameraFilterPack_Color_BrightContrastSaturation : MonoBehaviour
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
		CameraFilterPack_Color_BrightContrastSaturation.ChangeBrightness = this.Brightness;
		CameraFilterPack_Color_BrightContrastSaturation.ChangeSaturation = this.Saturation;
		CameraFilterPack_Color_BrightContrastSaturation.ChangeContrast = this.Contrast;
		this.SCShader = Shader.Find("CameraFilterPack/Color_BrightContrastSaturation");
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
			this.material.SetFloat("_Brightness", this.Brightness);
			this.material.SetFloat("_Saturation", this.Saturation);
			this.material.SetFloat("_Contrast", this.Contrast);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Color_BrightContrastSaturation.ChangeBrightness = this.Brightness;
		CameraFilterPack_Color_BrightContrastSaturation.ChangeSaturation = this.Saturation;
		CameraFilterPack_Color_BrightContrastSaturation.ChangeContrast = this.Contrast;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Brightness = CameraFilterPack_Color_BrightContrastSaturation.ChangeBrightness;
			this.Saturation = CameraFilterPack_Color_BrightContrastSaturation.ChangeSaturation;
			this.Contrast = CameraFilterPack_Color_BrightContrastSaturation.ChangeContrast;
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

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 10f)]
	public float Brightness = 2f;

	[Range(0f, 10f)]
	public float Saturation = 1.5f;

	[Range(0f, 10f)]
	public float Contrast = 1.5f;

	public static float ChangeBrightness;

	public static float ChangeSaturation;

	public static float ChangeContrast;
}
