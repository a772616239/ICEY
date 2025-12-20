using System;
using System.Collections;
using UnityEngine;

public class RedMoney : BaseBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	private void Awake()
	{
		this._rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		this.player = R.Player.Transform;
	}

	private void FixedUpdate()
	{
		if (!this.isOnGround)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, Vector2.down, 10f, LayerManager.GroundMask);
			if (raycastHit2D.distance < 0.5f || base.transform.position.y < raycastHit2D.point.y)
			{
				this._rigidbody2D.gravityScale = 0f;
				this._rigidbody2D.velocity = Vector2.zero;
				this.isOnGround = true;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!this.hasFly && other.name == "PlayerHurtBox" && this.coinValue != -1)
		{
			this._rigidbody2D.gravityScale = 0f;
			this._rigidbody2D.velocity = Vector2.zero;
			R.Audio.PlayEffect(193, new Vector3?(this.player.position));
			this.hasFly = true;
			this.startTime = Time.time;
			base.StartCoroutine(this.Fly());
		}
	}

	private IEnumerator Fly()
	{
		float randomX = (float)((UnityEngine.Random.Range(-1f, 1f) <= 0f) ? -1 : 1) * UnityEngine.Random.Range(-2f, -1f);
		float randomY = ((UnityEngine.Random.Range(-1f, 1f) <= 0f) ? -1f : 0.5f) * UnityEngine.Random.Range(-1f, -0.5f);
		Vector2 randomFore = new Vector2(randomX, randomY);
		Vector3 moneyPos = base.transform.position;
		while (this.deltaTime < this.flyingTime && Vector3.Distance(moneyPos, base.transform.position) < 5f)
		{
			this._rigidbody2D.AddForce(randomFore * 25f);
			yield return new WaitForFixedUpdate();
		}
		this._rigidbody2D.AddForce(new Vector2(0f, 0f));
		yield return new WaitForSeconds(0.3f);
		for (;;)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.player.position + this.repairPosition, 0.7f * this.speed * this.deltaTime);
			if (Vector3.Distance(base.transform.position, this.player.position + this.repairPosition) < 0.2f)
			{
				R.Equipment.CoinNum += this.coinValue;
				R.Effect.Generate(159, this.player, this.repairPosition, default(Vector3), default(Vector3), true);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	private bool hasFly;

	private bool isOnGround;

	public float speed = 1f;

	public int coinValue = -1;

	public float flyingTime = 0.1f;

	private float startTime;

	private Transform player;

	private Vector3 repairPosition = new Vector3(0f, 1.2f, -0.1f);

	private Rigidbody2D _rigidbody2D;
}
