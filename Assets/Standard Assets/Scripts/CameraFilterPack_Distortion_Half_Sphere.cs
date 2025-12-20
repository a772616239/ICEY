using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/Half_Sphere")]
public class CameraFilterPack_Distortion_Half_Sphere : MonoBehaviour
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
		CameraFilterPack_Distortion_Half_Sphere.ChangeSphereSize = this.SphereSize;
		CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionX = this.SpherePositionX;
		CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionY = this.SpherePositionY;
		CameraFilterPack_Distortion_Half_Sphere.ChangeStrength = this.Strength;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_Half_Sphere");
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
			this.material.SetFloat("_SphereSize", this.SphereSize);
			this.material.SetFloat("_SpherePositionX", this.SpherePositionX);
			this.material.SetFloat("_SpherePositionY", this.SpherePositionY);
			this.material.SetFloat("_Strength", this.Strength);
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
		CameraFilterPack_Distortion_Half_Sphere.ChangeSphereSize = this.SphereSize;
		CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionX = this.SpherePositionX;
		CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionY = this.SpherePositionY;
		CameraFilterPack_Distortion_Half_Sphere.ChangeStrength = this.Strength;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.SphereSize = CameraFilterPack_Distortion_Half_Sphere.ChangeSphereSize;
			this.SpherePositionX = CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionX;
			this.SpherePositionY = CameraFilterPack_Distortion_Half_Sphere.ChangeSpherePositionY;
			this.Strength = CameraFilterPack_Distortion_Half_Sphere.ChangeStrength;
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

	[Range(1f, 6f)]
	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public float SphereSize = 2.5f;

	[Range(-1f, 1f)]
	public float SpherePositionX;

	[Range(-1f, 1f)]
	public float SpherePositionY;

	[Range(1f, 10f)]
	public float Strength = 5f;

	public static float ChangeSphereSize;

	public static float ChangeSpherePositionX;

	public static float ChangeSpherePositionY;

	public static float ChangeStrength;
}
