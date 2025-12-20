using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/EnhancedComics")]
public class CameraFilterPack_Drawing_EnhancedComics : MonoBehaviour
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
		CameraFilterPack_Drawing_EnhancedComics.ChangeDotSize = this.DotSize;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_EnhancedComics");
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
			this.material.SetFloat("_DotSize", this.DotSize);
			this.material.SetFloat("_ColorR", this._ColorR);
			this.material.SetFloat("_ColorG", this._ColorG);
			this.material.SetFloat("_ColorB", this._ColorB);
			this.material.SetFloat("_Blood", this._Blood);
			this.material.SetColor("_ColorRGB", this.ColorRGB);
			this.material.SetFloat("_SmoothStart", this._SmoothStart);
			this.material.SetFloat("_SmoothEnd", this._SmoothEnd);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Drawing_EnhancedComics.ChangeDotSize = this.DotSize;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.DotSize = CameraFilterPack_Drawing_EnhancedComics.ChangeDotSize;
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

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float DotSize = 0.15f;

	[Range(0f, 1f)]
	public float _ColorR = 0.9f;

	[Range(0f, 1f)]
	public float _ColorG = 0.4f;

	[Range(0f, 1f)]
	public float _ColorB = 0.4f;

	[Range(0f, 1f)]
	public float _Blood = 0.5f;

	[Range(0f, 1f)]
	public float _SmoothStart = 0.02f;

	[Range(0f, 1f)]
	public float _SmoothEnd = 0.1f;

	public Color ColorRGB = new Color(1f, 0f, 0f);

	public static float ChangeDotSize;
}
