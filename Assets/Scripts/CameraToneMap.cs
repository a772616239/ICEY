using System;
using System.Collections.Generic;
using Colorful;
using GameWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class CameraToneMap : BaseBehaviour
{
	private void Awake()
	{
		this.ActiveCameraFilter(QualitySettings.GetQualityLevel() > 1);
		this.InitMapList();
	}

	private void InitMapList()
	{
		Dictionary<CameraToneMap.ToneMapType, string[]> dictionary = new Dictionary<CameraToneMap.ToneMapType, string[]>
		{
			{
				CameraToneMap.ToneMapType.BrightCold,
				new string[]
				{
					"C1L1S4",
					"C1L1S4_t",
					"C1L1S11",
					"C1L2S9",
					"C1L3S3",
					"C1L3S8",
					"C1L3S11",
					"C2L1S3",
					"C2L1S9",
					"C2L2S4",
					"C2L2S7",
					"C2L2S8",
					"C2L2S10",
					"C3L1S7",
					"C3L1S9",
					"C4L1S6",
					"C4L1S9",
					"C4L1S10",
					"C4L1S11",
					"C4L1S13",
					"C4L1S15",
					"C4L2S1",
					"C4L2S3",
					"C4L2S6",
					"C4L2S8",
					"C4L2S14",
					"C4L2S15",
					"C4L2S22",
					"C4L2S23",
					"C4L2S25",
					"C4L2S27",
					"C4L2S30",
					"C4L2S32",
					"C4L2S34",
					"C4L2S35",
					"C5L1S4",
					"palace0",
					"palace1",
					"palace2",
					"palace3",
					"palace4",
					"palace5",
					"palace6",
					"palace7"
				}
			},
			{
				CameraToneMap.ToneMapType.DarkCold,
				new string[]
				{
					"C0L1S1_t",
					"C0L1S2_t",
					"C0L1S3_t",
					"C0L1S4_t",
					"C1L1S1",
					"C1L1S1_t",
					"C1L1S2",
					"C1L1S2_t",
					"C1L1S3",
					"C1L1S3_t",
					"C1L1S5",
					"C1L1S5_t",
					"C1L1S6",
					"C1L1S6_t",
					"C1L1S7",
					"C1L1S7_t",
					"C1L1S8",
					"C1L1S12",
					"C1L1S13",
					"C1L3S14",
					"C1L3S15",
					"C1L3S16",
					"C2L1S11",
					"C2L1S12",
					"C2L1S13",
					"C2L2S6",
					"C3L1S1",
					"C3L1S2",
					"C3L1S21",
					"C3L1S22",
					"C4L1S4",
					"C4L1S5",
					"C4L1S7",
					"C4L1S8",
					"C4L1S14",
					"C4L2S10",
					"C4L2S13",
					"C4L2S18",
					"C4L2S20",
					"C4L2S21",
					"C4L2S33",
					"C5L1S1",
					"C5L1S2",
					"C5L1S3",
					"C6L1S1"
				}
			},
			{
				CameraToneMap.ToneMapType.BrightWarm,
				new string[]
				{
					"C1L2S4",
					"C1L3S1",
					"C1L3S2",
					"C1L3S4",
					"C1L3S5",
					"C1L3S6",
					"C1L3S7",
					"C1L3S9",
					"C1L3S12",
					"C1L3S13",
					"C2L1S1",
					"C2L1S2",
					"C2L1S4",
					"C2L1S5",
					"C3L1S4",
					"C3L1S8",
					"C3L1S10",
					"C3L1S14",
					"C3L1S15",
					"C3L1S16",
					"C3L1S20",
					"C4L2S9",
					"C4L2S11",
					"C4L2S16",
					"C4L2S17",
					"C4L2S24",
					"C4L2S28",
					"C4L2S31"
				}
			},
			{
				CameraToneMap.ToneMapType.DarkWarm,
				new string[]
				{
					"C1L2S1",
					"C1L2S2",
					"C1L2S3",
					"C1L2S5",
					"C1L2S7",
					"C1L2S8",
					"C1L2S10",
					"C1L2S11",
					"C1L2S12",
					"C1L2S13",
					"C1L2S14",
					"C1L3S10",
					"C2L1S6",
					"C2L1S7",
					"C2L1S8",
					"C2L1S10",
					"C2L2S1",
					"C2L2S2",
					"C2L2S3",
					"C2L2S5",
					"C2L2S9",
					"C2L2S11",
					"C2L2S12",
					"C2L2S13",
					"C3L1S3",
					"C3L1S5",
					"C3L1S6",
					"C3L1S11",
					"C3L1S12",
					"C3L1S13",
					"C3L1S18",
					"C3L1S19",
					"C4L1S12",
					"C4L2S2",
					"C4L2S5",
					"C4L2S7",
					"C4L2S12",
					"C4L2S19",
					"C4L2S26",
					"C4L2S29"
				}
			}
		};
		foreach (KeyValuePair<CameraToneMap.ToneMapType, string[]> keyValuePair in dictionary)
		{
			for (int i = 0; i < keyValuePair.Value.Length; i++)
			{
				this._mapList.Add(keyValuePair.Value[i], keyValuePair.Key);
			}
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		EventManager.RegisterEvent<EventArgs>("QualityChange", new EventManager.FBEventHandler<EventArgs>(this.OnQualityChange), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		EventManager.UnregisterEvent<EventArgs>("QualityChange", new EventManager.FBEventHandler<EventArgs>(this.OnQualityChange), EventManager.ListenerQueue.Game);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (QualitySettings.GetQualityLevel() < 1)
		{
			return;
		}
		if (this._brightnessContrastGamma == null || this._photoFilter == null)
		{
			return;
		}
		this._brightnessContrastGamma.enabled = true;
		this._photoFilter.enabled = true;
		CameraToneMap.ToneMapType toneMapType = CameraToneMap.ToneMapType.Unknow;
		if (this._mapList.ContainsKey(scene.name))
		{
			toneMapType = this._mapList[scene.name];
		}
		switch (toneMapType)
		{
		case CameraToneMap.ToneMapType.BrightCold:
			this._brightnessContrastGamma.Contrast = 6f;
			this._photoFilter.Color = new Color(0f, 0.3647059f, 1f);
			this._photoFilter.Density = 0.097f;
			break;
		case CameraToneMap.ToneMapType.DarkCold:
			this._brightnessContrastGamma.Contrast = 8f;
			this._photoFilter.Color = new Color(0f, 0.3647059f, 1f);
			this._photoFilter.Density = 0.107f;
			break;
		case CameraToneMap.ToneMapType.BrightWarm:
			this._brightnessContrastGamma.Contrast = 5f;
			this._photoFilter.Color = new Color(0.980392158f, 0.5411765f, 0f);
			this._photoFilter.Density = 0.087f;
			break;
		case CameraToneMap.ToneMapType.DarkWarm:
			this._brightnessContrastGamma.Contrast = 6f;
			this._photoFilter.Color = new Color(0.980392158f, 0.5411765f, 0f);
			this._photoFilter.Density = 0.087f;
			break;
		default:
			this._brightnessContrastGamma.enabled = false;
			this._photoFilter.enabled = false;
			break;
		}
	}

	private bool OnQualityChange(string eventdefine, object sender, EventArgs msg)
	{
		this.ActiveCameraFilter(QualitySettings.GetQualityLevel() > 1);
		return true;
	}

	private void ActiveCameraFilter(bool active)
	{
		if (active)
		{
			if (this._brightnessContrastGamma == null)
			{
				this._brightnessContrastGamma = base.gameObject.AddComponent<BrightnessContrastGamma>();
				this._brightnessContrastGamma.Brightness = 0f;
				this._brightnessContrastGamma.Gamma = 1f;
				this._brightnessContrastGamma.enabled = false;
			}
			if (this._photoFilter == null)
			{
				this._photoFilter = base.gameObject.AddComponent<PhotoFilter>();
				this._photoFilter.enabled = false;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this._brightnessContrastGamma);
			UnityEngine.Object.Destroy(this._photoFilter);
		}
	}

	private BrightnessContrastGamma _brightnessContrastGamma;

	private PhotoFilter _photoFilter;

	private readonly Dictionary<string, CameraToneMap.ToneMapType> _mapList = new Dictionary<string, CameraToneMap.ToneMapType>();

	private enum ToneMapType
	{
		Unknow,
		BrightCold,
		DarkCold,
		BrightWarm,
		DarkWarm
	}
}
