using System;
using UnityEngine;
using Xft;

public class GravityDemo : MonoBehaviour
{
	private void SetUpMissile()
	{
		this.Effect.Active();
		this.Effect.SetCollisionGoalPos(this.Target.transform, "soul");
		this.Effect.SetGravityGoal(this.Target.transform, "soul");
	}

	private void OnMissileCollision(CollisionParam param)
	{
		XffectComponent effect = this.EffectCache.GetEffect("explode");
		effect.transform.position = param.CollidePos;
		effect.Active();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 350f, 30f), "Fire!"))
		{
			this.SetUpMissile();
		}
	}

	public XffectComponent Effect;

	public GameObject Target;

	public XffectCache EffectCache;
}
