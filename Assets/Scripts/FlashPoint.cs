using System;
using UnityEngine;

public class FlashPoint : MonoBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform == this.player)
		{
			this.player.position = this.target.position;
		}
	}

	[SerializeField]
	private Transform target;
}
