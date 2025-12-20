using System;
using UnityEngine;

public class UIROController : SingletonMono<UIROController>
{
	public void Show()
	{
		this._panel.SetActive(true);
		this._isPlaying = true;
	}

	public void Hide()
	{
		this._panel.SetActive(false);
		this._isPlaying = false;
	}

	private void Update()
	{
		if (this._isPlaying && !this._animation.isPlaying)
		{
			this._panel.SetActive(false);
			this._isPlaying = false;
		}
	}

	[SerializeField]
	private GameObject _panel;

	[SerializeField]
	private Animation _animation;

	private bool _isPlaying;
}
