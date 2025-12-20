using System;
using System.Collections;
using UnityEngine;

public class ARBullet : BaseBehaviour
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
		this.player = R.Player.Transform;
		this.direction = this.player.transform.position + new Vector3(0f, 1.24f, 0f) - base.transform.position;
		base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)((this.direction.x <= 0f) ? 90 : -90)));
		this.isUseExplosion = false;
		base.StartCoroutine(this.CalculateDirection());
	}

	private void Start()
	{
	}

	private void Update()
	{
		this.CalculateAngle();
		if (this.isOnGround)
		{
			if (!this.isUseExplosion)
			{
				this.isUseExplosion = true;
				R.Effect.Generate(75, base.transform, new Vector3(0f, 0.4f, 0f), default(Vector3), default(Vector3), true);
				R.Audio.PlayEffect(UnityEngine.Random.Range(136, 139), new Vector3?(base.transform.position));
			}
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	private void CalculateAngle()
	{
		this.dir = ((this.direction.x <= 0f) ? 1 : -1);
		float z = Vector2.Angle(base.GetComponent<Rigidbody2D>().velocity, Vector2.up) * (float)this.dir;
		base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, z));
	}

	private IEnumerator CalculateDirection()
	{
		yield return new WaitForSeconds(this.waitTime);
		if (this.canUse)
		{
			R.Audio.PlayEffect(88, new Vector3?(base.transform.position));
		}
		this.direction = this.player.transform.position + (Vector3)new Vector2(0f, 1.24f) - base.transform.position;
		this.direction = Quaternion.AngleAxis((float)this.rotation, Vector3.back) * this.direction;
		Rigidbody2D component = base.GetComponent<Rigidbody2D>();
		Quaternion quaternion = Quaternion.AngleAxis((float)this.rotation, Vector3.back);
		Vector2 vector = new Vector2(this.direction.x, this.direction.y);
		component.velocity = quaternion * vector.normalized * 10f;
		this.canUse = false;
		yield break;
	}

	public float waitTime;

	public int rotation = -15;

	public bool canUse;

	private float speedAngle;

	private int dir;

	private Transform player;

	private Vector3 direction;

	private bool isUseExplosion;
}
