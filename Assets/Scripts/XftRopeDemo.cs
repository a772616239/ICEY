using System;
using UnityEngine;
using Xft;

public class XftRopeDemo : MonoBehaviour
{
	private void SetUpMagicChain()
	{
		this.Effect.Active();
		this.Effect.SetGravityGoal(this.Target.transform, "line");
	}

	private void OnMissileCollision()
	{
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 350f, 30f), "Reset"))
		{
			this.SetUpMagicChain();
		}
	}

	public XffectComponent Effect;

	public GameObject Target;
}
