using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/Real VHS HQ")]
public class CameraFilterPack_Real_VHS : MonoBehaviour
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
		this.SCShader = Shader.Find("CameraFilterPack/Real_VHS");
		this.VHS = (Resources.Load("CameraFilterPack_VHS1") as Texture2D);
		this.VHS2 = (Resources.Load("CameraFilterPack_VHS2") as Texture2D);
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	public static Texture2D GetRTPixels(Texture2D t, RenderTexture rt, int sx, int sy)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = rt;
		t.ReadPixels(new Rect(0f, 0f, (float)t.width, (float)t.height), 0, 0);
		RenderTexture.active = active;
		return t;
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.material.SetTexture("VHS", this.VHS);
			this.material.SetTexture("VHS2", this.VHS2);
			this.material.SetFloat("TRACKING", this.TRACKING);
			int width = 382;
			int height = 576;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(sourceTexture, temporary, this.material);
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
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

	private Material SCMaterial;

	private Texture2D VHS;

	private Texture2D VHS2;

	[Range(0f, 1f)]
	public float TRACKING = 0.212f;

	public static float ChangeDistortion;

	private Rect rect = new Rect(0f, 0f, 200f, 200f);
}
