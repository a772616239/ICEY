using System;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

public class UIFlashItem : MonoBehaviour
{
	public void Appear()
	{
		this._bg.alpha = 0f;
		this._bg.gameObject.SetActive(true);
		this._effect.alpha = 0f;
		this._effect.transform.localScale = Vector3.one;
		this._effect.gameObject.SetActive(true);
		Sequence s = DOTween.Sequence();
		s.Append(this._bg.DOFade(0.7058824f, 0.5f));
		s.Append(this._bg.DOFade(1f, 0.0833333358f));
		this._effect.transform.DOScale(new Vector3(2f, 2.5f, 1f), 0.333333343f).SetEase(Ease.OutSine).OnPlay(delegate
		{
			this._effect.alpha = 1f;
		}).SetDelay(0.5f);
		this._effect.DOFade(0f, 0.333333343f).OnComplete(delegate
		{
			this._effect.gameObject.SetActive(false);
		}).SetDelay(0.5f);
	}

	public void Disapper()
	{
		this._bg.gameObject.SetActive(false);
	}

	[SerializeField]
	private UISprite _effect;

	[SerializeField]
	private UISprite _bg;
}
