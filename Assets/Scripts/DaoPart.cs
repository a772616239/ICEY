using System;
using UnityEngine;

public class DaoPart : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.5f, LayerManager.GroundMask | LayerManager.OneWayGroundMask | LayerManager.ObstacleMask);
			return hit;
		}
	}

	private void Awake()
	{
		this.anim = base.GetComponent<SkeletonAnimation>();
		this._animName = this.anim.AnimationName;
	}

	private void Update()
	{
		if (this.isOnGround && !this.hitGround)
		{
			this.hitGround = true;
			this.anim.state.SetAnimation(0, "HitGround", false);
			R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
		}
	}

	private void OnDisable()
	{
		this.anim.state.SetAnimation(0, this._animName, true);
		this.hitGround = false;
		this.anim.Reset();
	}

	private SkeletonAnimation anim;

	private bool hitGround;

	private string _animName;
}
