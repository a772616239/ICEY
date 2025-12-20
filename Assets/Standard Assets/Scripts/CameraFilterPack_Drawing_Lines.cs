using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Lines")]
public class CameraFilterPack_Drawing_Lines : MonoBehaviour
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
		CameraFilterPack_Drawing_Lines.ChangeValue = this.Number;
		CameraFilterPack_Drawing_Lines.ChangeValue2 = this.Random;
		CameraFilterPack_Drawing_Lines.ChangeValue3 = this.PositionY;
		CameraFilterPack_Drawing_Lines.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Lines");
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
			this.material.SetFloat("_Value", this.Number);
			this.material.SetFloat("_Value2", this.Random);
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
		CameraFilterPack_Drawing_Lines.ChangeValue = this.Number;
		CameraFilterPack_Drawing_Lines.ChangeValue2 = this.Random;
		CameraFilterPack_Drawing_Lines.ChangeValue3 = this.PositionY;
		CameraFilterPack_Drawing_Lines.ChangeValue4 = this.Value4;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Number = CameraFilterPack_Drawing_Lines.ChangeValue;
			this.Random = CameraFilterPack_Drawing_Lines.ChangeValue2;
			this.PositionY = CameraFilterPack_Drawing_Lines.ChangeValue3;
			this.Value4 = CameraFilterPack_Drawing_Lines.ChangeValue4;
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

	[UnityEngine.Range(0.1f, 10f)]
	public float Number = 1f;

	[UnityEngine.Range(0f, 1f)]
	public float Random = 0.5f;

	[UnityEngine.Range(0f, 10f)]
	private float PositionY = 1f;

	[UnityEngine.Range(0f, 10f)]
	private float Value4 = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
