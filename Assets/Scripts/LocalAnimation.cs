using System;
using UnityEngine;

public class LocalAnimation : BaseBehaviour
{
	private void Start()
	{
		this._renderer = base.GetComponent<Animation>();
		this._asset_cn = this._renderer.clip;
		if (!UILanguage.IsSimplifiedChinese && this._setChinese)
		{
			this._renderer.Stop();
			this._renderer.clip = this.asset;
			this._renderer.Play();
			this._setChinese = false;
		}
		else if (UILanguage.IsSimplifiedChinese && !this._setChinese)
		{
			this._renderer.Stop();
			this._renderer.clip = this._asset_cn;
			this._renderer.Play();
			this._setChinese = true;
		}
	}

	private void Update()
	{
		if (!UILanguage.IsSimplifiedChinese && this._setChinese)
		{
			this._renderer.Stop();
			this._renderer.clip = this.asset;
			this._renderer.Play();
			this._setChinese = false;
		}
		else if (UILanguage.IsSimplifiedChinese && !this._setChinese)
		{
			this._renderer.Stop();
			this._renderer.clip = this._asset_cn;
			this._renderer.Play();
			this._setChinese = true;
		}
	}

	public AnimationClip asset;

	private AnimationClip _asset_cn;

	private Animation _renderer;

	private bool _setChinese = true;
}
