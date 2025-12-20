using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class BombKillerAnimEvent : BaseBehaviour
{
	private void Awake()
	{
		this._action = base.GetComponent<BombKillerAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.半身自爆];
	}

	private void Update()
	{
		if (this._eAttr.isFlyingUp)
		{
			bool flag = this.maxFlyHeight > 0f && this._eAttr.height >= this.maxFlyHeight;
			if (flag)
			{
				Vector2 currentSpeed = this._eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				this._eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (this._eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				this._eAttr.isFlyingUp = false;
				this._action.AnimChangeState(BombKillerAction.StateEnum.FlyToFall, 1f);
			}
		}
		if (this._eAttr.checkHitGround && this._eAttr.isOnGround)
		{
			this._eAttr.checkHitGround = false;
			R.Effect.Generate(6, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			if (this._action.stateMachine.currentState == "HitFall")
			{
				this.maxFlyHeight = 4f;
				this._eAttr.timeController.SetSpeed(Vector2.up * 25f);
				this._action.AnimChangeState(BombKillerAction.StateEnum.HitToFly1, 1f);
			}
			else
			{
				this._action.AnimChangeState(BombKillerAction.StateEnum.HitGround, 1f);
			}
		}
	}

	public void ChangeState(BombKillerAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void HitGround()
	{
		this._eAttr.checkHitGround = true;
	}

	public void FlyUp()
	{
		this._eAttr.isFlyingUp = true;
	}

	public void GetUp()
	{
		if (this._eAttr.isDead)
		{
			this.DieBlock();
			this.DestroySelf();
		}
		else
		{
			this._action.AnimChangeState(BombKillerAction.StateEnum.GetUp, 1f);
		}
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
		}
		this._action.AnimChangeState(BombKillerAction.StateEnum.Idle, 1f);
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void Atk1DieMessage()
	{
		EventManager.PostEvent<EnemyAttribute>("EnemyKilled", this._eAttr);
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", Time.deltaTime);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetAtkData()
	{
		base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
		base.GetComponentInChildren<EnemyAtk>().atkData = this._jsonData[this._action.stateMachine.currentState];
	}

	public void Atk1Finish()
	{
		if (this._action.atk1Success)
		{
			this._action.AnimChangeState(BombKillerAction.StateEnum.Atk1Success, 1f);
		}
		else
		{
			this._action.AnimChangeState(BombKillerAction.StateEnum.Atk1Fail, 1f);
		}
	}

	public void Atk1FollowOver()
	{
		this._action.atk1Success = false;
	}

	public void GenerateExplosion_Atk1()
	{
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._jsonData["Atk1Ready"], true);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	public void GenerateExplosionEffectNoDamage()
	{
		Transform transform = R.Effect.Generate(162, base.transform, Vector2.up * 1.2f, Vector3.zero, Vector3.one * 0.4f, true);
		UnityEngine.Object.Destroy(transform.GetChild(1).gameObject);
	}

	public void GenerateExplosionEffect()
	{
		Transform transform = R.Effect.Generate(162, base.transform, Vector2.up * 1.2f, default(Vector3), default(Vector3), true);
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData[this._action.stateMachine.currentState]);
		componentInChildren.damage = this._eAttr.atk;
		componentInChildren.origin = null;
	}

	private BombKillerAction _action;

	private EnemyAttribute _eAttr;

	public float maxFlyHeight;

	private JsonData _jsonData;
}
