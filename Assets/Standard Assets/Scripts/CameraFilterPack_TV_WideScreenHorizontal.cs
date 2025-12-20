using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/WideScreenHorizontal")]
public class CameraFilterPack_TV_WideScreenHorizontal : MonoBehaviour
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
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue = this.Size;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue2 = this.Smooth;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue3 = this.StretchX;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue4 = this.StretchY;
		this.SCShader = Shader.Find("CameraFilterPack/TV_WideScreenHorizontal");
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
			this.material.SetFloat("_Value2", this.Smooth);
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
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue = this.Size;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue2 = this.Smooth;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue3 = this.StretchX;
		CameraFilterPack_TV_WideScreenHorizontal.ChangeValue4 = this.StretchY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_TV_WideScreenHorizontal.ChangeValue;
			this.Smooth = CameraFilterPack_TV_WideScreenHorizontal.ChangeValue2;
			this.StretchX = CameraFilterPack_TV_WideScreenHorizontal.ChangeValue3;
			this.StretchY = CameraFilterPack_TV_WideScreenHorizontal.ChangeValue4;
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

	[Range(0f, 0.8f)]
	public float Size = 0.55f;

	[Range(0.001f, 0.4f)]
	public float Smooth = 0.01f;

	[Range(0f, 10f)]
	private float StretchX = 1f;

	[Range(0f, 10f)]
	private float StretchY = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
