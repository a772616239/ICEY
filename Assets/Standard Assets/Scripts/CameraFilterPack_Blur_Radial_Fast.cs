using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Radial_Fast")]
public class CameraFilterPack_Blur_Radial_Fast : MonoBehaviour
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
		CameraFilterPack_Blur_Radial_Fast.ChangeValue = this.Intensity;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue2 = this.MovX;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue3 = this.MovY;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue4 = this.blurWidth;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Radial_Fast");
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
			this.material.SetFloat("_Value", this.Intensity);
			this.material.SetFloat("_Value2", this.MovX);
			this.material.SetFloat("_Value3", this.MovY);
			this.material.SetFloat("_Value4", this.blurWidth);
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
		CameraFilterPack_Blur_Radial_Fast.ChangeValue = this.Intensity;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue2 = this.MovX;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue3 = this.MovY;
		CameraFilterPack_Blur_Radial_Fast.ChangeValue4 = this.blurWidth;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Intensity = CameraFilterPack_Blur_Radial_Fast.ChangeValue;
			this.MovX = CameraFilterPack_Blur_Radial_Fast.ChangeValue2;
			this.MovY = CameraFilterPack_Blur_Radial_Fast.ChangeValue3;
			this.blurWidth = CameraFilterPack_Blur_Radial_Fast.ChangeValue4;
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

	[Range(-0.5f, 0.5f)]
	public float Intensity = 0.125f;

	[Range(-2f, 2f)]
	public float MovX = 0.5f;

	[Range(-2f, 2f)]
	public float MovY = 0.5f;

	[Range(0f, 10f)]
	private float blurWidth = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
