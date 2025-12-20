using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/Snow_8bits")]
public class CameraFilterPack_Atmosphere_Snow_8bits : MonoBehaviour
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
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue = this.Threshold;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue2 = this.Size;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue3 = this.Value3;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Atmosphere_Snow_8bits");
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
			this.material.SetFloat("_Value", this.Threshold);
			this.material.SetFloat("_Value2", this.Size);
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
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue = this.Threshold;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue2 = this.Size;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue3 = this.Value3;
		CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Threshold = CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue;
			this.Size = CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue2;
			this.Value3 = CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue3;
			this.Value4 = CameraFilterPack_Atmosphere_Snow_8bits.ChangeValue4;
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

	[Range(0.9f, 2f)]
	public float Threshold = 1f;

	[Range(8f, 256f)]
	public float Size = 64f;

	[Range(0f, 10f)]
	private float Value3 = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
