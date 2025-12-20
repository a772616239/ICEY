using System;
using UnityEngine;

public class AnimatorFrozen : BaseBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
			SingletonMono<WorldTime>.Instance.FrozenEvent += new EventHandler<WorldTime.FrozenArgs>(this.ClipFrozen);
			SingletonMono<WorldTime>.Instance.ResumeEvent += new EventHandler<WorldTime.FrozenArgs>(this.ClipResume);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
			SingletonMono<WorldTime>.Instance.FrozenEvent -= new EventHandler<WorldTime.FrozenArgs>(this.ClipFrozen);
			SingletonMono<WorldTime>.Instance.ResumeEvent -= new EventHandler<WorldTime.FrozenArgs>(this.ClipResume);
		}
	}

	private void ClipFrozen(object obj, EventArgs e)
	{
		if (this.isPause)
		{
			return;
		}
		this.anim.speed = 0f;
		this.isPause = true;
	}

	private void ClipResume(object obj = null, EventArgs e = null)
	{
		if (!this.isPause)
		{
			return;
		}
		this.anim.speed = 1f;
		this.isPause = false;
	}

	[SerializeField]
	private Animator anim;

	[SerializeField]
	public bool isPause;
}
