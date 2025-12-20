using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class FBBullet : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void OnEnable()
	{
		if (this.normalBullet != null)
		{
			this.normalData = JsonMapper.ToObject(this.normalBullet);
		}
		this.atkId = Incrementor.GetNextId();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EnemyHurtBox") && this.canCollide(other))
		{
			this.EventTrigger(other.transform);
			if (!this.canThrough)
			{
				EffectController.TerminateEffect(base.gameObject);
			}
			if (this.enemyEffect != -1)
			{
				R.Effect.Generate(this.enemyEffect, null, other.bounds.center, default(Vector3), default(Vector3), true);
			}
		}
		if (other.CompareTag("EnemyBullet"))
		{
			EnemyBullet component = other.GetComponent<EnemyBullet>();
			EnemyBulletLaucher component2 = other.GetComponent<EnemyBulletLaucher>();
			if (component != null)
			{
				component.beAtked = true;
				R.Camera.Controller.CameraShake(0.166666672f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
				component.HitBullet();
			}
			else if (component2 != null)
			{
				component2.beAtked = true;
				R.Camera.Controller.CameraShake(0.166666672f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
				component2.HitBullet();
			}
		}
	}

	public void SetBulletData(JsonData data)
	{
		this.normalData = data;
	}

	private void Update()
	{
		if (this.isOnGround && this.groundEffect != -1 && this.firstHitGround)
		{
			this.firstHitGround = false;
			R.Effect.Generate(this.groundEffect, null, base.transform.position + new Vector3(0f, -0.4f, 0f), default(Vector3), default(Vector3), true);
			this.ExpandFunction();
			if (this.groundImmediatelyDisable)
			{
				this.Disable();
			}
			else
			{
				base.Invoke("Disable", this.groundDisableDelayTime);
			}
		}
	}

	public virtual void ExpandFunction()
	{
	}

	private void EventTrigger(Transform enemyBody)
	{
		HurtCheck component = enemyBody.GetComponent<HurtCheck>();
		HurtCheck.BodyType body = (!(component != null)) ? HurtCheck.BodyType.Body : component.bodyType;
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(enemyBody.parent.gameObject, base.gameObject, this.atkId, enemyBody.GetComponent<Collider2D>().bounds.center, body, new EnemyHurtAtkEventArgs.PlayerNormalAtkData(this.normalData, true), false);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", base.gameObject, args);
	}

	private void Disable()
	{
		EffectController.TerminateEffect(base.gameObject);
	}

	private bool canCollide(Collider2D other)
	{
		return Vector3.Distance(other.transform.position, base.transform.position) < this.collsionDistance || Mathf.Abs(other.transform.position.x - base.transform.position.x) < this.collsionDistance;
	}

	public float collsionDistance = 3f;

	[SerializeField]
	private string normalBullet;

	[SerializeField]
	private bool canThrough;

	[SerializeField]
	private Transform pulse;

	[SerializeField]
	private int enemyEffect = -1;

	[SerializeField]
	private int groundEffect = -1;

	private JsonData normalData;

	private int atkId;

	[Header("在地面上立即消失")]
	public bool groundImmediatelyDisable = true;

	[Header("地面延时消失时间")]
	public float groundDisableDelayTime;

	public bool firstHitGround = true;
}
