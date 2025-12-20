using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/ShockWave")]
public class CameraFilterPack_Distortion_ShockWave : MonoBehaviour
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
		CameraFilterPack_Distortion_ShockWave.ChangeValue = this.PosX;
		CameraFilterPack_Distortion_ShockWave.ChangeValue2 = this.PosY;
		CameraFilterPack_Distortion_ShockWave.ChangeValue3 = this.Speed;
		CameraFilterPack_Distortion_ShockWave.ChangeValue4 = this.Size;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_ShockWave");
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
			this.material.SetFloat("_Value", this.PosX);
			this.material.SetFloat("_Value2", this.PosY);
			this.material.SetFloat("_Value3", this.Speed);
			this.material.SetFloat("_Value4", this.Size);
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
		CameraFilterPack_Distortion_ShockWave.ChangeValue = this.PosX;
		CameraFilterPack_Distortion_ShockWave.ChangeValue2 = this.PosY;
		CameraFilterPack_Distortion_ShockWave.ChangeValue3 = this.Speed;
		CameraFilterPack_Distortion_ShockWave.ChangeValue4 = this.Size;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.PosX = CameraFilterPack_Distortion_ShockWave.ChangeValue;
			this.PosY = CameraFilterPack_Distortion_ShockWave.ChangeValue2;
			this.Speed = CameraFilterPack_Distortion_ShockWave.ChangeValue3;
			this.Size = CameraFilterPack_Distortion_ShockWave.ChangeValue4;
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

	[Range(-1.5f, 1.5f)]
	public float PosX = 0.5f;

	[Range(-1.5f, 1.5f)]
	public float PosY = 0.5f;

	[Range(0f, 10f)]
	public float Speed = 1f;

	[Range(0f, 10f)]
	private float Size = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
