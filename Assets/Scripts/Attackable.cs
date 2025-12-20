using System;
using CIS;
using DG.Tweening;
using UnityEngine;

[SerializeField]
public class Attackable : MonoBehaviour
{
	protected virtual void Start()
	{
		this._next = new Attackable[]
		{
			this.next
		};
	}

	public virtual bool IsFixYSpeed(Attackable next)
	{
		return false;
	}

	public virtual float GetFutureTime(Attackable next)
	{
		return this.futureTime;
	}

	public virtual void PlayAnimationAfterTime(string name, float time)
	{
	}

	public virtual void SetJumpTween(Tween tween)
	{
		this.attackerJumpTween = tween;
	}

	public virtual bool Action(Attackable next)
	{
		return true;
	}

	public virtual bool Available()
	{
		return true;
	}

	public virtual void React(Vector2 dir, float delay = 0f)
	{
	}

	public virtual Attackable[] Next()
	{
		return this._next;
	}

	public virtual bool AffectGravity()
	{
		return false;
	}

	public virtual void Update()
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		if (this.next != null && this.nextNotificationSprite != null)
		{
			Vector2 v = this.next.transform.position - base.transform.position;
			this.nextNotificationSprite.transform.localPosition = Vector2.zero;
			this.nextNotificationSprite.transform.rotation = Quaternion.FromToRotation(Vector2.right, v);
			this.nextNotificationSprite.transform.Translate(this.nextNotificationRadius * 2f, 0f, 0f);
		}
	}

	public virtual void OnPickingTime(float time)
	{
	}

	public Attackable next;

	public int index;

	public float futureTime = 0.13333334f;

	public float gravityFactor = 1f;

	public float horizenalAccelFactor = 1f;

	public float overrideVelY;

	public bool needNextNotification;

	public SpriteRenderer nextNotificationSprite;

	public float nextNotificationRadius;

	protected Tween attackerJumpTween;

	protected Attackable[] _next;
}
