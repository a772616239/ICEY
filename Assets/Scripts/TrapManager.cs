using System;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
		if (this.distance == 0f)
		{
			this.startLunch = true;
		}
	}

	private void Update()
	{
		this.Lunch();
		if (this.startLunch)
		{
			return;
		}
		if (Mathf.Abs(this.player.position.x - (base.transform.position.x + this.distance)) <= 1f)
		{
			this.startLunch = true;
		}
	}

	private void Lunch()
	{
		if (!this.startLunch)
		{
			return;
		}
		this.currentTime -= Time.deltaTime;
		if (this.currentTime <= 0f)
		{
			this.currentTime = this.lunchInterval;
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.prefab, base.transform.position + (Vector3)this.lunchOffset, Quaternion.identity);
			transform.rotation = Quaternion.Euler(this.euler);
			transform.GetComponent<Rigidbody2D>().velocity = this.speed;
			if (transform.GetComponent<TrapAttack>() != null)
			{
				transform.GetComponent<TrapInfoSetting>().SetInfo(this.atkType, this.damage, (float)this.atkInterval);
			}
		}
	}

	[Header("发射间隔")]
	public float lunchInterval;

	public Transform prefab;

	[Header("发射速度")]
	public Vector2 speed;

	public Vector3 euler;

	[Header("发射点")]
	public Vector2 lunchOffset;

	private float currentTime;

	[Header("导弹伤害")]
	public int damage;

	[Header("导弹伤害间隔")]
	public int atkInterval;

	[Header("导弹攻击类型")]
	public TrapAttack.AttackType atkType;

	[Header("触发距离")]
	public float distance;

	private bool startLunch;
}
