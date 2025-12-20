using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/VHS_Rewind")]
public class CameraFilterPack_TV_VHS_Rewind : MonoBehaviour
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
		CameraFilterPack_TV_VHS_Rewind.ChangeValue = this.Cryptage;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue2 = this.Parasite;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue3 = this.Parasite2;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue4 = this.WhiteParasite;
		this.SCShader = Shader.Find("CameraFilterPack/TV_VHS_Rewind");
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
			this.material.SetFloat("_Value", this.Cryptage);
			this.material.SetFloat("_Value2", this.Parasite);
			this.material.SetFloat("_Value3", this.Parasite2);
			this.material.SetFloat("_Value4", this.WhiteParasite);
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
		CameraFilterPack_TV_VHS_Rewind.ChangeValue = this.Cryptage;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue2 = this.Parasite;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue3 = this.Parasite2;
		CameraFilterPack_TV_VHS_Rewind.ChangeValue4 = this.WhiteParasite;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Cryptage = CameraFilterPack_TV_VHS_Rewind.ChangeValue;
			this.Parasite = CameraFilterPack_TV_VHS_Rewind.ChangeValue2;
			this.Parasite2 = CameraFilterPack_TV_VHS_Rewind.ChangeValue3;
			this.WhiteParasite = CameraFilterPack_TV_VHS_Rewind.ChangeValue4;
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
	public float Cryptage = 1f;

	[Range(-20f, 20f)]
	public float Parasite = 9f;

	[Range(-20f, 20f)]
	public float Parasite2 = 12f;

	[Range(0f, 1f)]
	private float WhiteParasite = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
