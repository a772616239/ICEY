using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Distortion/BigFace")]
public class CameraFilterPack_Distortion_BigFace : MonoBehaviour
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
		CameraFilterPack_Distortion_BigFace.ChangeSize = this._Size;
		CameraFilterPack_Distortion_BigFace.ChangeDistortion = this.Distortion;
		this.SCShader = Shader.Find("CameraFilterPack/Distortion_BigFace");
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
			this.material.SetFloat("_Distortion", this.Distortion);
			this.material.SetFloat("_Size", this._Size);
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
		CameraFilterPack_Distortion_BigFace.ChangeSize = this._Size;
		CameraFilterPack_Distortion_BigFace.ChangeDistortion = this.Distortion;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this._Size = CameraFilterPack_Distortion_BigFace.ChangeSize;
			this.Distortion = CameraFilterPack_Distortion_BigFace.ChangeDistortion;
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

	private float TimeX = 6.5f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public float _Size = 5f;

	[Range(2f, 10f)]
	public float Distortion = 2.5f;

	public static float ChangeSize;

	public static float ChangeDistortion;
}
