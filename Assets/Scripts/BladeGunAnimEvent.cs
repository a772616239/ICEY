using System;
using System.Collections;
using LitJson;
using UnityEngine;

public class BladeGunAnimEvent : BaseBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Awake()
	{
		this._action = base.GetComponent<BladeGunAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.霰弹利刃];
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
				this._action.AnimChangeState(BladeGunAction.StateEnum.FlyToFall, 1f);
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
				this._action.AnimChangeState(BladeGunAction.StateEnum.HitToFly1, 1f);
			}
			else
			{
				this._action.AnimChangeState(BladeGunAction.StateEnum.FallHitGround, 1f);
			}
		}
	}

	public void ChangeState(BladeGunAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta.ToString(), 1f);
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
		this.SetAtkId();
	}

	public void SetAtkId()
	{
		this._enemyAtk.atkId = Incrementor.GetNextId();
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

	public void Shoot()
	{
		Transform transform = R.Effect.Generate(207, null, default(Vector3), default(Vector3), default(Vector3), true);
		EnemyBullet component = transform.GetComponent<EnemyBullet>();
		component.damage = this._eAttr.atk;
		component.origin = base.gameObject;
		transform.position = this.gunPos.position;
		component.SetAtkData(this._jsonData[this._action.stateMachine.currentState]);
		Vector2 from = this.gunPos.position - this.gunAssistant.position;
		float z = Mathf.Clamp(Vector2.Angle(from, new Vector2((float)this._eAttr.faceDir, 0f)), 0f, 90f);
		transform.localRotation = Quaternion.Euler(new Vector3(0f, (float)((this._eAttr.faceDir != 1) ? 180 : 0), z));
	}

	public IEnumerator TargetingPlayer()
	{
		float angle = Vector2.Angle(this.player.position - base.transform.position, Vector2.up);
		if (angle >= 30f)
		{
			this.gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
			Vector3 startEuler = this.gun.localEulerAngles;
			float targetAngle = Mathf.Clamp(angle + 90f, 120f, 180f);
			for (int i = 0; i < 30; i++)
			{
				this.gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, targetAngle), (float)i / 29f);
				yield return null;
			}
		}
		yield break;
	}

	public IEnumerator TargetRecover()
	{
		this.gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
		Vector3 startEuler = this.gun.localEulerAngles;
		for (int i = 0; i < 30; i++)
		{
			this.gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, 180f), (float)i / 29f);
			yield return null;
		}
		yield break;
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public float maxFlyHeight;

	private BladeGunAction _action;

	private EnemyAttribute _eAttr;

	private JsonData _jsonData;

	[SerializeField]
	private Transform bullet;

	[SerializeField]
	private Transform gunPos;

	[SerializeField]
	private Transform gunAssistant;

	[SerializeField]
	private Transform gun;

	private EnemyAtk _enemyAtk;
}
