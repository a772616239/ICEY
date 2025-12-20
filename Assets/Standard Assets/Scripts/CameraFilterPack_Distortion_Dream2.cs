using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Dream2")]
public class CameraFilterPack_Distortion_Dream2 : MonoBehaviour
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
		CameraFilterPack_Distortion_Dream2.ChangeDistortion = this.Distortion;
		CameraFilterPack_Distortion_Dream2.ChangeSpeed = this.Speed;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Dream2");
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
			this.material.SetFloat("_Speed", this.Speed);
			this.material.SetFloat("_Distortion", this.Distortion);
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
		CameraFilterPack_Distortion_Dream2.ChangeDistortion = this.Distortion;
		CameraFilterPack_Distortion_Dream2.ChangeSpeed = this.Speed;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Distortion = CameraFilterPack_Distortion_Dream2.ChangeDistortion;
			this.Speed = CameraFilterPack_Distortion_Dream2.ChangeSpeed;
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

	[Range(0f, 100f)]
	public float Distortion = 6f;

	[Range(0f, 32f)]
	public float Speed = 5f;

	public static float ChangeDistortion;

	public static float ChangeSpeed;
}
