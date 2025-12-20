using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Glitch/Mozaic")]
public class CameraFilterPack_Glitch_Mozaic : MonoBehaviour
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
		CameraFilterPack_Glitch_Mozaic.ChangeValue = this.Intensity;
		CameraFilterPack_Glitch_Mozaic.ChangeValue2 = this.Value2;
		CameraFilterPack_Glitch_Mozaic.ChangeValue3 = this.Value3;
		CameraFilterPack_Glitch_Mozaic.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Glitch_Mozaic");
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
			this.material.SetFloat("_Value2", this.Value2);
			this.material.SetFloat("_Value3", this.Value3);
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
		CameraFilterPack_Glitch_Mozaic.ChangeValue = this.Intensity;
		CameraFilterPack_Glitch_Mozaic.ChangeValue2 = this.Value2;
		CameraFilterPack_Glitch_Mozaic.ChangeValue3 = this.Value3;
		CameraFilterPack_Glitch_Mozaic.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Intensity = CameraFilterPack_Glitch_Mozaic.ChangeValue;
			this.Value2 = CameraFilterPack_Glitch_Mozaic.ChangeValue2;
			this.Value3 = CameraFilterPack_Glitch_Mozaic.ChangeValue3;
			this.Value4 = CameraFilterPack_Glitch_Mozaic.ChangeValue4;
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
	public float Intensity = 1f;

	[Range(0f, 10f)]
	private float Value2 = 1f;

	[Range(0f, 10f)]
	private float Value3 = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
