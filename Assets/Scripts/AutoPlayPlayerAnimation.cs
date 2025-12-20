using System;
using UnityEngine;

public class AutoPlayPlayerAnimation : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			this.animation.state.SetAnimation(0, this.animeName, false);
		}
	}

	public SkeletonAnimation animation;

	public string animeName;
}
