using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/ColorsAdjust/ColorRGB")]
public class CameraFilterPack_Colors_Adjust_ColorRGB : MonoBehaviour
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
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue = this.Red;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue2 = this.Green;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue3 = this.Blue;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue4 = this.Brightness;
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Adjust_ColorRGB");
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
			this.material.SetFloat("_Value", this.Red);
			this.material.SetFloat("_Value2", this.Green);
			this.material.SetFloat("_Value3", this.Blue);
			this.material.SetFloat("_Value4", this.Brightness);
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
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue = this.Red;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue2 = this.Green;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue3 = this.Blue;
		CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue4 = this.Brightness;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Red = CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue;
			this.Green = CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue2;
			this.Blue = CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue3;
			this.Brightness = CameraFilterPack_Colors_Adjust_ColorRGB.ChangeValue4;
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

	[Range(-2f, 2f)]
	public float Red;

	[Range(-2f, 2f)]
	public float Green;

	[Range(-2f, 2f)]
	public float Blue;

	[Range(-1f, 1f)]
	public float Brightness;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
