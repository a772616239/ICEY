using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Sharpen/Sharpen")]
public class CameraFilterPack_Sharpen_Sharpen : MonoBehaviour
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
		CameraFilterPack_Sharpen_Sharpen.ChangeValue = this.Value;
		CameraFilterPack_Sharpen_Sharpen.ChangeValue2 = this.Value2;
		this.SCShader = Shader.Find("CameraFilterPack/Sharpen_Sharpen");
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetFloat("_Value", this.Value);
			this.material.SetFloat("_Value2", this.Value2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Sharpen_Sharpen.ChangeValue = this.Value;
		CameraFilterPack_Sharpen_Sharpen.ChangeValue2 = this.Value2;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Value = CameraFilterPack_Sharpen_Sharpen.ChangeValue;
			this.Value2 = CameraFilterPack_Sharpen_Sharpen.ChangeValue2;
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

	[Range(0.001f, 100f)]
	public float Value = 4f;

	[Range(0.001f, 32f)]
	public float Value2 = 1f;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public static float ChangeValue;

	public static float ChangeValue2;
}
