using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Split Screen/SideBySide")]
public class CameraFilterPack_Blend2Camera_SplitScreen : MonoBehaviour
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
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture((int)this.ScreenSize.x, (int)this.ScreenSize.y, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue = this.BlendFX;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue2 = this.SwitchCameraToCamera2;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue3 = this.SplitX;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue6 = this.SplitY;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue4 = this.Smooth;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue5 = this.Rotation;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue7 = this.ForceYSwap;
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
			if (this.Camera2 != null)
			{
				this.material.SetTexture("_MainTex2", this.Camera2tex);
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.BlendFX);
			this.material.SetFloat("_Value2", this.SwitchCameraToCamera2);
			this.material.SetFloat("_Value3", this.SplitX);
			this.material.SetFloat("_Value6", this.SplitY);
			this.material.SetFloat("_Value4", this.Smooth);
			this.material.SetFloat("_Value5", this.Rotation);
			this.material.SetInt("_ForceYSwap", (!this.ForceYSwap) ? 1 : 0);
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue = this.BlendFX;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue2 = this.SwitchCameraToCamera2;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue3 = this.SplitX;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue6 = this.SplitY;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue7 = this.ForceYSwap;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue4 = this.Smooth;
		CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue5 = this.Rotation;
	}

	private void Update()
	{
		this.ScreenSize.x = (float)Screen.width;
		this.ScreenSize.y = (float)Screen.height;
		if (Application.isPlaying)
		{
			this.BlendFX = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue;
			this.SwitchCameraToCamera2 = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue2;
			this.SplitX = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue3;
			this.SplitY = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue6;
			this.Smooth = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue4;
			this.Rotation = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue5;
			this.ForceYSwap = CameraFilterPack_Blend2Camera_SplitScreen.ChangeValue7;
		}
	}

	private void OnEnable()
	{
		this.Start();
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

	private string ShaderName = "CameraFilterPack/Blend2Camera_SplitScreen";

	public Shader SCShader;

	public Camera Camera2;

	private float TimeX = 1f;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	[Range(0f, 1f)]
	public float BlendFX = 1f;

	[Range(-3f, 3f)]
	public float SplitX = 0.5f;

	[Range(-3f, 3f)]
	public float SplitY = 0.5f;

	[Range(0f, 2f)]
	public float Smooth = 0.1f;

	[Range(-3.14f, 3.14f)]
	public float Rotation = 3.14f;

	private bool ForceYSwap;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	public static float ChangeValue5;

	public static float ChangeValue6;

	public static bool ChangeValue7;

	private RenderTexture Camera2tex;

	private Vector2 ScreenSize;
}
