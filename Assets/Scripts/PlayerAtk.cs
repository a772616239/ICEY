using System;
using ExtensionMethods;
using GameWorld;
using LitJson;
using UnityEngine;

public class PlayerAtk : BaseBehaviour
{
	private void Start()
	{
		this._collider = base.GetComponent<Collider2D>();
	}

	public void SetData(JsonData atkData, int atkId)
	{
		this.data = atkData;
		this.attackId = atkId;
		this._hitTimes = this.data.Get<int>("hitTimes", 0);
		this._interval = this.data.Get<float>("interval", 100f);
		this._hitType = (PlayerAtk.HitType)this.data.Get<int>("hitType", 0);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EnemyHurtBox"))
		{
			this.EventTrigger(this.EventArgs(other, true));
		}
		else if (other.CompareTag("EnemyBullet"))
		{
			this.BreakBullet(other);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (SingletonMono<WorldTime>.Instance.IsFrozen || this._hitType == PlayerAtk.HitType.Once || this.data == null)
		{
			return;
		}
		this._interval -= Time.deltaTime;
		if (this._interval > 0f)
		{
			return;
		}
		if (other.CompareTag("EnemyHurtBox"))
		{
			PlayerAtk.HitType hitType = this._hitType;
			if (hitType != PlayerAtk.HitType.Limited)
			{
				if (hitType == PlayerAtk.HitType.UnLimited)
				{
					this.UnlimitedAttack(other);
				}
			}
			else
			{
				this.LimitedAttack(other);
			}
		}
	}

	private void UnlimitedAttack(Collider2D other)
	{
		this._interval = this.data.Get<float>("interval", 0f);
		this.attackId = Incrementor.GetNextId();
		this.EventTrigger(this.EventArgs(other, false));
	}

	private void LimitedAttack(Collider2D other)
	{
		if (this._hitTimes > 0)
		{
			this._interval = this.data.Get<float>("interval", 0f);
			this.attackId = Incrementor.GetNextId();
			this.EventTrigger(this.EventArgs(other, false));
			this._hitTimes--;
		}
	}

	private void BreakBullet(Collider2D bullet)
	{
		if (R.Player.Action.stateMachine.currentState.IsInArray(PlayerAtkType.CantBreakBullet))
		{
			return;
		}
		EnemyBullet component = bullet.GetComponent<EnemyBullet>();
		EnemyBulletLaucher component2 = bullet.GetComponent<EnemyBulletLaucher>();
		if (component != null)
		{
			component.beAtked = true;
			component.HitBullet();
		}
		else if (component2 != null)
		{
			component2.beAtked = true;
			component2.HitBullet();
		}
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(7, WorldTime.FrozenArgs.FrozenType.All, true);
		R.Camera.Controller.CameraShake(0.166666672f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	private Vector2 HurtPos(Bounds enemyBound)
	{
		return MathfX.Intersect2DCenter(enemyBound, this._collider.bounds);
	}

	private EnemyHurtAtkEventArgs EventArgs(Collider2D enemyBody, bool firstHurt)
	{
		return new EnemyHurtAtkEventArgs(enemyBody.transform.parent.gameObject, base.gameObject, this.attackId, this.HurtPos(enemyBody.bounds), HurtCheck.BodyType.Body, new EnemyHurtAtkEventArgs.PlayerNormalAtkData(this.data, firstHurt), false);
	}

	private void EventTrigger(EnemyHurtAtkEventArgs args)
	{
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", base.gameObject, args);
	}

	public JsonData data;

	public int attackId;

	private float _interval;

	private int _hitTimes;

	private PlayerAtk.HitType _hitType;

	private Collider2D _collider;

	public enum HitType
	{
		Once,
		Limited,
		UnLimited
	}
}
