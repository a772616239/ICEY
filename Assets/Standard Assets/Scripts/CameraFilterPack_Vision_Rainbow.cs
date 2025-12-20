using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Rainbow")]
public class CameraFilterPack_Vision_Rainbow : MonoBehaviour
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
		CameraFilterPack_Vision_Rainbow.ChangeValue = this.Speed;
		CameraFilterPack_Vision_Rainbow.ChangeValue2 = this.PosX;
		CameraFilterPack_Vision_Rainbow.ChangeValue3 = this.PosY;
		CameraFilterPack_Vision_Rainbow.ChangeValue4 = this.Colors;
		CameraFilterPack_Vision_Rainbow.ChangeValue5 = this.Vision;
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Rainbow");
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.PosX);
			this.material.SetFloat("_Value3", this.PosY);
			this.material.SetFloat("_Value4", this.Colors);
			this.material.SetFloat("_Value5", this.Vision);
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
		CameraFilterPack_Vision_Rainbow.ChangeValue = this.Speed;
		CameraFilterPack_Vision_Rainbow.ChangeValue2 = this.PosX;
		CameraFilterPack_Vision_Rainbow.ChangeValue3 = this.PosY;
		CameraFilterPack_Vision_Rainbow.ChangeValue4 = this.Colors;
		CameraFilterPack_Vision_Rainbow.ChangeValue5 = this.Vision;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Speed = CameraFilterPack_Vision_Rainbow.ChangeValue;
			this.PosX = CameraFilterPack_Vision_Rainbow.ChangeValue2;
			this.PosY = CameraFilterPack_Vision_Rainbow.ChangeValue3;
			this.Colors = CameraFilterPack_Vision_Rainbow.ChangeValue4;
			this.Vision = CameraFilterPack_Vision_Rainbow.ChangeValue5;
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
	public float Speed = 1f;

	[Range(0f, 1f)]
	public float PosX = 0.5f;

	[Range(0f, 1f)]
	public float PosY = 0.5f;

	[Range(0f, 5f)]
	public float Colors = 0.5f;

	[Range(0f, 1f)]
	public float Vision = 0.5f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	public static float ChangeValue5;
}
