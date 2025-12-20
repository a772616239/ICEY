using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Night Vision/Night Vision 1")]
public class CameraFilterPack_Oculus_NightVision1 : MonoBehaviour
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
		CameraFilterPack_Oculus_NightVision1.ChangeVignette = this.Vignette;
		CameraFilterPack_Oculus_NightVision1.ChangeLinecount = this.Linecount;
		this.SCShader = Shader.Find("CameraFilterPack/Oculus_NightVision1");
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
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			this.material.SetFloat("_Vignette", this.Vignette);
			this.material.SetFloat("_Linecount", this.Linecount);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
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

	private float Distortion = 1f;

	private Material SCMaterial;

	private Vector4 ScreenResolution;

	[Range(0f, 100f)]
	public float Vignette = 1.3f;

	[Range(1f, 150f)]
	public float Linecount = 90f;

	public static float ChangeVignette;

	public static float ChangeLinecount;
}
