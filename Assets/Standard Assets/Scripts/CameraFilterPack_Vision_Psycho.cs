using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Psycho")]
public class CameraFilterPack_Vision_Psycho : MonoBehaviour
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
		CameraFilterPack_Vision_Psycho.ChangeValue = this.HoleSize;
		CameraFilterPack_Vision_Psycho.ChangeValue2 = this.HoleSmooth;
		CameraFilterPack_Vision_Psycho.ChangeValue3 = this.Color1;
		CameraFilterPack_Vision_Psycho.ChangeValue4 = this.Color2;
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Psycho");
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
			this.material.SetFloat("_Value", this.HoleSize);
			this.material.SetFloat("_Value2", this.HoleSmooth);
			this.material.SetFloat("_Value3", this.Color1);
			this.material.SetFloat("_Value4", this.Color2);
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
		CameraFilterPack_Vision_Psycho.ChangeValue = this.HoleSize;
		CameraFilterPack_Vision_Psycho.ChangeValue2 = this.HoleSmooth;
		CameraFilterPack_Vision_Psycho.ChangeValue3 = this.Color1;
		CameraFilterPack_Vision_Psycho.ChangeValue4 = this.Color2;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.HoleSize = CameraFilterPack_Vision_Psycho.ChangeValue;
			this.HoleSmooth = CameraFilterPack_Vision_Psycho.ChangeValue2;
			this.Color1 = CameraFilterPack_Vision_Psycho.ChangeValue3;
			this.Color2 = CameraFilterPack_Vision_Psycho.ChangeValue4;
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

	[Range(0.01f, 1f)]
	public float HoleSize = 0.6f;

	[Range(-1f, 1f)]
	public float HoleSmooth = 0.3f;

	[Range(-2f, 2f)]
	public float Color1 = 0.2f;

	[Range(-2f, 2f)]
	public float Color2 = 0.9f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
