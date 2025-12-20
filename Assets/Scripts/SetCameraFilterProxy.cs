using System;
using UnityEngine;

[ExecuteInEditMode]
public class SetCameraFilterProxy : BaseBehaviour
{
	private void Start()
	{
		if (Application.isPlaying && this._triggerMode == SetCameraFilterProxy.TriggerMode.EnterScene)
		{
			this.AddFilter();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this._hasTriggered || !collision.CompareTag("Player") || this._triggerMode != SetCameraFilterProxy.TriggerMode.EnterCollider)
		{
			return;
		}
		this._hasTriggered = true;
		this.AddFilter();
		base.Invoke("RemoveFilter", this._duration);
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.RemoveFilter();
		}
	}

	private void AddFilter()
	{
		if (R.Camera.GameObject != null)
		{
			R.Camera.GameObject.AddComponent(CameraFilterUtils.FilterTypes[(int)this._type]);
		}
	}

	private void RemoveFilter()
	{
		if (R.Camera.GameObject != null)
		{
			UnityEngine.Object.Destroy(R.Camera.GameObject.GetComponent(CameraFilterUtils.FilterTypes[(int)this._type]));
		}
	}

	[SerializeField]
	private SetCameraFilterProxy.CameraFilterPackType _type;

	[SerializeField]
	private string _typeString;

	[SerializeField]
	private SetCameraFilterProxy.TriggerMode _triggerMode;

	[SerializeField]
	private float _duration;

	private bool _hasTriggered;

	private enum CameraFilterPackType
	{
		AAA_Blood,
		AAA_SuperComputer,
		AAA_SuperHexagon,
		AAA_WaterDrop,
		Alien_Vision,
		Antialiasing_FXAA,
		Atmosphere_Snow_8bits,
		Blend2Camera_Blend,
		Blend2Camera_BlueScreen,
		Blend2Camera_Color,
		Blend2Camera_ColorBurn,
		Blend2Camera_ColorDodge,
		Blend2Camera_Darken,
		Blend2Camera_DarkerColor,
		Blend2Camera_Difference,
		Blend2Camera_Divide,
		Blend2Camera_Exclusion,
		Blend2Camera_GreenScreen,
		Blend2Camera_HardLight,
		Blend2Camera_HardMix,
		Blend2Camera_Hue,
		Blend2Camera_Lighten,
		Blend2Camera_LighterColor,
		Blend2Camera_LinearBurn,
		Blend2Camera_LinearDodge,
		Blend2Camera_LinearLight,
		Blend2Camera_Luminosity,
		Blend2Camera_Multiply,
		Blend2Camera_Overlay,
		Blend2Camera_PhotoshopFilters,
		Blend2Camera_PinLight,
		Blend2Camera_Saturation,
		Blend2Camera_Screen,
		Blend2Camera_SoftLight,
		Blend2Camera_SplitScreen,
		Blend2Camera_Subtract,
		Blend2Camera_VividLight,
		Blizzard,
		Blur_Bloom,
		Blur_BlurHole,
		Blur_Blurry,
		Blur_Dithering2x2,
		Blur_DitherOffset,
		Blur_Focus,
		Blur_GaussianBlur,
		Blur_Movie,
		Blur_Noise,
		Blur_Radial,
		Blur_Radial_Fast,
		Blur_Regular,
		Blur_Steam,
		Colors_Adjust_ColorRGB,
		Colors_Adjust_FullColors,
		Colors_Adjust_PreFilters,
		Colors_Brightness,
		Colors_DarkColor,
		Colors_HSV,
		Colors_HUE_Rotate,
		Colors_NewPosterize,
		Colors_Threshold,
		Color_BrightContrastSaturation,
		Color_Chromatic_Aberration,
		Color_Contrast,
		Color_GrayScale,
		Color_Invert,
		Color_Noise,
		Color_RGB,
		Color_Sepia,
		Color_Switching,
		Color_YUV,
		Distortion_BigFace,
		Distortion_BlackHole,
		Distortion_Dissipation,
		Distortion_Dream,
		Distortion_Dream2,
		Distortion_FishEye,
		Distortion_Flag,
		Distortion_Flush,
		Distortion_Half_Sphere,
		Distortion_Heat,
		Distortion_Lens,
		Distortion_Noise,
		Distortion_ShockWave,
		Distortion_Water_Drop,
		Distortion_Wave_Horizontal,
		Drawing_CellShading,
		Drawing_CellShading2,
		Drawing_Comics,
		Drawing_Crosshatch,
		Drawing_Curve,
		Drawing_EnhancedComics,
		Drawing_Halftone,
		Drawing_Laplacian,
		Drawing_Lines,
		Drawing_Manga,
		Drawing_Manga2,
		Drawing_Manga3,
		Drawing_Manga4,
		Drawing_Manga5,
		Drawing_Manga_Color,
		Drawing_Manga_Flash,
		Drawing_Manga_FlashWhite,
		Drawing_NewCellShading,
		Drawing_Toon,
		Edge_BlackLine,
		Edge_Edge_filter,
		Edge_Golden,
		Edge_Neon,
		Edge_Sigmoid,
		Edge_Sobel,
		EXTRA_Rotation,
		EXTRA_SHOWFPS,
		EyesVision_1,
		EyesVision_2,
		Film_ColorPerfection,
		Film_Grain,
		FX_8bits,
		FX_8bits_gb,
		FX_Ascii,
		FX_DigitalMatrix,
		FX_DigitalMatrixDistortion,
		FX_Dot_Circle,
		FX_Drunk,
		FX_Drunk2,
		FX_EarthQuake,
		FX_Funk,
		FX_Glitch1,
		FX_Glitch2,
		FX_Glitch3,
		FX_Grid,
		FX_Hexagon,
		FX_Hexagon_Black,
		FX_Hypno,
		FX_InverChromiLum,
		FX_Mirror,
		FX_Plasma,
		FX_Psycho,
		FX_Scan,
		FX_Screens,
		FX_Spot,
		FX_superDot,
		FX_ZebraColor,
		Glitch_Mozaic,
		Gradients_Ansi,
		Gradients_Desert,
		Gradients_ElectricGradient,
		Gradients_FireGradient,
		Gradients_Hue,
		Gradients_NeonGradient,
		Gradients_Rainbow,
		Gradients_Stripe,
		Gradients_Tech,
		Gradients_Therma,
		Light_Rainbow,
		Light_Rainbow2,
		Light_Water,
		Light_Water2,
		NightVisionFX,
		NightVision_4,
		Oculus_NightVision1,
		Oculus_NightVision2,
		Oculus_NightVision3,
		Oculus_NightVision5,
		Oculus_ThermaVision,
		OldFilm_Cutting1,
		OldFilm_Cutting2,
		Pixelisation_Dot,
		Pixelisation_OilPaint,
		Pixelisation_OilPaintHQ,
		Pixel_Pixelisation,
		Real_VHS,
		Retro_Loading,
		Sharpen_Sharpen,
		Special_Bubble,
		TV_50,
		TV_80,
		TV_ARCADE,
		TV_ARCADE_2,
		TV_Artefact,
		TV_BrokenGlass,
		TV_BrokenGlass2,
		TV_Chromatical,
		TV_Chromatical2,
		TV_CompressionFX,
		TV_Distorted,
		TV_LED,
		TV_Noise,
		TV_Old,
		TV_Old_Movie,
		TV_Old_Movie_2,
		TV_PlanetMars,
		TV_Posterize,
		TV_Rgb,
		TV_Tiles,
		TV_Vcr,
		TV_VHS,
		TV_VHS_Rewind,
		TV_Video3D,
		TV_Videoflip,
		TV_Vintage,
		TV_WideScreenCircle,
		TV_WideScreenHorizontal,
		TV_WideScreenHV,
		TV_WideScreenVertical,
		VHS_Tracking,
		Vision_Aura,
		Vision_AuraDistortion,
		Vision_Blood,
		Vision_Crystal,
		Vision_Drost,
		Vision_Plasma,
		Vision_Psycho,
		Vision_Rainbow,
		Vision_Tunnel,
		Vision_Warp,
		Vision_Warp2
	}

	private enum TriggerMode
	{
		EnterScene,
		EnterCollider
	}
}
