using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class UIVolumeController : MonoBehaviour
{
	public YieldInstruction Show(float volume)
	{
		this._widget.gameObject.SetActive(true);
		this._widget.alpha = 0f;
		if ((double)Mathf.Abs(volume) < 0.0001)
		{
			this._volumeOffSprite.gameObject.SetActive(true);
			this._volumeUpSprite.gameObject.SetActive(false);
			this._volumeBar.gameObject.SetActive(false);
		}
		else
		{
			this._volumeOffSprite.gameObject.SetActive(false);
			this._volumeUpSprite.gameObject.SetActive(true);
			this._volumeBar.gameObject.SetActive(true);
		}
		this._volumeBar.value = volume;
		return DOTween.To(() => this._widget.alpha, delegate(float a)
		{
			this._widget.alpha = a;
		}, 1f, 1f).WaitForCompletion();
	}

	public YieldInstruction Hide()
	{
		return DOTween.To(() => this._widget.alpha, delegate(float a)
		{
			this._widget.alpha = a;
		}, 0f, 1f).WaitForCompletion();
	}

	public YieldInstruction BarAnim(float endValue, float duration)
	{
		if ((double)Mathf.Abs(endValue) > 0.0001 && this._volumeOffSprite.gameObject.activeInHierarchy)
		{
			this._volumeOffSprite.gameObject.SetActive(false);
			this._volumeUpSprite.gameObject.SetActive(true);
			this._volumeBar.gameObject.SetActive(true);
		}
		return DOTween.To(() => this._volumeBar.value, delegate(float v)
		{
			this._volumeBar.value = v;
		}, endValue, duration).OnComplete(delegate
		{
			if ((double)Mathf.Abs(endValue) < 0.0001)
			{
				this._volumeOffSprite.gameObject.SetActive(true);
				this._volumeUpSprite.gameObject.SetActive(false);
				this._volumeBar.gameObject.SetActive(false);
			}
		}).WaitForCompletion();
	}

	[SerializeField]
	private UIWidget _widget;

	[SerializeField]
	private UISprite _volumeUpSprite;

	[SerializeField]
	private UISprite _volumeOffSprite;

	[SerializeField]
	private UIProgressBar _volumeBar;
}
