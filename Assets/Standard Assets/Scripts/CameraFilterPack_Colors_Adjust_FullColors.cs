using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/ColorsAdjust/FullColors")]
public class CameraFilterPack_Colors_Adjust_FullColors : MonoBehaviour
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
		this.SCShader = Shader.Find("CameraFilterPack/Colors_Adjust_FullColors");
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
			this.material.SetFloat("_Red_R", this.Red_R / 100f);
			this.material.SetFloat("_Red_G", this.Red_G / 100f);
			this.material.SetFloat("_Red_B", this.Red_B / 100f);
			this.material.SetFloat("_Green_R", this.Green_R / 100f);
			this.material.SetFloat("_Green_G", this.Green_G / 100f);
			this.material.SetFloat("_Green_B", this.Green_B / 100f);
			this.material.SetFloat("_Blue_R", this.Blue_R / 100f);
			this.material.SetFloat("_Blue_G", this.Blue_G / 100f);
			this.material.SetFloat("_Blue_B", this.Blue_B / 100f);
			this.material.SetFloat("_Red_C", this.Red_Constant / 100f);
			this.material.SetFloat("_Green_C", this.Green_Constant / 100f);
			this.material.SetFloat("_Blue_C", this.Blue_Constant / 100f);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
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

	[Range(-200f, 200f)]
	public float Red_R = 100f;

	[Range(-200f, 200f)]
	public float Red_G;

	[Range(-200f, 200f)]
	public float Red_B;

	[Range(-200f, 200f)]
	public float Red_Constant;

	[Range(-200f, 200f)]
	public float Green_R;

	[Range(-200f, 200f)]
	public float Green_G = 100f;

	[Range(-200f, 200f)]
	public float Green_B;

	[Range(-200f, 200f)]
	public float Green_Constant;

	[Range(-200f, 200f)]
	public float Blue_R;

	[Range(-200f, 200f)]
	public float Blue_G;

	[Range(-200f, 200f)]
	public float Blue_B = 100f;

	[Range(-200f, 200f)]
	public float Blue_Constant;
}
