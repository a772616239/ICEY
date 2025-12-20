using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Hypno")]
public class CameraFilterPack_FX_Hypno : MonoBehaviour
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
		CameraFilterPack_FX_Hypno.ChangeValue = this.Speed;
		CameraFilterPack_FX_Hypno.ChangeValue2 = this.Red;
		CameraFilterPack_FX_Hypno.ChangeValue3 = this.Green;
		CameraFilterPack_FX_Hypno.ChangeValue4 = this.Blue;
		this.SCShader = Shader.Find("CameraFilterPack/FX_Hypno");
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
			this.material.SetFloat("_Value", this.Speed);
			this.material.SetFloat("_Value2", this.Red);
			this.material.SetFloat("_Value3", this.Green);
			this.material.SetFloat("_Value4", this.Blue);
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
		CameraFilterPack_FX_Hypno.ChangeValue = this.Speed;
		CameraFilterPack_FX_Hypno.ChangeValue2 = this.Red;
		CameraFilterPack_FX_Hypno.ChangeValue3 = this.Green;
		CameraFilterPack_FX_Hypno.ChangeValue4 = this.Blue;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Speed = CameraFilterPack_FX_Hypno.ChangeValue;
			this.Red = CameraFilterPack_FX_Hypno.ChangeValue2;
			this.Green = CameraFilterPack_FX_Hypno.ChangeValue3;
			this.Blue = CameraFilterPack_FX_Hypno.ChangeValue4;
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

	[Range(0f, 1f)]
	public float Speed = 1f;

	[Range(-2f, 2f)]
	public float Red;

	[Range(-2f, 2f)]
	public float Green = 1f;

	[Range(-2f, 2f)]
	public float Blue = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
