using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Bloom")]
public class CameraFilterPack_Blur_Bloom : MonoBehaviour
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
		CameraFilterPack_Blur_Bloom.ChangeAmount = this.Amount;
		CameraFilterPack_Blur_Bloom.ChangeGlow = this.Glow;
		this.SCShader = Shader.Find("CameraFilterPack/Blur_Bloom");
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
			this.material.SetFloat("_Amount", this.Amount);
			this.material.SetFloat("_Glow", this.Glow);
			this.material.SetVector("_ScreenResolution", new Vector2((float)Screen.width, (float)Screen.height));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Blur_Bloom.ChangeAmount = this.Amount;
		CameraFilterPack_Blur_Bloom.ChangeGlow = this.Glow;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Amount = CameraFilterPack_Blur_Bloom.ChangeAmount;
			this.Glow = CameraFilterPack_Blur_Bloom.ChangeGlow;
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
	public float Amount = 4.5f;

	[Range(0f, 1f)]
	public float Glow = 0.5f;

	public static float ChangeAmount;

	public static float ChangeGlow;
}
