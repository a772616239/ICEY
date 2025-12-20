using System;
using I2.Loc;
using UnityEngine;

public class UIStartGraphicOptionController : MonoBehaviour
{
	private void Start()
	{
		this._fps = new UIStartGraphicOptionController.FpsOptions();
		this.UpdateResolutionLabel();
		this.UpdateFullScreenLabel(Screen.fullScreen);
		this.UpdateVSyncLabel();
		this.UpdateFpsLabel();
		this.UpdateFpsWarningLabel();
	}

	private void OnEnable()
	{
		this.UpdateResolutionLabel();
	}

	private void OnDisable()
	{
		R.Settings.Save();
	}

	public void OnResolutionNext()
	{
		this.OnResolutionChange(1);
	}

	public void OnResolutionPrevious()
	{
		this.OnResolutionChange(-1);
	}

	public void OnFpsNext()
	{
		this.OnFpsChange(1);
	}

	public void OnFpsPrevious()
	{
		this.OnFpsChange(-1);
	}

	private void OnFpsChange(int step)
	{
		this._fps.SetFpsByStep(step);
		this.UpdateFpsLabel();
		this.UpdateFpsWarningLabel();
	}

	private void OnResolutionChange(int step)
	{
		Singleton<ResolutionOption>.Instance.SetResolutionByOffset(step);
		this.UpdateResolutionLabel();
		this.UpdateFpsWarningLabel();
	}

	private void OnFullScreenClick()
	{
		this.UpdateFullScreenLabel(!Screen.fullScreen);
		Screen.fullScreen = !Screen.fullScreen;
	}

	private void OnVSyncChange(int step)
	{
		QualitySettings.vSyncCount = (QualitySettings.vSyncCount + step + 3) % 3;
		this.UpdateVSyncLabel();
		R.Settings.VSync = QualitySettings.vSyncCount;
		R.Settings.Save();
	}

	private void UpdateResolutionLabel()
	{
		this._resolutionLabel.text = Singleton<ResolutionOption>.Instance.ResolutionString;
	}

	private void UpdateFpsLabel()
	{
		this._fpsLabel.text = Application.targetFrameRate.ToString();
	}

	private void UpdateFpsWarningLabel()
	{
		bool active = Application.targetFrameRate == this._fps.MaxFps && QualitySettings.GetQualityLevel() == 2;
		this._fpsWarningGameObject.SetActive(active);
	}

	private void UpdateFullScreenLabel(bool isFullScreen)
	{
		this._fullScreenLocalize.Term = ((!isFullScreen) ? "ui/start/off" : "ui/start/on");
	}

	private void UpdateVSyncLabel()
	{
		switch (QualitySettings.vSyncCount)
		{
		case 0:
			this._vSyncLocalize.Term = "ui/start/off";
			break;
		case 1:
			this._vSyncLocalize.Term = "ui/start/on";
			break;
		case 2:
			this._vSyncLocalize.Term = "ui/start/half";
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	[SerializeField]
	private GameObject _fullScreenButton;

	[SerializeField]
	private GameObject _fpsWarningGameObject;

	[SerializeField]
	private Localize _fullScreenLocalize;

	[SerializeField]
	private GameObject _resolutionButton;

	[SerializeField]
	private UILabel _resolutionLabel;

	[SerializeField]
	private GameObject _vSyncButton;

	[SerializeField]
	private Localize _vSyncLocalize;

	[SerializeField]
	private UILabel _fpsLabel;

	private UIStartGraphicOptionController.FpsOptions _fps;

	private class FpsOptions
	{
		public FpsOptions()
		{
			int targetFrameRate = Application.targetFrameRate;
			if (targetFrameRate != 30)
			{
				if (targetFrameRate == 60)
				{
					this._fpsIndex = 0;
				}
			}
			else
			{
				this._fpsIndex = 1;
			}
		}

		public void SetFpsByStep(int step)
		{
			this._fpsIndex = Mathf.Clamp(this._fpsIndex + step, 0, this._fpsArray.Length - 1);
			Application.targetFrameRate = this._fpsArray[this._fpsIndex];
			R.Settings.FPS = this._fpsArray[this._fpsIndex];
		}

		private readonly int[] _fpsArray = new int[]
		{
			30,
			60
		};

		private int _fpsIndex;

		public readonly int MaxFps = 60;
	}
}
