using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Edge/Edge_filter")]
public class CameraFilterPack_Edge_Edge_filter : MonoBehaviour
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
		CameraFilterPack_Edge_Edge_filter.ChangeRedAmplifier = this.RedAmplifier;
		CameraFilterPack_Edge_Edge_filter.ChangeGreenAmplifier = this.GreenAmplifier;
		CameraFilterPack_Edge_Edge_filter.ChangeBlueAmplifier = this.BlueAmplifier;
		this.SCShader = Shader.Find("CameraFilterPack/Edge_Edge_filter");
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
			this.material.SetFloat("_RedAmplifier", this.RedAmplifier);
			this.material.SetFloat("_GreenAmplifier", this.GreenAmplifier);
			this.material.SetFloat("_BlueAmplifier", this.BlueAmplifier);
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
		CameraFilterPack_Edge_Edge_filter.ChangeRedAmplifier = this.RedAmplifier;
		CameraFilterPack_Edge_Edge_filter.ChangeGreenAmplifier = this.GreenAmplifier;
		CameraFilterPack_Edge_Edge_filter.ChangeBlueAmplifier = this.BlueAmplifier;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.RedAmplifier = CameraFilterPack_Edge_Edge_filter.ChangeRedAmplifier;
			this.GreenAmplifier = CameraFilterPack_Edge_Edge_filter.ChangeGreenAmplifier;
			this.BlueAmplifier = CameraFilterPack_Edge_Edge_filter.ChangeBlueAmplifier;
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
	public float RedAmplifier;

	[Range(0f, 10f)]
	public float GreenAmplifier = 2f;

	[Range(0f, 10f)]
	public float BlueAmplifier;

	public static float ChangeRedAmplifier;

	public static float ChangeGreenAmplifier;

	public static float ChangeBlueAmplifier;
}
