using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class EnemyBulletLaucher : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			return Physics2D.Raycast(base.transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask).collider != null;
		}
	}

	private void OnEnable()
	{
		this.player = null;
		this.beAtked = false;
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
		if (this.disablePlayEffect != -1)
		{
			R.Effect.Generate(this.disablePlayEffect, null, base.transform.position, default(Vector3), default(Vector3), true);
		}
	}

	private void Update()
	{
		if (this.isOnGround && this.groundEffect != -1)
		{
			R.Effect.Generate(this.groundEffect, null, base.transform.position + new Vector3(0f, -0.3f, 0f), default(Vector3), default(Vector3), true);
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, this.attacker.gameObject, this.damage, Incrementor.GetNextId(), this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
			if (!this.beAtked)
			{
				this.player = R.Player.Transform;
			}
			this.HitBullet();
		}
	}

	public void HitBullet()
	{
		if (this.beAtked && this.hitEffect != -1)
		{
			R.Effect.Generate(this.hitEffect, null, base.transform.position, default(Vector3), default(Vector3), true);
			R.Effect.Generate(49, null, base.transform.position, default(Vector3), default(Vector3), true);
		}
		if (this.player && this.hitEffect != -1)
		{
			R.Effect.Generate(this.hitEffect, null, new Vector3((base.transform.position.x + this.player.position.x) / 2f, base.transform.position.y, base.transform.position.z), default(Vector3), default(Vector3), true);
		}
		EffectController.TerminateEffect(base.gameObject);
	}

	public void SetVelocity(float speed, float angle)
	{
		base.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Mathf.Sin(angle * 0.0174532924f), speed * Mathf.Cos(angle * 0.0174532924f));
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -angle));
	}

	public void SetAtkData(JsonData data)
	{
		this.atkData = data;
	}

	[SerializeField]
	public float speed;

	[SerializeField]
	public float angle;

	[SerializeField]
	private bool canThrough;

	[SerializeField]
	private int hitEffect = -1;

	[SerializeField]
	private int groundEffect = -1;

	[SerializeField]
	private int disablePlayEffect = -1;

	private JsonData atkData;

	[SerializeField]
	public bool beAtked;

	public int damage;

	private Transform player;

	public Transform attacker;
}
