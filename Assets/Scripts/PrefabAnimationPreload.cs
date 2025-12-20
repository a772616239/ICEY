using System;
using UnityEngine;

public class PrefabAnimationPreload : BaseBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this._skeletonDataAssets.Length; i++)
		{
			this._skeletonDataAssets[i].GetSkeletonData(false);
			this._skeletonDataAssets[i].GetAnimationStateData();
		}
		Log.Info("Skeleton Data Assets is loaded");
		for (int j = 0; j < this._animations.Length; j++)
		{
			this._animations[j].GetClipCount();
		}
		Log.Info("Prefab's Animation is loaded");
	}

	[SerializeField]
	private Animation[] _animations;

	[SerializeField]
	private SkeletonDataAsset[] _skeletonDataAssets;
}
