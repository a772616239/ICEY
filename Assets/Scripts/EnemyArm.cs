using System;
using UnityEngine;

public class EnemyArm : BaseBehaviour
{
	public void SetAngularSpeed(float speed)
	{
		base.GetComponent<Rigidbody2D>().angularVelocity = speed;
	}
}
