using System;
using UnityEngine;

public class BulletHitGround : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			return Physics2D.Raycast(base.transform.position, new Vector2(0f, -1f), 0.36f, LayerManager.GroundMask).collider != null;
		}
	}

	private void Update()
	{
		if (!this.canThrough && this.isOnGround && this.groundEffect != -1)
		{
			R.Effect.Generate(this.groundEffect, null, base.transform.position + new Vector3(0f, -0.3f, 0f), default(Vector3), default(Vector3), true);
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	[SerializeField]
	private bool canThrough;

	[SerializeField]
	private int groundEffect = -1;
}
