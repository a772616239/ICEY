using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Noise")]
public class CameraFilterPack_Blur_Noise : MonoBehaviour
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
		CameraFilterPack_Blur_Noise.ChangeLevel = this.Level;
		CameraFilterPack_Blur_Noise.ChangeDistance = this.Distance;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Noise");
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
			this.material.SetFloat("_Level", (float)this.Level);
			this.material.SetVector("_Distance", this.Distance);
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
		CameraFilterPack_Blur_Noise.ChangeLevel = this.Level;
		CameraFilterPack_Blur_Noise.ChangeDistance = this.Distance;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Level = CameraFilterPack_Blur_Noise.ChangeLevel;
			this.Distance = CameraFilterPack_Blur_Noise.ChangeDistance;
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

	[Range(2f, 16f)]
	public int Level = 4;

	public Vector2 Distance = new Vector2(30f, 0f);

	public static int ChangeLevel;

	public static Vector2 ChangeDistance;
}
