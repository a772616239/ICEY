using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UISplashController : MonoBehaviour
{
	private void Start()
	{
		Log.Info("Loading the start scene");
		this._asyncOperation = LevelManager.LoadScene("ui_start");
		this._asyncOperation.allowSceneActivation = false;
		base.StartCoroutine(this.Sequence0());
	}

	private void Update()
	{
		if (this._asyncOperation != null && this._asyncOperation.isDone)
		{
			Log.Info("Loaded the start scene");
		}
	}

	private IEnumerator Sequence0()
	{
		if (UILanguage.IsSimplifiedChinese)
		{
			yield return DOTween.To(delegate(float a)
			{
				this._widgets[0].alpha = a;
			}, 0f, 1f, this._fade).WaitForCompletion();
			yield return new WaitForSeconds(5f);
			yield return DOTween.To(delegate(float a)
			{
				this._widgets[0].alpha = a;
			}, 1f, 0f, this._fade).WaitForCompletion();
			yield return new WaitForSeconds(this._fade);
		}
		for (int i = 1; i < this._widgets.Length; i++)
		{
			int iCopy = i;
			yield return DOTween.To(delegate(float a)
			{
				this._widgets[iCopy].alpha = a;
			}, 0f, 1f, this._fade).WaitForCompletion();
			yield return new WaitForSeconds((i == 0) ? 5f : this._duration);
			yield return DOTween.To(delegate(float a)
			{
				this._widgets[iCopy].alpha = a;
			}, 1f, 0f, this._fade).WaitForCompletion();
			if (iCopy != this._widgets.Length - 1)
			{
				yield return new WaitForSeconds(this._fade);
			}
		}
		
		this._asyncOperation.allowSceneActivation = true;
		
		yield break;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause && this._asyncOperation != null)
		{
			this._asyncOperation.allowSceneActivation = true;
		}
	}

	[SerializeField]
	private UIWidget[] _widgets;

	[SerializeField]
	private float _fade;

	[SerializeField]
	private float _duration;

	private AsyncOperation _asyncOperation;
}
