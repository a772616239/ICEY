using System;
using UnityEngine;

public class WarningDisable : BaseBehaviour
{
	private void Start()
	{
		this.player = R.Player.Transform;
		base.Invoke("FollowPlayer", 1f);
	}

	private void Update()
	{
		if (this.followPlayer)
		{
			base.transform.position = new Vector3(this.player.position.x, 0f, this.player.position.z);
		}
	}

	private void FollowPlayer()
	{
		this.followPlayer = false;
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.cannonBullet);
		transform.position = R.Player.Transform.position + Vector3.up * 10f;
		transform.GetComponent<Rigidbody2D>().velocity = Vector2.up * -10f;
		base.Invoke("DestroySelf", 0.5f);
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private bool followPlayer = true;

	private Transform player;

	[SerializeField]
	private Transform cannonBullet;
}
