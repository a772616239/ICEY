using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Blur Hole")]
public class CameraFilterPack_Blur_BlurHole : MonoBehaviour
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
		CameraFilterPack_Blur_BlurHole.ChangeSize = this.Size;
		this.SCShader = Shader.Find("CameraFilterPack/BlurHole");
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
			this.material.SetFloat("_Distortion", this.Size);
			this.material.SetFloat("_Radius", this._Radius);
			this.material.SetFloat("_SpotSize", this._SpotSize);
			this.material.SetFloat("_CenterX", this._CenterX);
			this.material.SetFloat("_CenterY", this._CenterY);
			this.material.SetFloat("_Alpha", this._AlphaBlur);
			this.material.SetFloat("_Alpha2", this._AlphaBlurInside);
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
		CameraFilterPack_Blur_BlurHole.ChangeSize = this.Size;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_Blur_BlurHole.ChangeSize;
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

	[Range(1f, 16f)]
	public float Size = 10f;

	[Range(0f, 1f)]
	public float _Radius = 0.25f;

	[Range(0f, 4f)]
	public float _SpotSize = 1f;

	[Range(0f, 1f)]
	public float _CenterX = 0.5f;

	[Range(0f, 1f)]
	public float _CenterY = 0.5f;

	[Range(0f, 1f)]
	public float _AlphaBlur = 1f;

	[Range(0f, 1f)]
	public float _AlphaBlurInside;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public static float ChangeSize;
}
