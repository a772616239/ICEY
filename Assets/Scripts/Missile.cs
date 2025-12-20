using System;
using LitJson;
using UnityEngine;

public class Missile : BaseBehaviour
{
	private void Awake()
	{
		this._rigidbody2D = base.GetComponent<Rigidbody2D>();
		this._spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		this.hasCollide = false;
		this.firstHitGround = true;
		this._rigidbody2D.gravityScale = 4f;
		this._spriteRenderer.enabled = true;
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		if (this.isOnGround && this.firstHitGround && !this.hasCollide)
		{
			this.hasCollide = true;
			this.firstHitGround = false;
			this.WaitExplosion();
		}
	}

	private bool isOnGround
	{
		get
		{
			return Physics2D.Raycast(base.transform.position, -Vector2.up, 0.8f, LayerManager.GroundMask).collider != null;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox" && !this.hasCollide)
		{
			this.hasCollide = true;
			this.StartExplosion();
		}
	}

	private void WaitExplosion()
	{
		this._rigidbody2D.gravityScale = 0f;
		this._rigidbody2D.velocity = Vector2.zero;
		this._spriteRenderer.enabled = false;
		R.Audio.PlayEffect(274, new Vector3?(base.transform.position));
		Transform transform = R.Effect.Generate(198, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		SalvoBulletExplosion component = transform.GetComponent<SalvoBulletExplosion>();
		component.SetAtkData(this.atkData, this.origin);
		component.SetDamage(this.damage);
	}

	private void StartExplosion()
	{
		this._rigidbody2D.gravityScale = 0f;
		this._rigidbody2D.velocity = Vector2.zero;
		this._spriteRenderer.enabled = false;
		R.Audio.PlayEffect(274, new Vector3?(base.transform.position));
		Transform transform = R.Effect.Generate(198, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		SalvoBulletExplosion component = transform.GetComponent<SalvoBulletExplosion>();
		component.SetAtkData(this.atkData, this.origin);
		component.SetDamage(this.damage);
	}

	public void SetAtkData(JsonData jsonData, GameObject origin)
	{
		this.atkData = jsonData;
		this.origin = origin;
	}

	public int damage;

	private JsonData atkData;

	[HideInInspector]
	public Transform attacker;

	private GameObject origin;

	public bool hasCollide;

	public bool firstHitGround = true;

	private Rigidbody2D _rigidbody2D;

	private SpriteRenderer _spriteRenderer;
}
