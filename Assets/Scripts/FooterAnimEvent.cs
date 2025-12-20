using System;
using LitJson;
using UnityEngine;

public class FooterAnimEvent : BaseBehaviour
{
	private void Awake()
	{
		this._action = base.GetComponent<FooterAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.大脚];
	}

	private void Update()
	{
		if (this._eAttr.isDead)
		{
			return;
		}
		if (this._eAttr.isFlyingUp)
		{
			bool flag = this.MaxFlyHeight > 0f && this._eAttr.height >= this.MaxFlyHeight;
			if (flag)
			{
				Vector2 currentSpeed = this._eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				this._eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (this._eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				this._eAttr.isFlyingUp = false;
				this._action.AnimChangeState(FooterAction.StateEnum.FlyToFall, 1f);
			}
		}
		if (this._eAttr.checkHitGround && this._eAttr.isOnGround)
		{
			this._eAttr.checkHitGround = false;
			if (this._action.stateMachine.currentState == "HitFall")
			{
				this.MaxFlyHeight = 4f;
				this._eAttr.timeController.SetSpeed(Vector2.up * 25f);
				this._action.AnimChangeState(FooterAction.StateEnum.HitToFly, 1f);
			}
			else
			{
				this._action.AnimChangeState(FooterAction.StateEnum.FallHitGround, 1f);
			}
		}
	}

	public void ChangeState(FooterAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void FlyUp()
	{
		this._eAttr.isFlyingUp = true;
	}

	public void HitGround()
	{
		this._eAttr.checkHitGround = true;
	}

	public void GetUp()
	{
		if (!this._eAttr.isDead)
		{
			this._action.AnimChangeState(FooterAction.StateEnum.GetUp, 1f);
		}
		else
		{
			this.DestroySelf();
		}
	}

	public void BackToIdle()
	{
		this._action.AnimChangeState(FooterAction.StateEnum.Idle, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void DestroySelf()
	{
		this.RealDestroy();
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void PlayMoveAudio()
	{
		int id = this.moveAudio[UnityEngine.Random.Range(0, this.moveAudio.Length)];
		this.PlayAudio(id);
	}

	public void SpwanEffect()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.footerEffect, base.transform.position + Vector3.back * 0.01f, Quaternion.identity);
		gameObject.GetComponent<SkeletonAnimation>().state.SetAnimation(0, this._action.stateMachine.currentState, false);
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.AddComponent<AutoDestroy>();
		UnityEngine.Object.Destroy(gameObject.GetComponent<EnemyEffectSync>());
	}

	public float MaxFlyHeight;

	private FooterAction _action;

	private EnemyAttribute _eAttr;

	private EnemyAtk _enemyAtk;

	private JsonData _jsonData;

	[SerializeField]
	private int[] moveAudio;

	[SerializeField]
	private GameObject footerEffect;
}
