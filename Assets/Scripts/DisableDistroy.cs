using System;
using UnityEngine;

public class DisableDistroy : BaseBehaviour
{
	private void Start()
	{
	}

	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OmDestroy()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
		}
	}

	private void ClipFrozen(object obj, EventArgs e)
	{
		if (this.isPause)
		{
			return;
		}
		this.isPause = true;
		this.anim.speed = 0f;
	}

	private void ClipResume(object obj, EventArgs e)
	{
		if (!this.isPause)
		{
			return;
		}
		this.anim.speed = 1f;
		this.isPause = false;
	}

	public bool isPause;

	private Animator anim;
}
