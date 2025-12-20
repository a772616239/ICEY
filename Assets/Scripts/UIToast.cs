using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIToast : MonoBehaviour
{
	public UILabel Label
	{
		get
		{
			return this.content;
		}
	}

	public void Appear(string text, int fontSize, bool withBackground = true)
	{
		this._background.SetActive(withBackground);
		this.content.text = text;
		this.content.fontSize = fontSize;
		this.content.text = text;
		this.content.fontSize = fontSize;
		this.toastPanel.alpha = 1f;
		this.toastPanel.gameObject.SetActive(true);
	}

	public void Disappear()
	{
		this.toastPanel.gameObject.SetActive(false);
	}

	public Coroutine Show(string text, float duration = 2f, bool withBackground = true)
	{
		return this.Show(text, 48, duration, withBackground);
	}

	public Coroutine Show(string text, int fontSize, float duration, bool withBackground = true)
	{
		return base.StartCoroutine(this.ShowCoroutine(text, fontSize, duration, withBackground));
	}

	private IEnumerator ShowCoroutine(string text, int fontSize, float duration, bool withBackground)
	{
		this._background.SetActive(withBackground);
		this.content.text = text;
		this.content.fontSize = fontSize;
		this.content.text = text;
		this.content.fontSize = fontSize;
		this.toastPanel.gameObject.SetActive(true);
		yield return DOTween.To(() => this.toastPanel.alpha, delegate(float a)
		{
			this.toastPanel.alpha = a;
		}, 1f, 0.5f).WaitForCompletion();
		yield return new WaitForSeconds((duration < 1f) ? 0f : (duration - 1f));
		yield return DOTween.To(() => this.toastPanel.alpha, delegate(float a)
		{
			this.toastPanel.alpha = a;
		}, 0f, 0.5f).WaitForCompletion();
		this.toastPanel.gameObject.SetActive(false);
		yield break;
	}

	[SerializeField]
	private UIPanel toastPanel;

	[SerializeField]
	private UILabel content;

	[SerializeField]
	private GameObject _background;
}
