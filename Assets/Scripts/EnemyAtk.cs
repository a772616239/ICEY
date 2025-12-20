using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class EnemyAtk : BaseBehaviour
{
	public JsonData atkData
	{
		private get
		{
			return this._atkData;
		}
		set
		{
			this._atkData = value;
			this.hitTimes = value.Get<int>("hitTimes", 0);
			this.hitInterval = value.Get<float>("hitInterval", 0f);
			this.hitType = value.Get<int>("hitType", 1);
		}
	}

	private void Start()
	{
		this.eAttr = base.transform.parent.GetComponent<EnemyAttribute>();
	}

	private void Update()
	{
		if (this.atkStart)
		{
			this.hitInterval = Mathf.Clamp(this.hitInterval - Time.deltaTime, 0f, float.PositiveInfinity);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.transform.parent.gameObject, base.transform.parent.gameObject, this.eAttr.atk, this.atkId, this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
			this.atkStart = true;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (SingletonMono<WorldTime>.Instance.IsFrozen)
		{
			return;
		}
		if (this.atkData == null)
		{
			return;
		}
		if (other.name == "PlayerHurtBox")
		{
			if (this.hitType == 0)
			{
				this.UnlimitedAttack(other);
			}
			else if (this.hitType == 1)
			{
				this.LimitedAttack(other);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			this.atkStart = false;
		}
	}

	private void UnlimitedAttack(Collider2D other)
	{
		if (this.hitInterval <= 0f)
		{
			this.atkId = Incrementor.GetNextId();
			this.hitInterval = this.atkData.Get<float>("hitInterval", 0f);
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.transform.parent.gameObject, base.transform.parent.gameObject, this.eAttr.atk, this.atkId, this.atkData, false);
			EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
		}
	}

	private void LimitedAttack(Collider2D other)
	{
		if (this.hitTimes > 0)
		{
			this.hitInterval -= Time.deltaTime;
			if (this.hitInterval <= 0f)
			{
				this.atkId = Incrementor.GetNextId();
				this.hitInterval = this.atkData.Get<float>("hitInterval", 0f);
				PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, base.transform.parent.gameObject, base.transform.parent.gameObject, this.eAttr.atk, this.atkId, this.atkData, false);
				EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
				this.hitTimes--;
			}
		}
	}

	private JsonData _atkData;

	private EnemyAttribute eAttr;

	public int atkId;

	private int hitTimes;

	private float hitInterval;

	private int hitType;

	public bool atkStart;
}
