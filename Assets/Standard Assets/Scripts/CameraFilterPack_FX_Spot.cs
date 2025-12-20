using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/Spot")]
public class CameraFilterPack_FX_Spot : MonoBehaviour
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
		CameraFilterPack_FX_Spot.Changecenter = this.center;
		CameraFilterPack_FX_Spot.ChangeRadius = this.Radius;
		this.SCShader = Shader.Find("CameraFilterPack/FX_Spot");
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
			this.material.SetFloat("_PositionX", this.center.x);
			this.material.SetFloat("_PositionY", this.center.y);
			this.material.SetFloat("_Radius", this.Radius);
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
		CameraFilterPack_FX_Spot.Changecenter = this.center;
		CameraFilterPack_FX_Spot.ChangeRadius = this.Radius;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.center = CameraFilterPack_FX_Spot.Changecenter;
			this.Radius = CameraFilterPack_FX_Spot.ChangeRadius;
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

	public Vector2 center = new Vector2(0.5f, 0.5f);

	public float Radius = 0.2f;

	public static Vector2 Changecenter;

	public static float ChangeRadius;
}
