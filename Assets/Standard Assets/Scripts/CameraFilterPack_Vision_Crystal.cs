using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/Crystal")]
public class CameraFilterPack_Vision_Crystal : MonoBehaviour
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
		CameraFilterPack_Vision_Crystal.ChangeValue = this.Value;
		CameraFilterPack_Vision_Crystal.ChangeValue2 = this.X;
		CameraFilterPack_Vision_Crystal.ChangeValue3 = this.Y;
		CameraFilterPack_Vision_Crystal.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Vision_Crystal");
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
			this.material.SetFloat("_Value", this.Value);
			this.material.SetFloat("_Value2", this.X);
			this.material.SetFloat("_Value3", this.Y);
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
		CameraFilterPack_Vision_Crystal.ChangeValue = this.Value;
		CameraFilterPack_Vision_Crystal.ChangeValue2 = this.X;
		CameraFilterPack_Vision_Crystal.ChangeValue3 = this.Y;
		CameraFilterPack_Vision_Crystal.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Value = CameraFilterPack_Vision_Crystal.ChangeValue;
			this.X = CameraFilterPack_Vision_Crystal.ChangeValue2;
			this.Y = CameraFilterPack_Vision_Crystal.ChangeValue3;
			this.Value4 = CameraFilterPack_Vision_Crystal.ChangeValue4;
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

	[Range(-10f, 10f)]
	public float Value = 1f;

	[Range(-1f, 1f)]
	public float X = 1f;

	[Range(-1f, 1f)]
	public float Y = 1f;

	[Range(-1f, 1f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
