using System;
using UnityEngine;

public class SalvoBullet : BaseBehaviour
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
		this.isUseExplosion = false;
	}

	private void Update()
	{
		base.GetComponent<Rigidbody2D>().velocity -= new Vector2(base.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime * 1.2f, 0f);
		if (this.start)
		{
			this.CalculateAngle();
		}
		if (this.isOnGround)
		{
			if (!this.isUseExplosion)
			{
				this.isUseExplosion = true;
				R.Effect.Generate(75, base.transform, new Vector3(0f, 0.5f, 0f), default(Vector3), default(Vector3), true).GetComponent<SalvoBulletExplosion>().SetDamage(this.damage);
			}
			this.start = false;
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	private void CalculateAngle()
	{
		this.speedAngle = Vector2.Angle(base.GetComponent<Rigidbody2D>().velocity, Vector2.up) * (float)this.dir;
		base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.speedAngle));
	}

	public void StartCalculate(int _dir)
	{
		this.dir = _dir;
		this.start = true;
	}

	public void SetDamage(int _damage)
	{
		this.damage = _damage;
	}

	private float speedAngle;

	private bool start;

	private int dir;

	private Transform player;

	public int damage;

	private bool isUseExplosion;
}
