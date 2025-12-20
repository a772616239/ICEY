using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/HSV")]
public class CameraFilterPack_Colors_HSV : MonoBehaviour
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
		CameraFilterPack_Colors_HSV.ChangeHueShift = this._HueShift;
		CameraFilterPack_Colors_HSV.ChangeSaturation = this._Saturation;
		CameraFilterPack_Colors_HSV.ChangeValueBrightness = this._ValueBrightness;
		this.SCShader = Shader.Find("CameraFilterPack/Colors_HSV");
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
			this.material.SetFloat("_HueShift", this._HueShift);
			this.material.SetFloat("_Sat", this._Saturation);
			this.material.SetFloat("_Val", this._ValueBrightness);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Colors_HSV.ChangeHueShift = this._HueShift;
		CameraFilterPack_Colors_HSV.ChangeSaturation = this._Saturation;
		CameraFilterPack_Colors_HSV.ChangeValueBrightness = this._ValueBrightness;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this._HueShift = CameraFilterPack_Colors_HSV.ChangeHueShift;
			this._Saturation = CameraFilterPack_Colors_HSV.ChangeSaturation;
			this._ValueBrightness = CameraFilterPack_Colors_HSV.ChangeValueBrightness;
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

	[Range(0f, 360f)]
	public float _HueShift = 180f;

	[Range(-32f, 32f)]
	public float _Saturation = 1f;

	[Range(-32f, 32f)]
	public float _ValueBrightness = 1f;

	private Material SCMaterial;

	public static float ChangeHueShift;

	public static float ChangeSaturation;

	public static float ChangeValueBrightness;
}
