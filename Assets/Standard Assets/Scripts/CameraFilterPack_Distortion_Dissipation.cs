using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Dissipation")]
public class CameraFilterPack_Distortion_Dissipation : MonoBehaviour
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
		CameraFilterPack_Distortion_Dissipation.ChangeValue = this.Dissipation;
		CameraFilterPack_Distortion_Dissipation.ChangeValue2 = this.Colors;
		CameraFilterPack_Distortion_Dissipation.ChangeValue3 = this.Green_Mod;
		CameraFilterPack_Distortion_Dissipation.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Dissipation");
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
			this.material.SetFloat("_Value", this.Dissipation);
			this.material.SetFloat("_Value2", this.Colors);
			this.material.SetFloat("_Value3", this.Green_Mod);
			this.material.SetFloat("_Value4", this.Value4);
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
		CameraFilterPack_Distortion_Dissipation.ChangeValue = this.Dissipation;
		CameraFilterPack_Distortion_Dissipation.ChangeValue2 = this.Colors;
		CameraFilterPack_Distortion_Dissipation.ChangeValue3 = this.Green_Mod;
		CameraFilterPack_Distortion_Dissipation.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Dissipation = CameraFilterPack_Distortion_Dissipation.ChangeValue;
			this.Colors = CameraFilterPack_Distortion_Dissipation.ChangeValue2;
			this.Green_Mod = CameraFilterPack_Distortion_Dissipation.ChangeValue3;
			this.Value4 = CameraFilterPack_Distortion_Dissipation.ChangeValue4;
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

	[Range(0f, 2.99f)]
	public float Dissipation = 1f;

	[Range(0f, 16f)]
	private float Colors = 11f;

	[Range(-1f, 1f)]
	private float Green_Mod = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
