using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Toon")]
public class CameraFilterPack_Drawing_Toon : MonoBehaviour
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
		CameraFilterPack_Drawing_Toon.ChangeThreshold = this.Threshold;
		CameraFilterPack_Drawing_Toon.ChangeDotSize = this.DotSize;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Toon");
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
			this.material.SetFloat("_Distortion", this.Threshold);
			this.material.SetFloat("_DotSize", this.DotSize);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Drawing_Toon.ChangeThreshold = this.Threshold;
		CameraFilterPack_Drawing_Toon.ChangeDotSize = this.DotSize;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Threshold = CameraFilterPack_Drawing_Toon.ChangeThreshold;
			this.DotSize = CameraFilterPack_Drawing_Toon.ChangeDotSize;
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

	private Material SCMaterial;

	private float TimeX = 1f;

	[Range(0f, 2f)]
	public float Threshold = 1f;

	[Range(0f, 8f)]
	public float DotSize = 1f;

	public static float ChangeThreshold;

	public static float ChangeDotSize;
}
