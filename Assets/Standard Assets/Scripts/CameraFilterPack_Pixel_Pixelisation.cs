using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Pixel/Pixelisation")]
public class CameraFilterPack_Pixel_Pixelisation : MonoBehaviour
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
		CameraFilterPack_Pixel_Pixelisation.ChangePixel = this._Pixelisation;
		CameraFilterPack_Pixel_Pixelisation.ChangePixelX = this._SizeX;
		CameraFilterPack_Pixel_Pixelisation.ChangePixelY = this._SizeY;
		this.SCShader = Shader.Find("CameraFilterPack/Pixel_Pixelisation");
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
			this.material.SetFloat("_Val", this._Pixelisation);
			this.material.SetFloat("_Val2", this._SizeX);
			this.material.SetFloat("_Val3", this._SizeY);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Pixel_Pixelisation.ChangePixel = this._Pixelisation;
		CameraFilterPack_Pixel_Pixelisation.ChangePixelX = this._SizeX;
		CameraFilterPack_Pixel_Pixelisation.ChangePixelY = this._SizeY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this._Pixelisation = CameraFilterPack_Pixel_Pixelisation.ChangePixel;
			this._SizeX = CameraFilterPack_Pixel_Pixelisation.ChangePixelX;
			this._SizeY = CameraFilterPack_Pixel_Pixelisation.ChangePixelY;
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

	[Range(0.6f, 120f)]
	public float _Pixelisation;

	[Range(0.6f, 120f)]
	public float _SizeX = 1f;

	[Range(0.6f, 120f)]
	public float _SizeY = 1f;

	private Material SCMaterial;

	public static float ChangePixel;

	public static float ChangePixelX;

	public static float ChangePixelY;
}
