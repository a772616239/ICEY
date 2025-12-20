using System;
using System.Collections;
using UnityEngine;

public class FadeInFadeOutAnim : MonoBehaviour
{
	private void Start()
	{
		if (!this.uIObject)
		{
			this.uIObject = base.GetComponent<UIRect>();
		}
		this.uIObject.alpha = 0f;
		this.animStatus = FadeInFadeOutAnim.AnimsEnum.Start;
	}

	private void Update()
	{
		switch (this.animStatus)
		{
		case FadeInFadeOutAnim.AnimsEnum.Start:
			this.uIObject.alpha = 0f;
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.Idle;
			base.StartCoroutine(this.ITimeLine());
			break;
		case FadeInFadeOutAnim.AnimsEnum.FadeIn:
			this.FadeInAnim();
			break;
		case FadeInFadeOutAnim.AnimsEnum.FadeOut:
			this.FadeOutAnim();
			break;
		case FadeInFadeOutAnim.AnimsEnum.Show:
			this.ShowAnim();
			break;
		}
	}

	private IEnumerator ITimeLine()
	{
		if (this.fadeInTime > 0f)
		{
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.FadeIn;
			yield return new WaitForSeconds(this.fadeInTime);
		}
		else
		{
			this.uIObject.alpha = 1f;
		}
		if (this.showTime > 0f)
		{
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.Show;
			yield return new WaitForSeconds(this.showTime);
		}
		if (this.fadeOutTime > 0f)
		{
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.FadeOut;
			yield return new WaitForSeconds(this.fadeOutTime);
		}
		yield break;
	}

	private void FadeInAnim()
	{
		if (this.uIObject.alpha < 1f)
		{
			this.uIObject.alpha += Time.deltaTime / this.fadeInTime;
		}
		else
		{
			this.uIObject.alpha = 1f;
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.Idle;
		}
	}

	private void ShowAnim()
	{
	}

	private void FadeOutAnim()
	{
		if (this.uIObject.alpha > 0f)
		{
			this.uIObject.alpha -= 1f * Time.deltaTime / this.fadeOutTime;
		}
		else
		{
			this.uIObject.alpha = 0f;
			this.animStatus = FadeInFadeOutAnim.AnimsEnum.Idle;
		}
	}

	public float fadeInTime;

	public float showTime;

	public float fadeOutTime;

	public UIRect uIObject;

	public FadeInFadeOutAnim.AnimsEnum animStatus;

	public enum AnimsEnum
	{
		Idle,
		Start,
		FadeIn,
		FadeOut,
		Show
	}
}
