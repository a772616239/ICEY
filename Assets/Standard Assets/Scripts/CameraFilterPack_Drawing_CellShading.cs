using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/CellShading")]
public class CameraFilterPack_Drawing_CellShading : MonoBehaviour
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
		CameraFilterPack_Drawing_CellShading.ChangeEdgeSize = this.EdgeSize;
		CameraFilterPack_Drawing_CellShading.ChangeColorLevel = this.ColorLevel;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading");
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
			this.material.SetFloat("_EdgeSize", this.EdgeSize);
			this.material.SetFloat("_ColorLevel", this.ColorLevel);
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
		CameraFilterPack_Drawing_CellShading.ChangeEdgeSize = this.EdgeSize;
		CameraFilterPack_Drawing_CellShading.ChangeColorLevel = this.ColorLevel;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.EdgeSize = CameraFilterPack_Drawing_CellShading.ChangeEdgeSize;
			this.ColorLevel = CameraFilterPack_Drawing_CellShading.ChangeColorLevel;
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

	[Range(0f, 1f)]
	public float EdgeSize = 0.1f;

	[Range(0f, 10f)]
	public float ColorLevel = 4f;

	public static float ChangeEdgeSize;

	public static float ChangeColorLevel;
}
