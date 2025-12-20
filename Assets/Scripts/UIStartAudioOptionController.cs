using System;
using UnityEngine;

public class UIStartAudioOptionController : MonoBehaviour
{
	private void Start()
	{
		this._effectButtonLabel.alpha = ((!R.Audio.IsEffectsMute) ? 1f : 0.5f);
		this._bgmButtonLabel.alpha = ((!R.Audio.IsBGMMute) ? 1f : 0.5f);
		this._effectProgressBar.value = R.Audio.EffectsVolume / 100f;
		this._effectProgressBar.alpha = ((!R.Audio.IsEffectsMute) ? 1f : 0.5f);
		this._bgmProgressBar.value = R.Audio.BGMVolume / 100f;
		this._bgmProgressBar.alpha = ((!R.Audio.IsBGMMute) ? 1f : 0.5f);
	}

	private void OnEnable()
	{
		EventDelegate.Add(this._effectProgressBar.onChange, new EventDelegate.Callback(this.OnEffectVolumeChanged));
		EventDelegate.Add(this._bgmProgressBar.onChange, new EventDelegate.Callback(this.OnBGMVolumeChanged));
	}

	private void OnDisable()
	{
		EventDelegate.Remove(this._effectProgressBar.onChange, new EventDelegate.Callback(this.OnEffectVolumeChanged));
		EventDelegate.Remove(this._bgmProgressBar.onChange, new EventDelegate.Callback(this.OnBGMVolumeChanged));
		R.Settings.Save();
	}

	private void OnEffectVolumeChanged()
	{
		R.Audio.EffectsVolume = this._effectProgressBar.value * 100f;
	}

	private void OnBGMVolumeChanged()
	{
		R.Audio.BGMVolume = this._bgmProgressBar.value * 100f;
	}

	public void OnEffectClick()
	{
		R.Audio.IsEffectsMute = !R.Audio.IsEffectsMute;
		this._effectButtonLabel.alpha = ((!R.Audio.IsEffectsMute) ? 1f : 0.5f);
		this._effectProgressBar.alpha = ((!R.Audio.IsEffectsMute) ? 1f : 0.5f);
	}

	public void OnBgmClick()
	{
		R.Audio.IsBGMMute = !R.Audio.IsBGMMute;
		this._bgmButtonLabel.alpha = ((!R.Audio.IsBGMMute) ? 1f : 0.5f);
		this._bgmProgressBar.alpha = ((!R.Audio.IsBGMMute) ? 1f : 0.5f);
	}

	[SerializeField]
	private GameObject _effectButton;

	[SerializeField]
	private UILabel _effectButtonLabel;

	[SerializeField]
	private GameObject _bgmButton;

	[SerializeField]
	private UILabel _bgmButtonLabel;

	[SerializeField]
	private UIProgressBar _effectProgressBar;

	[SerializeField]
	private UIProgressBar _bgmProgressBar;
}
