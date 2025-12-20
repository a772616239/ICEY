using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DigitalMatrixDistortion")]
public class CameraFilterPack_FX_DigitalMatrixDistortion : MonoBehaviour
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
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue = this.Size;
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue2 = this.Distortion;
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue5 = this.Speed;
		this.SCShader = Shader.Find("CameraFilterPack/FX_DigitalMatrixDistortion");
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Distortion);
			this.material.SetFloat("_Value5", this.Speed);
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
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue = this.Size;
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue2 = this.Distortion;
		CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue5 = this.Speed;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue;
			this.Distortion = CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue2;
			this.Speed = CameraFilterPack_FX_DigitalMatrixDistortion.ChangeValue5;
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

	[Range(0.4f, 5f)]
	public float Size = 1.4f;

	[Range(-2f, 2f)]
	public float Speed = 0.5f;

	[Range(-5f, 5f)]
	public float Distortion = 2.3f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	public static float ChangeValue5;
}
