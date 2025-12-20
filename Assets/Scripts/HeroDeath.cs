using System;
using DG.Tweening;
using Spine;
using UnityEngine;

public class HeroDeath : MonoBehaviour
{
	private void OnGhost2Man()
	{
		Skeleton skeleton = PlayerControllerDemo.Instance.GetComponent<SkeletonAnimator>().skeleton;
		DOTween.To(delegate(float x)
		{
			skeleton.a = x;
		}, skeleton.a, 1f, 0.1f);
		Vector2 v = PlayerControllerDemo.Instance.transform.localScale;
		PlayerControllerDemo.Instance.transform.localScale = Vector2.one * 0.01f;
		PlayerControllerDemo.Instance.transform.DOScale(v, 0.1f).OnComplete(delegate
		{
			PlayerControllerDemo.Instance.FixSide();
		});
	}
}
