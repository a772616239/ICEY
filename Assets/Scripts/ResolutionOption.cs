using System;
using System.Collections.Generic;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class ResolutionOption : Singleton<ResolutionOption>
{
	public ResolutionOption()
	{
		this.InitResolutions();
		for (int i = 0; i < this._resolutions.Length; i++)
		{
			Resolution resolution = this._resolutions[i];
			if (resolution.width == this.Resolution.width && resolution.height == this.Resolution.height)
			{
				this._index = i;
			}
		}
		this._index = QualitySettings.GetQualityLevel();
	}

	private void InitResolutions()
	{
		this._originResolution = new Resolution
		{
			width = Screen.width,
			height = Screen.height
		};
		float num = (float)Screen.width / (float)Screen.height;
		this._resolutions = new Resolution[]
		{
			new Resolution
			{
				width = (int)(540f * num),
				height = 540
			},
			new Resolution
			{
				width = (int)(720f * num),
				height = 720
			},
			this._originResolution
		};
		this._resolutionStrings.Add("low");
		this._resolutionStrings.Add("normal");
		this._resolutionStrings.Add("high");
	}

	private Resolution Resolution
	{
		get
		{
			return new Resolution
			{
				width = Screen.width,
				height = Screen.height
			};
		}
		set
		{
			Screen.SetResolution(value.width, value.height, Screen.fullScreen);
		}
	}

	public string ResolutionString
	{
		get
		{
			return ScriptLocalization.Get("mobile/ui/quality_level/" + this._resolutionStrings[this._index]);
		}
	}

	public void SetResolutionByOffset(int step)
	{
		int num = this._index + step;
		if (num >= 0 && num <= this._resolutions.Length - 1)
		{
			this._index = num;
			QualitySettings.SetQualityLevel(num);
			this.SetResolutionByQualitylevel();
			EventManager.PostEvent<ResolutionOption>("QualityChange", this);
		}
	}

	public void SetResolutionByQualitylevel()
	{
        Debug.Log("SetResolutionByQualitylevel" + QualitySettings.GetQualityLevel());
		this.Resolution = this._resolutions[QualitySettings.GetQualityLevel()];
	}

	private Resolution[] _resolutions;

	private readonly List<string> _resolutionStrings = new List<string>();

	private int _index;

	private Resolution _originResolution;
}
