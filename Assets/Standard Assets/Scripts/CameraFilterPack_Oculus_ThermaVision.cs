using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/ThermaVision")]
public class CameraFilterPack_Oculus_ThermaVision : MonoBehaviour
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
		CameraFilterPack_Oculus_ThermaVision.ChangeValue = this.Therma_Variation;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue2 = this.Contrast;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue3 = this.Burn;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue4 = this.SceneCut;
		this.SCShader = Shader.Find("CameraFilterPack/Oculus_ThermaVision");
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
			this.material.SetFloat("_Value", this.Therma_Variation);
			this.material.SetFloat("_Value2", this.Contrast);
			this.material.SetFloat("_Value3", this.Burn);
			this.material.SetFloat("_Value4", this.SceneCut);
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
		CameraFilterPack_Oculus_ThermaVision.ChangeValue = this.Therma_Variation;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue2 = this.Contrast;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue3 = this.Burn;
		CameraFilterPack_Oculus_ThermaVision.ChangeValue4 = this.SceneCut;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Therma_Variation = CameraFilterPack_Oculus_ThermaVision.ChangeValue;
			this.Contrast = CameraFilterPack_Oculus_ThermaVision.ChangeValue2;
			this.Burn = CameraFilterPack_Oculus_ThermaVision.ChangeValue3;
			this.SceneCut = CameraFilterPack_Oculus_ThermaVision.ChangeValue4;
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

	[Range(0f, 1f)]
	public float Therma_Variation = 0.5f;

	[Range(0f, 8f)]
	private float Contrast = 3f;

	[Range(0f, 4f)]
	private float Burn;

	[Range(0f, 16f)]
	private float SceneCut = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
