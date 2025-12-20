using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Color_YUV")]
public class CameraFilterPack_Color_YUV : MonoBehaviour
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
		this._Y = CameraFilterPack_Color_YUV.Y;
		this._U = CameraFilterPack_Color_YUV.U;
		this._V = CameraFilterPack_Color_YUV.V;
		this.SCShader = Shader.Find("CameraFilterPack/Color_YUV");
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
			this.material.SetFloat("_Y", this._Y);
			this.material.SetFloat("_U", this._U);
			this.material.SetFloat("_V", this._V);
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
		CameraFilterPack_Color_YUV.Y = this._Y;
		CameraFilterPack_Color_YUV.U = this._U;
		CameraFilterPack_Color_YUV.V = this._V;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this._Y = CameraFilterPack_Color_YUV.Y;
			this._U = CameraFilterPack_Color_YUV.U;
			this._V = CameraFilterPack_Color_YUV.V;
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
	public float _Y;

	[Range(-1f, 1f)]
	public float _U;

	[Range(-1f, 1f)]
	public float _V;

	public static float Y;

	public static float U;

	public static float V;
}
