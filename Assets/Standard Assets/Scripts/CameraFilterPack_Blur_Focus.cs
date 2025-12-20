using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Focus")]
public class CameraFilterPack_Blur_Focus : MonoBehaviour
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
		CameraFilterPack_Blur_Focus.ChangeCenterX = this.CenterX;
		CameraFilterPack_Blur_Focus.ChangeCenterY = this.CenterY;
		CameraFilterPack_Blur_Focus.ChangeSize = this._Size;
		CameraFilterPack_Blur_Focus.ChangeEyes = this._Eyes;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Focus");
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
			this.material.SetFloat("_CenterX", this.CenterX);
			this.material.SetFloat("_CenterY", this.CenterY);
			float value = Mathf.Round(this._Size / 0.2f) * 0.2f;
			this.material.SetFloat("_Size", value);
			this.material.SetFloat("_Circle", this._Eyes);
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
		CameraFilterPack_Blur_Focus.ChangeCenterX = this.CenterX;
		CameraFilterPack_Blur_Focus.ChangeCenterY = this.CenterY;
		CameraFilterPack_Blur_Focus.ChangeSize = this._Size;
		CameraFilterPack_Blur_Focus.ChangeEyes = this._Eyes;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.CenterX = CameraFilterPack_Blur_Focus.ChangeCenterX;
			this.CenterY = CameraFilterPack_Blur_Focus.ChangeCenterY;
			this._Size = CameraFilterPack_Blur_Focus.ChangeSize;
			this._Eyes = CameraFilterPack_Blur_Focus.ChangeEyes;
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
	public float CenterX;

	[Range(-1f, 1f)]
	public float CenterY;

	[Range(0f, 10f)]
	public float _Size = 5f;

	[Range(0.12f, 64f)]
	public float _Eyes = 2f;

	public static float ChangeCenterX;

	public static float ChangeCenterY;

	public static float ChangeSize;

	public static float ChangeEyes;
}
