using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Vision/AuraDistortion")]
public class CameraFilterPack_Vision_AuraDistortion : MonoBehaviour
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
		CameraFilterPack_Vision_AuraDistortion.ChangeValue = this.Twist;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue2 = this.Color;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue3 = this.PosX;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue4 = this.PosY;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue5 = this.Speed;
		this.SCShader = Shader.Find("CameraFilterPack/Vision_AuraDistortion");
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
			this.material.SetFloat("_Value", this.Twist);
			this.material.SetColor("_Value2", this.Color);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
			this.material.SetFloat("_Value5", this.Speed);
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
		CameraFilterPack_Vision_AuraDistortion.ChangeValue = this.Twist;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue2 = this.Color;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue3 = this.PosX;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue4 = this.PosY;
		CameraFilterPack_Vision_AuraDistortion.ChangeValue5 = this.Speed;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Twist = CameraFilterPack_Vision_AuraDistortion.ChangeValue;
			this.Color = CameraFilterPack_Vision_AuraDistortion.ChangeValue2;
			this.PosX = CameraFilterPack_Vision_AuraDistortion.ChangeValue3;
			this.PosY = CameraFilterPack_Vision_AuraDistortion.ChangeValue4;
			this.Speed = CameraFilterPack_Vision_AuraDistortion.ChangeValue5;
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

	[Range(0f, 2f)]
	public float Twist = 1f;

	[Range(-4f, 4f)]
	public float Speed = 1f;

	public Color Color = new Color(0.16f, 0.57f, 0.19f);

	[Range(-1f, 2f)]
	public float PosX = 0.5f;

	[Range(-1f, 2f)]
	public float PosY = 0.5f;

	public static float ChangeValue;

	public static Color ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	public static float ChangeValue5;
}
