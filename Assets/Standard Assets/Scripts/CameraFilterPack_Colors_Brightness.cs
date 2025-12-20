using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/Brightness")]
public class CameraFilterPack_Colors_Brightness : MonoBehaviour
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
		CameraFilterPack_Colors_Brightness.ChangeBrightness = this._Brightness;
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Brightness");
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
			this.material.SetFloat("_Val", this._Brightness);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Colors_Brightness.ChangeBrightness = this._Brightness;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this._Brightness = CameraFilterPack_Colors_Brightness.ChangeBrightness;
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

	[Range(0f, 2f)]
	public float _Brightness = 1.5f;

	private Material SCMaterial;

	public static float ChangeBrightness;
}
