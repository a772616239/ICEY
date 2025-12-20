using System;
using System.Collections;
using System.Collections.Generic;
using GameWorld;
using LitJson;
using UnityEngine;

public class JackAnimEvent : BaseBehaviour
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
		this._action = base.GetComponent<JackAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this.jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.杰克];
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
				this._action.AnimChangeState(JackAction.StateEnum.FlyToFall, 1f);
			}
		}
		if (this._eAttr.checkHitGround && this._eAttr.isOnGround)
		{
			this._eAttr.checkHitGround = false;
			R.Effect.Generate(6, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			this._action.AnimChangeState(DaoAction.StateEnum.FallHitGround, 1f);
		}
	}

	public void ChangeState(JackAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void FlyUp()
	{
		this._eAttr.isFlyingUp = true;
	}

	public void CheckHitGround()
	{
		this._eAttr.checkHitGround = true;
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this.jsonData[this._action.stateMachine.currentState];
		this.SetAtkID();
	}

	public void SetGivenAtkData(string state)
	{
		this._enemyAtk.atkData = this.jsonData[state];
		this.SetAtkID();
	}

	public void SetAtkID()
	{
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void LunchBulletLeft(float angle)
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.bulletPrefab, this.leftGun.position, Quaternion.identity);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, (float)this._eAttr.faceDir * (angle - 90f));
		Rigidbody2D component = transform.GetComponent<Rigidbody2D>();
		Vector2 vector = new Vector2((float)this._eAttr.faceDir * Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
		component.velocity = vector.normalized * 15f;
		EnemyBullet component2 = transform.GetComponent<EnemyBullet>();
		component2.damage = this._eAttr.atk;
		component2.origin = base.gameObject;
		component2.SetAtkData(this.jsonData["Atk1"]);
	}

	public void LunchBulletRight(float angle)
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.bulletPrefab, this.rightGun.position, Quaternion.identity);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, (float)this._eAttr.faceDir * (angle - 90f));
		Rigidbody2D component = transform.GetComponent<Rigidbody2D>();
		Vector2 vector = new Vector2((float)this._eAttr.faceDir * Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
		component.velocity = vector.normalized * 15f;
		EnemyBullet component2 = transform.GetComponent<EnemyBullet>();
		component2.damage = this._eAttr.atk;
		component2.origin = base.gameObject;
		component2.SetAtkData(this.jsonData["Atk1"]);
	}

	public void PlayAimEffect()
	{
		base.StartCoroutine(this.Aim());
	}

	public IEnumerator Aim()
	{
		this._realList = new List<Transform>();
		for (int j = 0; j < 3; j++)
		{
			this._realList.Add(UnityEngine.Object.Instantiate<Transform>(this.aimReal));
			this._realList[j].GetComponent<JackAimReal>().Jack = base.transform;
			this._realList[j].gameObject.SetActive(false);
		}
		for (int i = 0; i < 7; i++)
		{
			Transform effect = R.Effect.Generate(200, null, (this.player.position + base.transform.position) / 2f + new Vector3(UnityEngine.Random.Range(-7f, 5f), UnityEngine.Random.Range(-0.5f, 4f), -0.01f), default(Vector3), default(Vector3), true);
			effect.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "ShowDisappear", false);
			R.Audio.PlayEffect(320, new Vector3?(effect.position));
			if (i == 2)
			{
				base.StartCoroutine(this.AimReal());
			}
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}

	public IEnumerator AimReal()
	{
		for (int i = 0; i < 3; i++)
		{
			this._realList[i].transform.position = this.player.position + Vector3.up;
			R.Audio.PlayEffect(320, new Vector3?(this._realList[i].transform.position));
			this._realList[i].gameObject.SetActive(true);
			this._realList[i].GetComponent<Animation>().Play("ShowShootAppear");
			yield return new WaitForSeconds(0.35f);
		}
		yield break;
	}

	public void SpwanEffect(string anim)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.jackEffect, base.transform.position + Vector3.back * 0.01f, Quaternion.identity);
		gameObject.GetComponent<SkeletonAnimation>().state.SetAnimation(0, anim, false);
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.AddComponent<AutoDestroy>();
		UnityEngine.Object.Destroy(gameObject.GetComponent<EnemyEffectSync>());
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(JackAction.StateEnum.IdleToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(JackAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 65; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState("WeakModToIdle", 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void DestroySelf()
	{
		this.RealDestroy();
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void QTEHurtPlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush, 1f);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public void QTEHurtEffect()
	{
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
		R.Camera.Controller.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Audio.PlayEffect(153, new Vector3?(base.transform.position));
	}

	public void ExecutePlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.BeelzebubQTEDie, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public float maxFlyHeight;

	private JackAction _action;

	private EnemyAttribute _eAttr;

	public Transform aimReal;

	public JsonData jsonData;

	private List<Transform> _realList;

	[SerializeField]
	private Transform leftGun;

	[SerializeField]
	private Transform rightGun;

	[SerializeField]
	private Transform bulletPrefab;

	[SerializeField]
	private GameObject jackEffect;

	private EnemyAtk _enemyAtk;
}
