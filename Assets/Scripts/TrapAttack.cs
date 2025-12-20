using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class TrapAttack : MonoBehaviour
{
	private void OnEnable()
	{
		this.atkData = JsonMapper.ToObject(this.data);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			this.currentTime = this.interval;
			this.PlayerHurt(other);
		}
	}

	public void SetInfo(TrapAttack.AttackType type, int atkDamage, float atkInterval)
	{
		this.attackType = type;
		this.damage = atkDamage;
		this.interval = atkInterval;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (this.attackType == TrapAttack.AttackType.Once)
		{
			return;
		}
		this.currentTime -= Time.deltaTime;
		if (this.currentTime <= 0f)
		{
			this.currentTime = this.interval;
			if (other.name == "PlayerHurtBox")
			{
				this.PlayerHurt(other);
			}
		}
	}

	private void PlayerHurt(Collider2D other)
	{
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.transform.gameObject, base.transform.gameObject, this.damage, Incrementor.GetNextId(), this.atkData, false);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	public TrapAttack.AttackType attackType;

	public int damage;

	public float interval;

	[SerializeField]
	private string data;

	private JsonData atkData;

	private float currentTime;

	public enum AttackType
	{
		Once,
		Mulit
	}
}
