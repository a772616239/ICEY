using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Screens")]
public class CameraFilterPack_FX_Screens : MonoBehaviour
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
		CameraFilterPack_FX_Screens.ChangeValue = this.Tiles;
		CameraFilterPack_FX_Screens.ChangeValue2 = this.Speed;
		CameraFilterPack_FX_Screens.ChangeValue3 = this.PosX;
		CameraFilterPack_FX_Screens.ChangeValue4 = this.PosY;
		this.SCShader = Shader.Find("CameraFilterPack/FX_Screens");
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
			this.material.SetFloat("_Value", this.Tiles);
			this.material.SetFloat("_Value2", this.Speed);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
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
		CameraFilterPack_FX_Screens.ChangeValue = this.Tiles;
		CameraFilterPack_FX_Screens.ChangeValue2 = this.Speed;
		CameraFilterPack_FX_Screens.ChangeValue3 = this.PosX;
		CameraFilterPack_FX_Screens.ChangeValue4 = this.PosY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Tiles = CameraFilterPack_FX_Screens.ChangeValue;
			this.Speed = CameraFilterPack_FX_Screens.ChangeValue2;
			this.PosX = CameraFilterPack_FX_Screens.ChangeValue3;
			this.PosY = CameraFilterPack_FX_Screens.ChangeValue4;
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

	[Range(0f, 256f)]
	public float Tiles = 8f;

	[Range(0f, 5f)]
	public float Speed = 0.25f;

	[Range(-1f, 1f)]
	public float PosX;

	[Range(-1f, 1f)]
	public float PosY;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
