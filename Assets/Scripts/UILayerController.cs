using System;
using Core;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public sealed class UILayerController : MonoBehaviour
{
	private void Awake()
	{
		this._widget = base.GetComponent<UIWidget>();
	}

	private void Update()
	{
		if (this._lastLayer != null && Core.Input.UI.Cancel.OnClick)
		{
			this.OnCancelClick();
		}
	}

	public void OnCancelClick()
	{
		this._lastLayer.Appear();
		this.KillTweenerIfPlaying();
		this.Disappear();
		UIKeyInput.LoadHoveredObject();
	}

	public void Appear()
	{
		base.gameObject.SetActive(true);
		this.KillTweenerIfPlaying();
		this._widget.alpha = 0f;
		this._tweener = this._widget.DOFade(1f, 0.2f);
	}

	public void Disappear()
	{
		this.KillTweenerIfPlaying();
		this._widget.alpha = 0f;
		base.gameObject.SetActive(false);
	}

	private void KillTweenerIfPlaying()
	{
		if (this._tweener != null && this._tweener.IsPlaying())
		{
			this._tweener.Kill(false);
		}
	}

	[SerializeField]
	private UILayerController _lastLayer;

	private UIWidget _widget;

	private Tweener _tweener;
}
