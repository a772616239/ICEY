using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/EXTRA/Rotation")]
public class CameraFilterPack_EXTRA_Rotation : MonoBehaviour
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
		CameraFilterPack_EXTRA_Rotation.ChangeValue = this.Rotation;
		CameraFilterPack_EXTRA_Rotation.ChangeValue2 = this.PositionX;
		CameraFilterPack_EXTRA_Rotation.ChangeValue3 = this.PositionY;
		CameraFilterPack_EXTRA_Rotation.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/EXTRA_Rotation");
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
			this.material.SetFloat("_Value", -this.Rotation);
			this.material.SetFloat("_Value2", this.PositionX);
			this.material.SetFloat("_Value3", this.PositionY);
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
		CameraFilterPack_EXTRA_Rotation.ChangeValue = this.Rotation;
		CameraFilterPack_EXTRA_Rotation.ChangeValue2 = this.PositionX;
		CameraFilterPack_EXTRA_Rotation.ChangeValue3 = this.PositionY;
		CameraFilterPack_EXTRA_Rotation.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Rotation = CameraFilterPack_EXTRA_Rotation.ChangeValue;
			this.PositionX = CameraFilterPack_EXTRA_Rotation.ChangeValue2;
			this.PositionY = CameraFilterPack_EXTRA_Rotation.ChangeValue3;
			this.Value4 = CameraFilterPack_EXTRA_Rotation.ChangeValue4;
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

	[Range(-360f, 360f)]
	public float Rotation;

	[Range(-1f, 2f)]
	public float PositionX = 0.5f;

	[Range(-1f, 2f)]
	public float PositionY = 0.5f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
