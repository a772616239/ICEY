using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/Blend")]
public class CameraFilterPack_Blend2Camera_Blend : MonoBehaviour
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
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		CameraFilterPack_Blend2Camera_Blend.ChangeValue = this.BlendFX;
		this.SCShader = Shader.Find(this.ShaderName);
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
			this.material.SetTexture("_MainTex2", this.Camera2tex);
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
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
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		CameraFilterPack_Blend2Camera_Blend.ChangeValue = this.BlendFX;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.BlendFX = CameraFilterPack_Blend2Camera_Blend.ChangeValue;
		}
	}

	private void OnEnable()
	{
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
	}

	private void OnDisable()
	{
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2.targetTexture = null;
		}
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	private string ShaderName = "CameraFilterPack/Blend2Camera_Blend";

	public Shader SCShader;

	public Camera Camera2;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	public static float ChangeValue;

	private RenderTexture Camera2tex;
}
