using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Steam")]
public class CameraFilterPack_Blur_Steam : MonoBehaviour
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
		CameraFilterPack_Blur_Steam.ChangeRadius = this.Radius;
		CameraFilterPack_Blur_Steam.ChangeQuality = this.Quality;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Steam");
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
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetFloat("_Quality", this.Quality);
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
		CameraFilterPack_Blur_Steam.ChangeRadius = this.Radius;
		CameraFilterPack_Blur_Steam.ChangeQuality = this.Quality;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Radius = CameraFilterPack_Blur_Steam.ChangeRadius;
			this.Quality = CameraFilterPack_Blur_Steam.ChangeQuality;
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

	[Range(0f, 1f)]
	public float Radius = 0.1f;

	[Range(0f, 1f)]
	public float Quality = 0.75f;

	public static float ChangeRadius;

	public static float ChangeQuality;
}
