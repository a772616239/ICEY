using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class EnemyBullet : BaseBehaviour
{
	private bool _hitGround
	{
		get
		{
			return Physics2D.OverlapBox(base.transform.position, Vector2.one, 0f, LayerManager.WallMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask | LayerManager.CeilingMask | LayerManager.ObstacleMask);
		}
	}

	private void OnEnable()
	{
		this.player = null;
		this.beAtked = false;
	}

	private void Update()
	{
		if (this._hitGround && !this.enableOnGround)
		{
			if (this.explosionEffect > 0)
			{
				R.Effect.Generate(this.explosionEffect, null, base.transform.position, default(Vector3), default(Vector3), true);
			}
			R.Audio.PlayEffect(UnityEngine.Random.Range(136, 139), new Vector3?(base.transform.position));
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.gameObject, this.origin, this.damage, Incrementor.GetNextId(), this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
			if (this.hitAudio > 0)
			{
				R.Audio.PlayEffect(this.hitAudio, new Vector3?(base.transform.position));
			}
			if (!this.beAtked && this.type == EnemyBullet.BUlletType.Once)
			{
				this.player = other.transform.parent;
				this.HitBullet();
			}
			if (this.type == EnemyBullet.BUlletType.Stay)
			{
				UnityEngine.Object.Destroy(base.GetComponent<Collider2D>());
			}
		}
	}

	public void HitBullet()
	{
		if (this.beAtked)
		{
			if (this.explosionEffect > 0)
			{
				R.Effect.Generate(this.explosionEffect, null, base.transform.position, default(Vector3), default(Vector3), true);
			}
			R.Effect.Generate(49, null, base.transform.position, default(Vector3), default(Vector3), true);
			R.Audio.PlayEffect(UnityEngine.Random.Range(136, 139), new Vector3?(base.transform.position));
		}
		if (this.player)
		{
			if (this.explosionEffect > 0)
			{
				R.Effect.Generate(this.explosionEffect, null, new Vector3((base.transform.position.x + this.player.position.x) / 2f, base.transform.position.y, base.transform.position.z), default(Vector3), default(Vector3), true);
			}
			R.Audio.PlayEffect(UnityEngine.Random.Range(136, 139), new Vector3?(base.transform.position));
		}
		EffectController.TerminateEffect(base.gameObject);
	}

	private void OnDisable()
	{
		if (this.missAudio > 0)
		{
			R.Audio.PlayEffect(this.missAudio, new Vector3?(base.transform.position));
		}
	}

	public void SetAtkData(JsonData jsonData)
	{
		this.atkData = jsonData;
	}

	private Transform player;

	public bool beAtked;

	public int damage;

	public JsonData atkData;

	[HideInInspector]
	public GameObject origin;

	[SerializeField]
	public int explosionEffect = 90;

	[SerializeField]
	private bool enableOnGround;

	public EnemyBullet.BUlletType type;

	public EnemyType EnemyTypeOfShooter;

	public int hitAudio;

	public int missAudio;

	public enum BUlletType
	{
		Once,
		Stay,
		Continue
	}
}
