using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Flush")]
public class CameraFilterPack_Distortion_Flush : MonoBehaviour
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
		CameraFilterPack_Distortion_Flush.ChangeValue = this.Size;
		CameraFilterPack_Distortion_Flush.ChangeValue2 = this.LightBackGround;
		CameraFilterPack_Distortion_Flush.ChangeValue3 = this.Speed;
		CameraFilterPack_Distortion_Flush.ChangeValue4 = this.Size2;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Flush");
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
			this.material.SetFloat("_Value2", this.LightBackGround);
			this.material.SetFloat("_Value3", this.Speed);
			this.material.SetFloat("_Value4", this.Size2);
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
		CameraFilterPack_Distortion_Flush.ChangeValue = this.Size;
		CameraFilterPack_Distortion_Flush.ChangeValue2 = this.LightBackGround;
		CameraFilterPack_Distortion_Flush.ChangeValue3 = this.Speed;
		CameraFilterPack_Distortion_Flush.ChangeValue4 = this.Size2;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_Distortion_Flush.ChangeValue;
			this.LightBackGround = CameraFilterPack_Distortion_Flush.ChangeValue2;
			this.Speed = CameraFilterPack_Distortion_Flush.ChangeValue3;
			this.Size2 = CameraFilterPack_Distortion_Flush.ChangeValue4;
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

	[Range(0.0001f, 0.5f)]
	private float Size = 0.01f;

	[Range(-1.5f, 1.5f)]
	private float LightBackGround = 0.5f;

	[Range(0f, 10f)]
	private float Speed = 1f;

	[Range(0f, 10f)]
	private float Size2 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
