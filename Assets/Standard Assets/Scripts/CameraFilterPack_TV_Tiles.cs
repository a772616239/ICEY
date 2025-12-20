using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Tiles")]
public class CameraFilterPack_TV_Tiles : MonoBehaviour
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
		CameraFilterPack_TV_Tiles.ChangeValue = this.Size;
		CameraFilterPack_TV_Tiles.ChangeValue2 = this.Intensity;
		CameraFilterPack_TV_Tiles.ChangeValue3 = this.StretchX;
		CameraFilterPack_TV_Tiles.ChangeValue4 = this.StretchY;
		this.SCShader = Shader.Find("CameraFilterPack/TV_Tiles");
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
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", this.Intensity);
			this.material.SetFloat("_Value3", this.StretchX);
			this.material.SetFloat("_Value4", this.StretchY);
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
		CameraFilterPack_TV_Tiles.ChangeValue = this.Size;
		CameraFilterPack_TV_Tiles.ChangeValue2 = this.Intensity;
		CameraFilterPack_TV_Tiles.ChangeValue3 = this.StretchX;
		CameraFilterPack_TV_Tiles.ChangeValue4 = this.StretchY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_TV_Tiles.ChangeValue;
			this.Intensity = CameraFilterPack_TV_Tiles.ChangeValue2;
			this.StretchX = CameraFilterPack_TV_Tiles.ChangeValue3;
			this.StretchY = CameraFilterPack_TV_Tiles.ChangeValue4;
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

	[Range(0.5f, 2f)]
	public float Size = 1f;

	[Range(0f, 10f)]
	public float Intensity = 4f;

	[Range(0f, 1f)]
	public float StretchX = 0.6f;

	[Range(0f, 1f)]
	public float StretchY = 0.4f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
