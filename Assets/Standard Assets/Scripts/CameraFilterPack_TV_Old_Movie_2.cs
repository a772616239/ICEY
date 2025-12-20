using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Old Film/Old_Movie_2")]
public class CameraFilterPack_TV_Old_Movie_2 : MonoBehaviour
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
		CameraFilterPack_TV_Old_Movie_2.ChangeValue = this.FramePerSecond;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue2 = this.Contrast;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue3 = this.Burn;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue4 = this.SceneCut;
		this.SCShader = Shader.Find("CameraFilterPack/TV_Old_Movie_2");
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
			this.material.SetFloat("_Value", this.FramePerSecond);
			this.material.SetFloat("_Value2", this.Contrast);
			this.material.SetFloat("_Value3", this.Burn);
			this.material.SetFloat("_Value4", this.SceneCut);
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
		CameraFilterPack_TV_Old_Movie_2.ChangeValue = this.FramePerSecond;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue2 = this.Contrast;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue3 = this.Burn;
		CameraFilterPack_TV_Old_Movie_2.ChangeValue4 = this.SceneCut;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.FramePerSecond = CameraFilterPack_TV_Old_Movie_2.ChangeValue;
			this.Contrast = CameraFilterPack_TV_Old_Movie_2.ChangeValue2;
			this.Burn = CameraFilterPack_TV_Old_Movie_2.ChangeValue3;
			this.SceneCut = CameraFilterPack_TV_Old_Movie_2.ChangeValue4;
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

	[Range(1f, 60f)]
	public float FramePerSecond = 15f;

	[Range(0f, 5f)]
	public float Contrast = 1f;

	[Range(0f, 4f)]
	public float Burn;

	[Range(0f, 16f)]
	public float SceneCut = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
