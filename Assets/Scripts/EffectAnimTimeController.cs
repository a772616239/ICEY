using System;
using UnityEngine;

public class EffectAnimTimeController : BaseBehaviour
{
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
			SingletonMono<WorldTime>.Instance.FrozenEvent += this.ClipFrozen;
			SingletonMono<WorldTime>.Instance.ResumeEvent += this.ClipResume;
		}
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
			SingletonMono<WorldTime>.Instance.FrozenEvent -= this.ClipFrozen;
			SingletonMono<WorldTime>.Instance.ResumeEvent -= this.ClipResume;
		}
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	public void SetSpeed(Vector2 speed)
	{
		if (this.isPause)
		{
			this.currentSpeed = speed;
		}
		else
		{
			base.GetComponent<Rigidbody2D>().velocity = speed;
		}
	}

	private void ClipFrozen(object obj, WorldTime.FrozenArgs e)
	{
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Player)
		{
			return;
		}
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Target && base.gameObject != e.Target)
		{
			return;
		}
		if (this.isPause)
		{
			return;
		}
		this.isPause = true;
		if (this.anim != null)
		{
			this.anim.speed = 0f;
		}
	}

	private void ClipResume(object obj, WorldTime.FrozenArgs e)
	{
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Player)
		{
			return;
		}
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Target && base.gameObject != e.Target)
		{
			return;
		}
		if (!this.isPause)
		{
			return;
		}
		this.currentSpeed = Vector2.zero;
		if (this.anim != null)
		{
			this.anim.speed = 1f;
		}
		this.isPause = false;
	}

	public void Pause()
	{
	}

	public void Resume()
	{
	}

	public Vector2 currentSpeed;

	[SerializeField]
	public bool isPause;

	public Animator anim;
}
