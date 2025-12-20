using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/BlackHole")]
public class CameraFilterPack_Distortion_BlackHole : MonoBehaviour
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
		CameraFilterPack_Distortion_BlackHole.ChangePositionX = this.PositionX;
		CameraFilterPack_Distortion_BlackHole.ChangePositionY = this.PositionY;
		CameraFilterPack_Distortion_BlackHole.ChangeSize = this.Size;
		CameraFilterPack_Distortion_BlackHole.ChangeDistortion = this.Distortion;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_BlackHole");
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
			this.material.SetFloat("_PositionX", this.PositionX);
			this.material.SetFloat("_PositionY", this.PositionY);
			this.material.SetFloat("_Distortion", this.Size);
			this.material.SetFloat("_Distortion2", this.Distortion);
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
		CameraFilterPack_Distortion_BlackHole.ChangePositionX = this.PositionX;
		CameraFilterPack_Distortion_BlackHole.ChangePositionY = this.PositionY;
		CameraFilterPack_Distortion_BlackHole.ChangeSize = this.Size;
		CameraFilterPack_Distortion_BlackHole.ChangeDistortion = this.Distortion;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.PositionX = CameraFilterPack_Distortion_BlackHole.ChangePositionX;
			this.PositionY = CameraFilterPack_Distortion_BlackHole.ChangePositionY;
			this.Size = CameraFilterPack_Distortion_BlackHole.ChangeSize;
			this.Distortion = CameraFilterPack_Distortion_BlackHole.ChangeDistortion;
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

	[Range(-1f, 1f)]
	public float PositionX;

	[Range(-1f, 1f)]
	public float PositionY;

	[Range(0f, 20f)]
	public float Size = 1.5f;

	[Range(0f, 180f)]
	public float Distortion = 30f;

	public static float ChangePositionX;

	public static float ChangePositionY;

	public static float ChangeSize;

	public static float ChangeDistortion;
}
