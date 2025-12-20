using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Blurry")]
public class CameraFilterPack_Blur_Blurry : MonoBehaviour
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
		CameraFilterPack_Blur_Blurry.ChangeAmount = this.Amount;
		CameraFilterPack_Blur_Blurry.ChangeFastFilter = this.FastFilter;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Blurry");
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
			int fastFilter = this.FastFilter;
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Amount", this.Amount);
			this.material.SetVector("_ScreenResolution", new Vector2((float)(Screen.width / fastFilter), (float)(Screen.height / fastFilter)));
			int width = sourceTexture.width / fastFilter;
			int height = sourceTexture.height / fastFilter;
			if (this.FastFilter > 1)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				temporary.filterMode = FilterMode.Trilinear;
				Graphics.Blit(sourceTexture, temporary, this.material);
				Graphics.Blit(temporary, destTexture);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else
			{
				Graphics.Blit(sourceTexture, destTexture, this.material);
			}
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Blur_Blurry.ChangeAmount = this.Amount;
		CameraFilterPack_Blur_Blurry.ChangeFastFilter = this.FastFilter;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Amount = CameraFilterPack_Blur_Blurry.ChangeAmount;
			this.FastFilter = CameraFilterPack_Blur_Blurry.ChangeFastFilter;
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

	[Range(0f, 20f)]
	public float Amount = 2f;

	[Range(1f, 8f)]
	public int FastFilter = 2;

	public static float ChangeAmount;

	public static int ChangeFastFilter;
}
