using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blend 2 Camera/PhotoshopFilters")]
public class CameraFilterPack_Blend2Camera_PhotoshopFilters : MonoBehaviour
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

	private void ChangeFilters()
	{
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Darken)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Darken";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Multiply)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Multiply";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorBurn)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_ColorBurn";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearBurn)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearBurn";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.DarkerColor)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_DarkerColor";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Lighten)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Lighten";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Screen)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Screen";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.ColorDodge)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_ColorDodge";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearDodge)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearDodge";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LighterColor)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LighterColor";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Overlay)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Overlay";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.SoftLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_SoftLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_HardLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.VividLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_VividLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.LinearLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_LinearLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.PinLight)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_PinLight";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.HardMix)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_HardMix";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Difference)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Difference";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Exclusion)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Exclusion";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Subtract)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Subtract";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Divide)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Divide";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Hue)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Hue";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Saturation)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Saturation";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Color)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Color";
		}
		if (this.filterchoice == CameraFilterPack_Blend2Camera_PhotoshopFilters.filters.Luminosity)
		{
			this.ShaderName = "CameraFilterPack/Blend2Camera_Luminosity";
		}
	}

	private void Start()
	{
		this.filterchoicememo = this.filterchoice;
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue = this.BlendFX;
		CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue2 = this.SwitchCameraToCamera2;
		this.ChangeFilters();
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
		if (this.filterchoice != this.filterchoicememo)
		{
			this.ChangeFilters();
			this.SCShader = Shader.Find(this.ShaderName);
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
		}
		this.filterchoicememo = this.filterchoice;
		if (this.Camera2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Camera2.targetTexture);
			this.Camera2tex = new RenderTexture(Screen.width, Screen.height, 24);
			this.Camera2.targetTexture = this.Camera2tex;
		}
		CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue = this.BlendFX;
		CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue2 = this.SwitchCameraToCamera2;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.BlendFX = CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue;
			this.SwitchCameraToCamera2 = CameraFilterPack_Blend2Camera_PhotoshopFilters.ChangeValue2;
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

	private string ShaderName = "CameraFilterPack/Blend2Camera_Darken";

	public Shader SCShader;

	public Camera Camera2;

	public CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoice;

	private CameraFilterPack_Blend2Camera_PhotoshopFilters.filters filterchoicememo;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float SwitchCameraToCamera2;

	[Range(0f, 1f)]
	public float BlendFX = 0.5f;

	public static float ChangeValue;

	public static float ChangeValue2;

	private RenderTexture Camera2tex;

	public enum filters
	{
		Darken,
		Multiply,
		ColorBurn,
		LinearBurn,
		DarkerColor,
		Lighten,
		Screen,
		ColorDodge,
		LinearDodge,
		LighterColor,
		Overlay,
		SoftLight,
		HardLight,
		VividLight,
		LinearLight,
		PinLight,
		HardMix,
		Difference,
		Exclusion,
		Subtract,
		Divide,
		Hue,
		Saturation,
		Color,
		Luminosity
	}
}
