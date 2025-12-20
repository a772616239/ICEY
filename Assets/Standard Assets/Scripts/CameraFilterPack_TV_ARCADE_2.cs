using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/ARCADE_2")]
public class CameraFilterPack_TV_ARCADE_2 : MonoBehaviour
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
		CameraFilterPack_TV_ARCADE_2.ChangeValue = this.Interferance_Size;
		CameraFilterPack_TV_ARCADE_2.ChangeValue2 = this.Interferance_Speed;
		CameraFilterPack_TV_ARCADE_2.ChangeValue3 = this.Contrast;
		CameraFilterPack_TV_ARCADE_2.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/TV_ARCADE_2");
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
			this.material.SetFloat("_Value", this.Interferance_Size);
			this.material.SetFloat("_Value2", this.Interferance_Speed);
			this.material.SetFloat("_Value3", this.Contrast);
			this.material.SetFloat("_Value4", this.Value4);
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
		CameraFilterPack_TV_ARCADE_2.ChangeValue = this.Interferance_Size;
		CameraFilterPack_TV_ARCADE_2.ChangeValue2 = this.Interferance_Speed;
		CameraFilterPack_TV_ARCADE_2.ChangeValue3 = this.Contrast;
		CameraFilterPack_TV_ARCADE_2.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Interferance_Size = CameraFilterPack_TV_ARCADE_2.ChangeValue;
			this.Interferance_Speed = CameraFilterPack_TV_ARCADE_2.ChangeValue2;
			this.Contrast = CameraFilterPack_TV_ARCADE_2.ChangeValue3;
			this.Value4 = CameraFilterPack_TV_ARCADE_2.ChangeValue4;
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

	[Range(0f, 10f)]
	public float Interferance_Size = 1f;

	[Range(0f, 10f)]
	public float Interferance_Speed = 0.5f;

	[Range(0f, 10f)]
	public float Contrast = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
