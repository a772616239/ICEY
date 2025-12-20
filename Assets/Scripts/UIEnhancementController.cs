using System;
using System.Diagnostics;
using Colorful;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameWorld;
using UnityEngine;

public class UIEnhancementController : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnAppear;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnFinish;

	private void Awake()
	{
		base.enabled = false;
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnLevelUp), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (Core.Input.UI.Cancel.OnClick)
		{
			this.OnCloseClick();
		}
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnLevelUp), EventManager.ListenerQueue.Game);
	}

	public void OnCloseClick()
	{
		if (!this._isLocked)
		{
			this.CloseWithAnim();
		}
	}

	private bool OnLevelUp(string eventdefine, object sender, EnhanceArgs msg)
	{
		this._isLevelUpInThisTime = true;
		return true;
	}

	private void Open()
	{
		if (this.OnAppear != null)
		{
			this.OnAppear(this, EventArgs.Empty);
		}
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		R.Ui.HideUI(true);
		this._enhancenmentPanel.gameObject.SetActive(true);
		this._background.gameObject.SetActive(true);
		this._items.gameObject.SetActive(true);
		CameraFilterUtils.Create<CameraFilterPack_TV_80>(R.Ui.CameraGO).SCShader = Shader.Find("CameraFilterPack/FB_TV_80");
		AnalogTV analogTV = R.Ui.CameraGO.AddComponent<AnalogTV>();
		analogTV.Shader = Shader.Find("Hidden/Colorful/Analog TV");
		analogTV.NoiseIntensity = 1f;
		analogTV.ScanlinesIntensity = 0f;
		analogTV.ScanlinesCount = 696;
		analogTV.Distortion = 0.18f;
		analogTV.CubicDistortion = 0f;
		analogTV.Scale = 1.02f;
	}

	public YieldInstruction OpenWithAnim()
	{
		if (this._isLocked)
		{
			return null;
		}
		base.enabled = true;
		this._isLocked = true;
		this.Open();
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		analogTV.Scale = 0f;
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 1.02f, this._duration).OnComplete(delegate
		{
			this._isLocked = false;
			R.Camera.Camera.enabled = false;
		}).SetUpdate(true).WaitForCompletion();
	}

	private void Close()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		UnityEngine.Object.Destroy(R.Ui.CameraGO.GetComponent<AnalogTV>());
		R.Mode.ExitMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = true;
		R.Ui.ShowUI(true);
		this._enhancenmentPanel.gameObject.SetActive(false);
		this._background.gameObject.SetActive(false);
		this._items.gameObject.SetActive(false);
		if (this._isLevelUpInThisTime)
		{
			this._isLevelUpInThisTime = false;
			R.GameData.Save(true);
		}
		if (this.OnFinish != null)
		{
			this.OnFinish(this, null);
		}
	}

	public YieldInstruction CloseWithAnim()
	{
		if (this._isLocked)
		{
			return null;
		}
		InputSetting.Stop(false);
		base.enabled = false;
		this._isLocked = true;
		R.Camera.Camera.enabled = true;
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 0f, this._duration).SetUpdate(true).OnComplete(delegate
		{
			this.Close();
			InputSetting.Resume(false);
			this._isLocked = false;
		}).WaitForCompletion();
	}

	[SerializeField]
	private UISprite _background;

	[SerializeField]
	private UIPanel _enhancenmentPanel;

	[SerializeField]
	private GameObject _items;

	[SerializeField]
	private UIEnhancementVideoController _enhancementVideo;

	[SerializeField]
	private float _duration = 0.5f;

	private bool _isLocked;

	private float _lockedTime;

	private bool _isLevelUpInThisTime;
}
