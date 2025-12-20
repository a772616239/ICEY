using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Artefact")]
public class CameraFilterPack_TV_Artefact : MonoBehaviour
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
		this.SCShader = Shader.Find("CameraFilterPack/TV_Artefact");
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
			this.material.SetFloat("_Colorisation", this.Colorisation);
			this.material.SetFloat("_Parasite", this.Parasite);
			this.material.SetFloat("_Noise", this.Noise);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	public Shader SCShader;

	private Vector4 ScreenResolution;

	private float TimeX = 1f;

	[Range(-10f, 10f)]
	public float Colorisation = 1f;

	[Range(-10f, 10f)]
	public float Parasite = 1f;

	[Range(-10f, 10f)]
	public float Noise = 1f;

	private Material SCMaterial;
}
