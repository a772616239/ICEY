using System;
using GameWorld;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveAreaLimit : BaseBehaviour
{
	private void Start()
	{
		if (base.GetComponent<PlayerAction>() != null)
		{
			this.type = MoveAreaLimit.LimitType.Player;
			this.bodySize = base.GetComponent<Collider2D>().bounds.size;
		}
		this.body = base.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (this._frameCount < 1)
		{
			this._frameCount++;
			return;
		}
		this._frameCount = 0;
		float num = 0.05f;
		Vector3 position = base.transform.position;
		Vector3 v = this.body.velocity;
		if (this.type == MoveAreaLimit.LimitType.Player)
		{
			float num2 = GameArea.PlayerRange.xMin + this.bodySize.x / 2f;
			float num3 = GameArea.PlayerRange.xMax - this.bodySize.x / 2f;
			if (position.x > num3 + num || position.x < num2 - num)
			{
				EventManager.PostEvent<MoveAreaLimit, HitWallArgs>("HitWall", this, new HitWallArgs(base.gameObject));
				v.x = 0f;
				this.body.velocity = v;
				this.body.position = new Vector2(Mathf.Clamp(position.x, num2, num3), Mathf.Clamp(position.y, GameArea.PlayerRange.yMin, GameArea.PlayerRange.yMax));
			}
			if (position.y > GameArea.PlayerRange.yMax + num)
			{
				v.y = 0f;
				this.body.velocity = v;
				this.body.position = new Vector2(Mathf.Clamp(position.x, num2, num3), Mathf.Clamp(position.y, GameArea.PlayerRange.yMin, GameArea.PlayerRange.yMax));
			}
		}
		float yMax = GameArea.MapRange.yMax;
		float yMin = GameArea.MapRange.yMin;
		Vector3 position2 = base.transform.position;
		if (yMin > position2.y)
		{
			position2.y = yMax;
			base.transform.position = position2;
			Log.Error(base.transform.name + "击穿了地面");
		}
	}

	public MoveAreaLimit.LimitType type = MoveAreaLimit.LimitType.Enemy;

	public Vector2 bodySize = new Vector2(0.5f, 0f);

	public int battleZoneId = 1;

	private EnemyAttribute eAttr;

	private Rigidbody2D body;

	private const int Freq = 1;

	private int _frameCount;

	public enum LimitType
	{
		Player,
		Enemy,
		Map
	}
}
