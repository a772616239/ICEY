using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class GiantBullet : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void Start()
	{
		this.euler = base.transform.rotation.eulerAngles;
		this._rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		this.euler = base.transform.rotation.eulerAngles;
		this._rigidbody2D.velocity = new Vector2(-Mathf.Sin(this.euler.z * 0.0174532924f), Mathf.Cos(this.euler.z * 0.0174532924f));
		if (this.isOnGround)
		{
			if (!this.isUseExplosion)
			{
				this.isUseExplosion = true;
			}
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(collision.transform.parent.gameObject, base.gameObject, this.attacker.gameObject, this.damage, Incrementor.GetNextId(), this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
		}
	}

	[SerializeField]
	private string data;

	[SerializeField]
	private JsonData atkData;

	public int damage;

	private Vector3 euler;

	public Transform attacker;

	private bool isUseExplosion;

	private Rigidbody2D _rigidbody2D;
}
