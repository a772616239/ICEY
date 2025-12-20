using System;
using UnityEngine;

public class SalvoGenerator : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init(int damage)
	{
		for (int i = 0; i < 5; i++)
		{
			Transform transform = R.Effect.Generate(72, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			transform.GetComponent<Rigidbody2D>().velocity = new Vector3(-this.speed + this.speed * (float)i * 0.4f, UnityEngine.Random.Range(-1.7f, 1.7f), 0f);
			transform.GetComponent<SalvoBullet>().StartCalculate((transform.GetComponent<Rigidbody2D>().velocity.x <= 0f) ? 1 : -1);
			transform.GetComponent<SalvoBullet>().SetDamage(damage);
		}
	}

	public float speed = 6f;
}
