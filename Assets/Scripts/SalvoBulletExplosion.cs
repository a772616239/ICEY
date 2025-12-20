using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class SalvoBulletExplosion : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.8f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void OnEnable()
	{
		this.isUsed = false;
		this.groundDisbale = false;
		base.GetComponent<Collider2D>().enabled = true;
	}

	private void Update()
	{
		if (this.isOnGround && !this.groundDisbale)
		{
			this.groundDisbale = true;
			base.StartCoroutine(this.DisableCollider());
		}
	}

	private IEnumerator DisableCollider()
	{
		yield return new WaitForSeconds(0.5f);
		base.GetComponent<Collider2D>().enabled = false;
		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox" && !this.isUsed)
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.gameObject, this.origin, this.damage, Incrementor.GetNextId(), this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
			this.isUsed = true;
		}
	}

	public void SetDamage(int _damage)
	{
		this.damage = _damage;
	}

	public void SetAtkData(JsonData data, GameObject origin)
	{
		this.atkData = data;
		this.origin = origin;
	}

	public int damage;

	private JsonData atkData;

	private GameObject origin;

	private bool isUsed;

	private bool groundDisbale;
}
