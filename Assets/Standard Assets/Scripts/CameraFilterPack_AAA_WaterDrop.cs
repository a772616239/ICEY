using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/WaterDrop")]
public class CameraFilterPack_AAA_WaterDrop : MonoBehaviour
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
		CameraFilterPack_AAA_WaterDrop.ChangeDistortion = this.Distortion;
		CameraFilterPack_AAA_WaterDrop.ChangeSizeX = this.SizeX;
		CameraFilterPack_AAA_WaterDrop.ChangeSizeY = this.SizeY;
		CameraFilterPack_AAA_WaterDrop.ChangeSpeed = this.Speed;
		this.Texture2 = (Resources.Load("CameraFilterPack_WaterDrop") as Texture2D);
		this.SCShader = Shader.Find("CameraFilterPack/AAA_WaterDrop");
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetFloat("_SizeX", this.SizeX);
			this.material.SetFloat("_SizeY", this.SizeY);
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_AAA_WaterDrop.ChangeDistortion = this.Distortion;
		CameraFilterPack_AAA_WaterDrop.ChangeSizeX = this.SizeX;
		CameraFilterPack_AAA_WaterDrop.ChangeSizeY = this.SizeY;
		CameraFilterPack_AAA_WaterDrop.ChangeSpeed = this.Speed;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Distortion = CameraFilterPack_AAA_WaterDrop.ChangeDistortion;
			this.SizeX = CameraFilterPack_AAA_WaterDrop.ChangeSizeX;
			this.SizeY = CameraFilterPack_AAA_WaterDrop.ChangeSizeY;
			this.Speed = CameraFilterPack_AAA_WaterDrop.ChangeSpeed;
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

	[Range(8f, 64f)]
	public float Distortion = 8f;

	[Range(0f, 7f)]
	public float SizeX = 1f;

	[Range(0f, 7f)]
	public float SizeY = 0.5f;

	[Range(0f, 10f)]
	public float Speed = 1f;

	private Material SCMaterial;

	private Texture2D Texture2;

	public static float ChangeDistortion;

	public static float ChangeSizeX;

	public static float ChangeSizeY;

	public static float ChangeSpeed;
}
