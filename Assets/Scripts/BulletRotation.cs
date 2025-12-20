using System;
using UnityEngine;

public class BulletRotation : BaseBehaviour
{
	private bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask);
			return hit;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.start)
		{
			this.CalculateAngle();
		}
		if (this.isOnGround)
		{
			UnityEngine.Object.Destroy(base.gameObject);
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

	private float speedAngle;

	private bool start;

	private int dir;
}
