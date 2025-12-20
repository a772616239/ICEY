using System;
using System.Collections;
using UnityEngine;

public class UINotifacationController : MonoBehaviour
{
	private void Start()
	{
		this._label = base.GetComponentInChildren<UILabel>();
		this._playAnimation = base.GetComponent<UIPlayAnimation>();
		this._animClipLength = this._playAnimation.target.clip.length;
	}

	public void FadeIn(string str)
	{
		R.Audio.PlayEffect(363, null);
		str = "[-]" + str;
		str = str.Replace("[-]", "[FFFFFF]");
		this._label.text = str;
		this._label.alpha = 0.8f;
		this._playAnimation.Play(true, false);
	}

	public Coroutine Show(string str, float delayTime)
	{
		R.Audio.PlayEffect(363, null);
		this.FadeIn(str);
		return this.FadeOut(this._animClipLength + delayTime, 1f);
	}

	public void SetText(string str)
	{
		this._label.text = str;
	}

	public Coroutine FadeOut(float delayTime = 0f, float duration = 1f)
	{
		return base.StartCoroutine(this.FadeOutCo(delayTime, duration));
	}

	private IEnumerator FadeOutCo(float delayTime, float duration)
	{
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(delayTime);
		this._playAnimation.Play(false, false);
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(this._animClipLength);
		yield break;
	}

	private UILabel _label;

	private UIPlayAnimation _playAnimation;

	private float _animClipLength;
}
