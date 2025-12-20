using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Water_Drop")]
public class CameraFilterPack_Distortion_Water_Drop : MonoBehaviour
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
		CameraFilterPack_Distortion_Water_Drop.ChangeCenterX = this.CenterX;
		CameraFilterPack_Distortion_Water_Drop.ChangeCenterY = this.CenterY;
		CameraFilterPack_Distortion_Water_Drop.ChangeWaveIntensity = this.WaveIntensity;
		CameraFilterPack_Distortion_Water_Drop.ChangeNumberOfWaves = this.NumberOfWaves;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Water_Drop");
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
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			this.material.SetFloat("_WaveIntensity", this.WaveIntensity);
			this.material.SetInt("_NumberOfWaves", this.NumberOfWaves);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Distortion_Water_Drop.ChangeCenterX = this.CenterX;
		CameraFilterPack_Distortion_Water_Drop.ChangeCenterY = this.CenterY;
		CameraFilterPack_Distortion_Water_Drop.ChangeWaveIntensity = this.WaveIntensity;
		CameraFilterPack_Distortion_Water_Drop.ChangeNumberOfWaves = this.NumberOfWaves;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.CenterX = CameraFilterPack_Distortion_Water_Drop.ChangeCenterX;
			this.CenterY = CameraFilterPack_Distortion_Water_Drop.ChangeCenterY;
			this.WaveIntensity = CameraFilterPack_Distortion_Water_Drop.ChangeWaveIntensity;
			this.NumberOfWaves = CameraFilterPack_Distortion_Water_Drop.ChangeNumberOfWaves;
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

	[Range(-1f, 1f)]
	public float CenterX;

	[Range(-1f, 1f)]
	public float CenterY;

	[Range(0f, 10f)]
	public float WaveIntensity = 1f;

	[Range(0f, 20f)]
	public int NumberOfWaves = 5;

	public static float ChangeCenterX;

	public static float ChangeCenterY;

	public static float ChangeWaveIntensity;

	public static int ChangeNumberOfWaves;
}
