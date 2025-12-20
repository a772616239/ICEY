using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/DigitalMatrix")]
public class CameraFilterPack_FX_DigitalMatrix : MonoBehaviour
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
		CameraFilterPack_FX_DigitalMatrix.ChangeValue = this.Size;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue2 = this.ColorR;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue3 = this.ColorG;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue4 = this.ColorB;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue5 = this.Speed;
		this.SCShader = Shader.Find("CameraFilterPack/FX_DigitalMatrix");
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
			this.material.SetFloat("_Value2", this.ColorR);
			this.material.SetFloat("_Value3", this.ColorG);
			this.material.SetFloat("_Value4", this.ColorB);
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
		CameraFilterPack_FX_DigitalMatrix.ChangeValue = this.Size;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue2 = this.ColorR;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue3 = this.ColorG;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue4 = this.ColorB;
		CameraFilterPack_FX_DigitalMatrix.ChangeValue5 = this.Speed;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_FX_DigitalMatrix.ChangeValue;
			this.ColorR = CameraFilterPack_FX_DigitalMatrix.ChangeValue2;
			this.ColorG = CameraFilterPack_FX_DigitalMatrix.ChangeValue3;
			this.ColorB = CameraFilterPack_FX_DigitalMatrix.ChangeValue4;
			this.Speed = CameraFilterPack_FX_DigitalMatrix.ChangeValue5;
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
	public float Size = 1f;

	[Range(-10f, 10f)]
	public float Speed = 1f;

	[Range(-1f, 1f)]
	public float ColorR = -1f;

	[Range(-1f, 1f)]
	public float ColorG = 1f;

	[Range(-1f, 1f)]
	public float ColorB = -1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	public static float ChangeValue5;
}
