using System;
using UnityEngine;

public class JumperPart : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.5f, LayerManager.GroundMask | LayerManager.ObstacleMask | LayerManager.OneWayGroundMask);
			return hit;
		}
	}

	private void Awake()
	{
		this.anim = base.GetComponent<SkeletonAnimation>();
	}

	public void Init(bool isGround, Vector2 speed)
	{
		if (isGround)
		{
			this.hitGround = true;
			this.anim.state.SetAnimation(0, "Die3", false);
			R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
		}
		else
		{
			base.GetComponent<Rigidbody2D>().velocity = speed;
			this.anim.state.SetAnimation(0, "DieAir1Fall", true);
		}
	}

	private void Update()
	{
		if (this.isOnGround && !this.hitGround)
		{
			this.hitGround = true;
			this.anim.state.SetAnimation(0, "DieAir1Die", false);
			R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
		}
	}

	private void OnDisable()
	{
		this.hitGround = false;
	}

	private SkeletonAnimation anim;

	private bool hitGround;
}
