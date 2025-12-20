using System;
using LitJson;
using UnityEngine;

public class SpiderAnimEvent : BaseBehaviour
{
	private void Awake()
	{
		this._action = base.GetComponent<SpiderAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.蜘蛛];
	}

	private void Update()
	{
		if (this._eAttr.isDead)
		{
			return;
		}
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
				this._action.AnimChangeState(SpiderAction.StateEnum.FlyToFall, 1f);
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
				this._action.AnimChangeState(SpiderAction.StateEnum.HitToFly1, 1f);
			}
			else
			{
				this._action.AnimChangeState(SpiderAction.StateEnum.FallHitGround, 1f);
			}
		}
	}

	public void ChangeState(SpiderAction.StateEnum sta)
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

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void GetUp()
	{
		if (this._eAttr.isDead)
		{
			this.DestroySelf();
		}
		else
		{
			this._action.AnimChangeState(SpiderAction.StateEnum.GetUp, 1f);
		}
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public float maxFlyHeight;

	private SpiderAction _action;

	private EnemyAttribute _eAttr;

	private JsonData _jsonData;

	private EnemyAtk _enemyAtk;
}
