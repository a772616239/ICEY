using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class UITutorialController : MonoBehaviour
{
	public YieldInstruction PlayVideo(string path, string name)
	{
		return base.StartCoroutine(this.PlayVideoCoroutine(path, name));
	}

	public void DisposeVideo()
	{
		this._videoRenderer.gameObject.SetActive(false);
		this._playVideo.Dispose();
	}

	private IEnumerator PlayVideoCoroutine(string path, string name)
	{
		this._videoRenderer.gameObject.SetActive(true);
		this._playVideo = global::PlayVideo.CreateVideo(this._videoRenderer);
		yield return this._playVideo.Play(path, name, false);
		this.DisposeVideo();
		yield break;
	}

	public YieldInstruction Show(string str)
	{
		if (!this._generalTutorialWidgit.gameObject.activeInHierarchy)
		{
			this._generalTutorialWidgit.alpha = 0f;
			this._generalTutorialWidgit.gameObject.SetActive(true);
			this._generalLabel.text = str;
			return this.FadeIn(this._generalTutorialWidgit, 1f, false);
		}
		return this.FadeOut(this._generalTutorialWidgit, 1f, false);
	}

	public YieldInstruction Show(int id)
	{
		if (!this._tutorialWidgets[id].gameObject.activeInHierarchy)
		{
			this._tutorialWidgets[id].alpha = 0f;
			this._tutorialWidgets[id].gameObject.SetActive(true);
			return this.FadeIn(this._tutorialWidgets[id], 1f, false);
		}
		return this.FadeOut(this._tutorialWidgets[id], 1f, false);
	}

	public YieldInstruction Hide(int? id = null)
	{
		return base.StartCoroutine(this.HideCoroutine(id));
	}

	private IEnumerator HideCoroutine(int? id)
	{
		UIWidget widget = (id != null) ? this._tutorialWidgets[id.Value] : this._generalTutorialWidgit;
		if (widget.gameObject.activeInHierarchy && widget.alpha > 0f)
		{
			yield return this.FadeOut(widget, 1f, false);
			widget.gameObject.SetActive(false);
		}
		yield break;
	}

	private YieldInstruction FadeIn(UIWidget target, float during = 1f, bool ignoreTimeScale = false)
	{
		return this.FadeTo(target, 1f, during, ignoreTimeScale);
	}

	private YieldInstruction FadeOut(UIWidget target, float during = 1f, bool ignoreTimeScale = false)
	{
		return this.FadeTo(target, 0f, during, ignoreTimeScale);
	}

	private YieldInstruction FadeTo(UIWidget target, float endValue, float during, bool ignoreTimeScale)
	{
		return DOTween.To(() => target.alpha, delegate(float alpha)
		{
			target.alpha = alpha;
		}, endValue, during).SetUpdate(ignoreTimeScale).WaitForCompletion();
	}

	[SerializeField]
	private UIWidget _generalTutorialWidgit;

	[SerializeField]
	private UILabel _generalLabel;

	[SerializeField]
	private UIWidget[] _tutorialWidgets;

	[SerializeField]
	private Renderer _videoRenderer;

	private PlayVideo _playVideo;
}
