using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Light/Water")]
public class CameraFilterPack_Light_Water : MonoBehaviour
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
		CameraFilterPack_Light_Water.ChangeAlpha = this.Alpha;
		CameraFilterPack_Light_Water.ChangeDistance = this.Distance;
		CameraFilterPack_Light_Water.ChangeSize = this.Size;
		this.SCShader = Shader.Find("CameraFilterPack/Light_Water");
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
			this.TimeX += Time.deltaTime * this.Speed;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Alpha", this.Alpha);
			this.material.SetFloat("_Distance", this.Distance);
			this.material.SetFloat("_Size", this.Size);
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
		CameraFilterPack_Light_Water.ChangeAlpha = this.Alpha;
		CameraFilterPack_Light_Water.ChangeDistance = this.Distance;
		CameraFilterPack_Light_Water.ChangeSize = this.Size;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Alpha = CameraFilterPack_Light_Water.ChangeAlpha;
			this.Distance = CameraFilterPack_Light_Water.ChangeDistance;
			this.Size = CameraFilterPack_Light_Water.ChangeSize;
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
	public float Size = 4f;

	[Range(0f, 2f)]
	public float Alpha = 0.07f;

	[Range(0f, 32f)]
	public float Distance = 10f;

	[Range(-2f, 2f)]
	public float Speed = 0.4f;

	public static float ChangeAlpha;

	public static float ChangeDistance;

	public static float ChangeSize;
}
