using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/TV/Broken Glass2")]
public class CameraFilterPack_TV_BrokenGlass2 : MonoBehaviour
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
		this.Texture2 = (Resources.Load("CameraFilterPack_TV_BrokenGlass_2") as Texture2D);
		this.SCShader = Shader.Find("CameraFilterPack/TV_BrokenGlass2");
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
			if (this.Bullet_1 < 0f)
			{
				this.Bullet_1 = 0f;
			}
			if (this.Bullet_2 < 0f)
			{
				this.Bullet_2 = 0f;
			}
			if (this.Bullet_3 < 0f)
			{
				this.Bullet_3 = 0f;
			}
			if (this.Bullet_4 < 0f)
			{
				this.Bullet_4 = 0f;
			}
			if (this.Bullet_5 < 0f)
			{
				this.Bullet_5 = 0f;
			}
			if (this.Bullet_6 < 0f)
			{
				this.Bullet_6 = 0f;
			}
			if (this.Bullet_7 < 0f)
			{
				this.Bullet_7 = 0f;
			}
			if (this.Bullet_8 < 0f)
			{
				this.Bullet_8 = 0f;
			}
			if (this.Bullet_9 < 0f)
			{
				this.Bullet_9 = 0f;
			}
			if (this.Bullet_10 < 0f)
			{
				this.Bullet_10 = 0f;
			}
			if (this.Bullet_11 < 0f)
			{
				this.Bullet_11 = 0f;
			}
			if (this.Bullet_12 < 0f)
			{
				this.Bullet_12 = 0f;
			}
			if (this.Bullet_1 > 1f)
			{
				this.Bullet_1 = 1f;
			}
			if (this.Bullet_2 > 1f)
			{
				this.Bullet_2 = 1f;
			}
			if (this.Bullet_3 > 1f)
			{
				this.Bullet_3 = 1f;
			}
			if (this.Bullet_4 > 1f)
			{
				this.Bullet_4 = 1f;
			}
			if (this.Bullet_5 > 1f)
			{
				this.Bullet_5 = 1f;
			}
			if (this.Bullet_6 > 1f)
			{
				this.Bullet_6 = 1f;
			}
			if (this.Bullet_7 > 1f)
			{
				this.Bullet_7 = 1f;
			}
			if (this.Bullet_8 > 1f)
			{
				this.Bullet_8 = 1f;
			}
			if (this.Bullet_9 > 1f)
			{
				this.Bullet_9 = 1f;
			}
			if (this.Bullet_10 > 1f)
			{
				this.Bullet_10 = 1f;
			}
			if (this.Bullet_11 > 1f)
			{
				this.Bullet_11 = 1f;
			}
			if (this.Bullet_12 > 1f)
			{
				this.Bullet_12 = 1f;
			}
			this.material.SetFloat("_Bullet_1", this.Bullet_1);
			this.material.SetFloat("_Bullet_2", this.Bullet_2);
			this.material.SetFloat("_Bullet_3", this.Bullet_3);
			this.material.SetFloat("_Bullet_4", this.Bullet_4);
			this.material.SetFloat("_Bullet_5", this.Bullet_5);
			this.material.SetFloat("_Bullet_6", this.Bullet_6);
			this.material.SetFloat("_Bullet_7", this.Bullet_7);
			this.material.SetFloat("_Bullet_8", this.Bullet_8);
			this.material.SetFloat("_Bullet_9", this.Bullet_9);
			this.material.SetFloat("_Bullet_10", this.Bullet_10);
			this.material.SetFloat("_Bullet_11", this.Bullet_11);
			this.material.SetFloat("_Bullet_12", this.Bullet_12);
			this.material.SetTexture("_MainTex2", this.Texture2);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
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

	[Range(0f, 1f)]
	public float Bullet_1;

	[Range(0f, 1f)]
	public float Bullet_2;

	[Range(0f, 1f)]
	public float Bullet_3;

	[Range(0f, 1f)]
	public float Bullet_4 = 1f;

	[Range(0f, 1f)]
	public float Bullet_5;

	[Range(0f, 1f)]
	public float Bullet_6;

	[Range(0f, 1f)]
	public float Bullet_7;

	[Range(0f, 1f)]
	public float Bullet_8;

	[Range(0f, 1f)]
	public float Bullet_9;

	[Range(0f, 1f)]
	public float Bullet_10;

	[Range(0f, 1f)]
	public float Bullet_11;

	[Range(0f, 1f)]
	public float Bullet_12;

	private Material SCMaterial;

	private Texture2D Texture2;
}
