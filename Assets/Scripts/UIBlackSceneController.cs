using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIBlackSceneController : MonoBehaviour
{
	public float Alpha
	{
		get
		{
			return this._panel.alpha;
		}
		set
		{
			this._panel.alpha = value;
		}
	}

	private void Awake()
	{
		this._panel = base.GetComponent<UIPanel>();
		this._panel.alpha = 0f;
	}

	public YieldInstruction FadeTransparent(float during = 0.3f, bool ignoreTimeScale = false)
	{
		return this.FadeTo(0f, during, ignoreTimeScale);
	}

	public YieldInstruction FadeBlack(float during = 0.3f, bool ignoreTimeScale = false)
	{
		return this.FadeTo(1f, during, ignoreTimeScale);
	}

	public YieldInstruction FadeTo(float endValue, float during, bool ignoreTimeScale = false)
	{
		return DOTween.To(() => this._panel.alpha, delegate(float alpha)
		{
			this._panel.alpha = alpha;
		}, endValue, during).SetUpdate(ignoreTimeScale).WaitForCompletion();
	}

	public void Kill()
	{
		this._panel.DOKill(false);
	}

	private UIPanel _panel;
}
