using System;
using System.Collections;
using UnityEngine;

public class DahalBullet : BaseBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void Awake()
	{
		this._rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		this.player = R.Player.Transform;
		this.isUseExplosion = false;
		this.startTime = Time.time;
		base.StartCoroutine(this.Rise());
	}

	private IEnumerator Rise()
	{
		float angle = 90f - this.riseAngle;
		this.riseSpeedVector2 = new Vector2(Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f)) * this.riseSpeed;
		while (this.riseTime > this.deltaTime)
		{
			yield return new WaitForFixedUpdate();
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
			this._rigidbody2D.velocity = this.riseSpeedVector2;
		}
		this._rigidbody2D.velocity = Vector2.zero;
		yield break;
	}

	[SerializeField]
	public float angularSpeed;

	[SerializeField]
	public float speed;

	[SerializeField]
	public float riseTime;

	[SerializeField]
	public float riseSpeed;

	[SerializeField]
	public float riseAngle;

	[SerializeField]
	public float riseAcceleration;

	private Transform player;

	private Vector2 riseSpeedVector2;

	private bool isUseExplosion;

	private float startTime;

	private Rigidbody2D _rigidbody2D;
}
