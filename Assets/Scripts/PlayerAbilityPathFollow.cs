using System;
using UnityEngine;

public class PlayerAbilityPathFollow : CharacterState
{
	public override void Start()
	{
		base.Start();
		this.pm = this.pab.GetComponent<PlatformMovement>();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.HandleCarry();
	}

	private void HandleCarry()
	{
		if (this.pAttr.isOnGround)
		{
			GameObject groundGameObject = this.pm.GetGroundGameObject();
			if (groundGameObject != null)
			{
				PathFollow component = groundGameObject.GetComponent<PathFollow>();
				if (component != null)
				{
					Vector3 v = component.CurrentSpeed * Time.fixedDeltaTime;
					this.pm.position += (Vector2)v;
				}
			}
		}
	}

	public PlatformMovement pm;
}
