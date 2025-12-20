using System;
using UnityEngine;

public class TimeController : BaseBehaviour, IPlatformPhysics
{
	private void Awake()
	{
		this.animControl = base.GetComponent<SpineAnimationController>();
		this.rigid = base.GetComponent<Rigidbody2D>();
		this.eAttr = base.GetComponent<EnemyAttribute>();
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.ApplicationIsQuitting)
		{
			return;
		}
		SingletonMono<WorldTime>.Instance.FrozenEvent += this.ClipFrozen;
		SingletonMono<WorldTime>.Instance.ResumeEvent += this.ClipResume;
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.ApplicationIsQuitting)
		{
			return;
		}
		SingletonMono<WorldTime>.Instance.FrozenEvent -= this.ClipFrozen;
		SingletonMono<WorldTime>.Instance.ResumeEvent -= this.ClipResume;
	}

	public Vector2 GetCurrentSpeed()
	{
		Vector2? vector = this.currentSpeed;
		return (vector == null) ? this.rigid.velocity : vector.Value;
	}

	private void FixedUpdate()
	{
		if (this.isPause)
		{
			return;
		}
		Vector2? vector = this.currentSpeed;
		if (vector != null)
		{
			this.rigid.velocity = this.currentSpeed.Value;
			this.currentSpeed = null;
		}
		float? num = this.currentGravity;
		if (num != null)
		{
			this.rigid.gravityScale = this.currentGravity.Value;
			this.currentGravity = null;
		}
	}

	public void SetSpeed(Vector2 speed)
	{
		if (this.isPause)
		{
			this.currentSpeed = new Vector2?(speed);
		}
		else
		{
			this.rigid.velocity = speed;
		}
	}

	public void SetGravity(float scale)
	{
		if (this.isPause)
		{
			this.currentGravity = new float?(scale);
		}
		else
		{
			this.rigid.gravityScale = scale;
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
		this.currentSpeed = new Vector2?(this.rigid.velocity);
		this.currentGravity = new float?(this.rigid.gravityScale);
		this.rigid.gravityScale = 0f;
		this.rigid.velocity = Vector2.zero;
		this.animControl.Pause();
	}

	private void ClipResume(object obj, WorldTime.FrozenArgs e)
	{
		if (!this.isPause)
		{
			return;
		}
		this.animControl.Resume();
		this.isPause = false;
	}

	public Vector2 velocity
	{
		get
		{
			return this.GetCurrentSpeed();
		}
		set
		{
			this.SetSpeed(value);
		}
	}

	public Vector2 position
	{
		get
		{
			return this.rigid.position;
		}
		set
		{
			this.rigid.position = value;
		}
	}

	public bool isOnGround
	{
		get
		{
			return this.eAttr.isOnGround;
		}
	}

	public float? currentGravity;

	public Vector2? currentSpeed;

	[SerializeField]
	public bool isPause;

	private EnemyAttribute eAttr;

	private Rigidbody2D rigid;

	private SpineAnimationController animControl;

	private Vector2? nextSpeed;
}
