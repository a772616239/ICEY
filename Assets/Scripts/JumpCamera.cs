using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class JumpCamera : BaseBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			R.Camera.Controller.IsFollowPivot = false;
		}
	}

	private void OnDestroy()
	{
		R.Camera.Controller.IsFollowPivot = true;
	}
}
