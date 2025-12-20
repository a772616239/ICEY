using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Lens")]
public class CameraFilterPack_Distortion_Lens : MonoBehaviour
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
		CameraFilterPack_Distortion_Lens.ChangeCenterX = this.CenterX;
		CameraFilterPack_Distortion_Lens.ChangeCenterY = this.CenterY;
		CameraFilterPack_Distortion_Lens.ChangeDistortion = this.Distortion;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Lens");
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
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Distortion_Lens.ChangeCenterX = this.CenterX;
		CameraFilterPack_Distortion_Lens.ChangeCenterY = this.CenterY;
		CameraFilterPack_Distortion_Lens.ChangeDistortion = this.Distortion;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.CenterX = CameraFilterPack_Distortion_Lens.ChangeCenterX;
			this.CenterY = CameraFilterPack_Distortion_Lens.ChangeCenterY;
			this.Distortion = CameraFilterPack_Distortion_Lens.ChangeDistortion;
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

	[Range(0f, 3f)]
	public float Distortion = 1f;

	public static float ChangeCenterX;

	public static float ChangeCenterY;

	public static float ChangeDistortion;
}
