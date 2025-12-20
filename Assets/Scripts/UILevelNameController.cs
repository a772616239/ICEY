using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UILevelNameController : MonoBehaviour
{
	public UILabel Label
	{
		get
		{
			return this._label;
		}
	}

	public Coroutine Show(string levelName, int fontSize, float duration, bool withBackground = true)
	{
		return base.StartCoroutine(this.ShowCoroutine(levelName, fontSize, duration, withBackground));
	}

	private IEnumerator ShowCoroutine(string levelName, int fontSize, float duration, bool withBackground = false)
	{
		this._background.SetActive(withBackground);
		this._label.alpha = 0f;
		this._label.text = levelName;
		this._label.fontSize = fontSize;
		this._label.gameObject.SetActive(true);
		yield return DOTween.To(() => this._label.alpha, delegate(float a)
		{
			this._label.alpha = a;
		}, 1f, duration).WaitForCompletion();
		yield return new WaitForSeconds(1f);
		yield return DOTween.To(() => this._label.alpha, delegate(float a)
		{
			this._label.alpha = a;
		}, 0f, duration).WaitForCompletion();
		this._label.gameObject.SetActive(false);
		yield break;
	}

	[SerializeField]
	private UILabel _label;

	[SerializeField]
	private GameObject _background;
}
