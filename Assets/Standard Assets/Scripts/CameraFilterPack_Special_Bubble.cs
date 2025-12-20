using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Special/Bubble")]
public class CameraFilterPack_Special_Bubble : MonoBehaviour
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
		CameraFilterPack_Special_Bubble.ChangeValue = this.X;
		CameraFilterPack_Special_Bubble.ChangeValue2 = this.Y;
		CameraFilterPack_Special_Bubble.ChangeValue3 = this.Rate;
		CameraFilterPack_Special_Bubble.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Special_Bubble");
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
			this.material.SetFloat("_Value", this.X);
			this.material.SetFloat("_Value2", this.Y);
			this.material.SetFloat("_Value3", this.Rate);
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
		CameraFilterPack_Special_Bubble.ChangeValue = this.X;
		CameraFilterPack_Special_Bubble.ChangeValue2 = this.Y;
		CameraFilterPack_Special_Bubble.ChangeValue3 = this.Rate;
		CameraFilterPack_Special_Bubble.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.X = CameraFilterPack_Special_Bubble.ChangeValue;
			this.Y = CameraFilterPack_Special_Bubble.ChangeValue2;
			this.Rate = CameraFilterPack_Special_Bubble.ChangeValue3;
			this.Value4 = CameraFilterPack_Special_Bubble.ChangeValue4;
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

	[Range(-4f, 4f)]
	public float X = 0.5f;

	[Range(-4f, 4f)]
	public float Y = 0.5f;

	[Range(0f, 5f)]
	public float Rate = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
