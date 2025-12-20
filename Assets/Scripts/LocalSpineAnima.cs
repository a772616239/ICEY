using System;
using UnityEngine;

public class LocalSpineAnima : BaseBehaviour
{
	private void Awake()
	{
		this._renderer = base.GetComponent<SkeletonAnimation>();
		this._asset_cn = this._renderer.skeletonDataAsset;
		if (!UILanguage.IsSimplifiedChinese)
		{
			this._renderer.skeletonDataAsset = this.asset;
			this._renderer.skeletonDataAsset.Reset();
			this._renderer.Reset();
			this._setChinese = false;
		}
	}

	private void Update()
	{
		if (!UILanguage.IsSimplifiedChinese && this._setChinese)
		{
			this._renderer.skeletonDataAsset = this.asset;
			this._renderer.skeletonDataAsset.Reset();
			this._renderer.Reset();
			this._setChinese = false;
		}
		else if (UILanguage.IsSimplifiedChinese && !this._setChinese)
		{
			this._renderer.skeletonDataAsset = this._asset_cn;
			this._renderer.skeletonDataAsset.Reset();
			this._renderer.Reset();
			this._setChinese = true;
		}
	}

	[SerializeField]
	private SkeletonDataAsset asset;

	private SkeletonDataAsset _asset_cn;

	private SkeletonAnimation _renderer;

	private bool _setChinese = true;
}
