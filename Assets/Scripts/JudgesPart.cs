using System;
using UnityEngine;

public class JudgesPart : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.5f, LayerManager.GroundMask | LayerManager.ObstacleMask | LayerManager.OneWayGroundMask);
			return hit;
		}
	}

	private void Start()
	{
		this.flyToFall = true;
		this.hitGround = false;
		this.spineAnim = base.GetComponent<SkeletonAnimation>();
		this.body = base.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (this.flyToFall && this.body.velocity.y <= 0f)
		{
			this.spineAnim.state.SetAnimation(0, "FlyToFall", false);
			this.flyToFall = false;
			this.hitGround = true;
		}
		if (this.hitGround && this.isOnGround)
		{
			this.spineAnim.state.SetAnimation(0, "FallHitGround", false);
			this.hitGround = false;
		}
	}

	private Rigidbody2D body;

	private SkeletonAnimation spineAnim;

	private bool flyToFall;

	private bool hitGround;
}
