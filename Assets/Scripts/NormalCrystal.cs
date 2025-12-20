using System;
using UnityEngine;

public class NormalCrystal : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.18f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void OnEnable()
	{
		this.canUse = false;
		base.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-this.speed.x, this.speed.x), UnityEngine.Random.Range(3f, this.speed.y));
	}

	private void Update()
	{
		if (this.isOnGround)
		{
			if (base.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f)
			{
				base.transform.Rotate(Vector3.back, (float)this.rotation * Time.deltaTime);
				base.GetComponent<Rigidbody2D>().velocity = new Vector2(base.GetComponent<Rigidbody2D>().velocity.x * (1f - this.friction), -base.GetComponent<Rigidbody2D>().velocity.y * this.bounce);
			}
			else
			{
				this.canUse = true;
				base.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
		else
		{
			base.transform.Rotate(Vector3.back, (float)this.rotation * Time.deltaTime);
			base.GetComponent<Rigidbody2D>().AddForce(this.gravity);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox" && this.canUse)
		{
			R.Effect.Generate(97, null, base.transform.position, default(Vector3), default(Vector3), true);
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	[SerializeField]
	private Vector2 speed;

	[SerializeField]
	private Vector2 gravity;

	[SerializeField]
	private int rotation;

	[SerializeField]
	private float friction = 0.1f;

	[SerializeField]
	private float bounce = 1f;

	private bool canUse;
}
