using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using LitJson;
using OnionSkin;
using UnityEngine;

public class DahalAnimListener : BaseBehaviour
{
	private PlayerManager player
	{
		get
		{
			return R.Player;
		}
	}

	private void Start()
	{
		this._eAction = base.GetComponent<DahalAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._atk = base.GetComponentInChildren<EnemyAtk>();
		this._atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.达哈尔];
		this._onion = base.GetComponent<OnionCreator>();
		this._animMoveController = base.GetComponent<AnimMoveController>();
		this._baseHurt = base.GetComponent<EnemyBaseHurt>();
	}

	private void Update()
	{
		if (this._eAttr.isDead)
		{
			return;
		}
		if (this._eAttr.isFlyingUp)
		{
			if (this.MaxFlyHeight > 0f && this._eAttr.height >= this.MaxFlyHeight)
			{
				Vector2 currentSpeed = this._eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				this._eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (this._eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				this._eAttr.isFlyingUp = false;
				this._eAction.AnimChangeState("HitToFly3", 1f);
			}
		}
		if (this._eAttr.checkHitGround && this._eAttr.isOnGround)
		{
			this._eAttr.checkHitGround = false;
			R.Effect.Generate(6, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			this._eAction.AnimChangeState("HitGround", 1f);
		}
		if (this._pushDown && this._eAttr.isOnGround)
		{
			this._pushDown = false;
			this._eAttr.timeController.SetGravity(1f);
			Vector2 currentSpeed2 = this._eAttr.timeController.GetCurrentSpeed();
			currentSpeed2.x = (float)(5 * this._eAttr.faceDir);
			this._eAttr.timeController.SetSpeed(currentSpeed2);
			this._eAction.AnimChangeState(DahalAction.StateEnum.AirPushEnd, 1f);
		}
	}

	public void ChangeState_Anim(DahalAction.StateEnum sta)
	{
		this._eAction.AnimChangeState(sta, 1f);
	}

	public void HitGround()
	{
		this._eAttr.checkHitGround = true;
		this._pushDown = false;
	}

	public void FlyUp()
	{
		this._eAttr.isFlyingUp = true;
		this._pushDown = false;
	}

	public void GetUp()
	{
		this._eAttr.currentActionInterruptPoint = 0;
		this.BackToIdle();
	}

	public void RollOver()
	{
		this._animMoveController.AnimMoveLoopReset();
	}

	public void Atk1Over(int move)
	{
		if (move != 1)
		{
			if (move != 2)
			{
				if (move == 3)
				{
					this.GravityScale(1f);
					this.ChangeState_Anim(DahalAction.StateEnum.Atk1End1);
				}
			}
			else
			{
				this.ChangeState_Anim(DahalAction.StateEnum.Atk1Move3);
			}
		}
		else
		{
			this.ChangeState_Anim(DahalAction.StateEnum.Atk1Move2);
		}
	}

	public void Speed(string speed)
	{
		JsonData jsonData = JsonMapper.ToObject(speed);
		float num = jsonData.Get<float>("x", 0f);
		float y = jsonData.Get<float>("y", 0f);
		this._eAttr.timeController.SetSpeed(new Vector2(num * (float)this._eAttr.faceDir, y));
	}

	public void StartPabody()
	{
		this._eAttr.paBody = true;
	}

	public void EndPabody()
	{
		this._eAttr.paBody = false;
	}

	public void SetAtkData()
	{
		this._atk.atkData = this._atkData[this._eAction.stateMachine.currentState];
	}

	public void SetAtkId()
	{
		this._atk.atkId = Incrementor.GetNextId();
	}

	public void GravityScale(float scale)
	{
		this._eAttr.timeController.SetGravity(scale);
	}

	public void PushDown()
	{
		this._eAttr.isFlyingUp = false;
		this._eAttr.checkHitGround = false;
		this._pushDown = true;
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayAtk2Sound()
	{
		R.Audio.PlayEffect(this.atk2Sound[UnityEngine.Random.Range(0, this.atk2Sound.Length)], new Vector3?(base.transform.position));
	}

	public void OpenOnion()
	{
		this._onion.Open(true, 0.7f, this.onionObj);
	}

	public void DestroySelf()
	{
		this.SparkL.gameObject.SetActive(false);
		this.SparkR.gameObject.SetActive(false);
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void DieBlock()
	{
		R.Effect.Generate(92, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void BackToIdle()
	{
		if (this._eAction.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._eAction.AnimChangeState(DahalAction.StateEnum.WeakMod, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(DahalAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 96; i++)
		{
			if (this._eAction.stateMachine.currentState == "WeakMod" && !this._eAction.IsInWeakSta())
			{
				this._eAction.AnimChangeState(DahalAction.StateEnum.Idle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void Shoot(int id = 0)
	{
		this.SparkL.GetComponent<SpriteRenderer>().enabled = true;
		this.SparkR.GetComponent<SpriteRenderer>().enabled = true;
		Vector2 from = (this.SparkL.position - this.SparkLGun.position).normalized;
		Vector2 from2 = (this.SparkR.position - this.SparkRGun.position).normalized;
		if (id == 1)
		{
			Transform transform = R.Effect.Generate(131, this.SparkL, new Vector3(0f, UnityEngine.Random.Range(-0.05f, 0.25f), 0f), default(Vector3), default(Vector3), true);
			Transform transform2 = R.Effect.Generate(131, this.SparkR, new Vector3(0f, UnityEngine.Random.Range(-0.05f, 0.25f), 0f), default(Vector3), default(Vector3), true);
			EnemyBulletLaucher component = transform.GetComponent<EnemyBulletLaucher>();
			component.SetVelocity(20f, Mathf.Sign(from.x) * Vector2.Angle(from, Vector2.up));
			component.damage = this._eAttr.atk;
			component.attacker = base.transform;
			component.SetAtkData(this._atkData[this._eAction.stateMachine.currentState]);
			EnemyBulletLaucher component2 = transform2.GetComponent<EnemyBulletLaucher>();
			component2.SetVelocity(20f, Mathf.Sign(from2.x) * Vector2.Angle(from2, Vector2.up));
			component2.damage = this._eAttr.atk;
			component2.attacker = base.transform;
			component2.SetAtkData(this._atkData[this._eAction.stateMachine.currentState]);
		}
		else
		{
			Transform transform3 = R.Effect.Generate(131, this.SparkL, new Vector3(0f, UnityEngine.Random.Range(-0.2f, 0.15f), 0f), default(Vector3), default(Vector3), true);
			Transform transform4 = R.Effect.Generate(131, this.SparkR, new Vector3(0f, UnityEngine.Random.Range(-0.2f, 0.15f), 0f), default(Vector3), default(Vector3), true);
			EnemyBulletLaucher component3 = transform3.GetComponent<EnemyBulletLaucher>();
			component3.SetVelocity(20f, Mathf.Sign(from.x) * Vector2.Angle(from, Vector2.up));
			component3.damage = this._eAttr.atk;
			component3.attacker = base.transform;
			component3.SetAtkData(this._atkData[this._eAction.stateMachine.currentState]);
			EnemyBulletLaucher component4 = transform4.GetComponent<EnemyBulletLaucher>();
			component4.SetVelocity(20f, Mathf.Sign(from2.x) * Vector2.Angle(from2, Vector2.up));
			component4.damage = this._eAttr.atk;
			component4.attacker = base.transform;
			component4.SetAtkData(this._atkData[this._eAction.stateMachine.currentState]);
		}
		base.Invoke("StopShoot", 0.28f);
	}

	public void StopShoot()
	{
		this.SparkL.GetComponent<SpriteRenderer>().enabled = false;
		this.SparkR.GetComponent<SpriteRenderer>().enabled = false;
	}

	public void ShowFireEffect()
	{
		this.DahalAtk4Effect.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "Fire", false);
	}

	public IEnumerator ShowFireBullet()
	{
		this._whitePoint = GameObject.Find("WhitePoint").transform.position;
		Transform fireBullet = R.Effect.Generate(148, base.transform, this.SparkR.localPosition, default(Vector3), default(Vector3), true);
		while (Vector3.Distance(fireBullet.position, this._whitePoint) > 0.1f)
		{
			fireBullet.position = Vector3.MoveTowards(fireBullet.position, this._whitePoint, 1f);
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(2f);
		UnityEngine.Object.Destroy(fireBullet.gameObject);
		yield break;
	}

	public void ShowATK6FallSpark()
	{
		R.Effect.Generate(149, base.transform, new Vector3(0.35f, 0.05f, 0f), default(Vector3), default(Vector3), true);
	}

	public void ShowATK6Effect()
	{
		this.DahalAtk6Effect.rotation = Quaternion.Euler(0f, (float)((base.transform.localScale.x >= 0f) ? 0 : 180), 0f);
		this.DahalAtk6Effect.GetChild(0).localPosition = new Vector3((base.transform.localScale.x >= 0f) ? 0.45f : -0.45f, 0f, 0f);
		this.DahalAtk6Effect.gameObject.SetActive(true);
	}

	public void StopATK6Effect()
	{
		this.DahalAtk6Effect.gameObject.SetActive(false);
	}

	public void ShowChangeStateEffect(int id = 0)
	{
		if (id == 0)
		{
			R.Effect.Generate(150, this.LBlade, Vector3.zero, new Vector3(0f, 0f, (float)((base.transform.localScale.x >= 0f) ? 0 : 90)), default(Vector3), true);
			R.Effect.Generate(150, this.RBlade, Vector3.zero, new Vector3(0f, 0f, (float)((base.transform.localScale.x >= 0f) ? 0 : 90)), default(Vector3), true);
		}
	}

	public void DahalAngryEffect()
	{
		R.Camera.Controller.OpenMotionBlur(1.2f, 0.06f, this._baseHurt.center.position);
		base.StartCoroutine(this.PlayDahalHaloEffect());
	}

	public IEnumerator PlayDahalHaloEffect()
	{
		for (int i = 0; i < 4; i++)
		{
			R.Effect.Generate(17, null, this._baseHurt.center.position, default(Vector3), default(Vector3), true);
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	public void QTEHurtPlayerApppear()
	{
		this.player.Transform.position = base.transform.position;
		this.player.Action.ChangeState(PlayerAction.StateEnum.RollEnd, 1f);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.GameObject, args);
	}

	public void ExecuteCameraMoveUp()
	{
		R.Camera.Controller.CameraMoveToBySpeed(base.transform.position + Vector3.up * 9f, 25f, false, Ease.Linear);
		R.Camera.Controller.CameraShake(0.166666672f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		this.ExecuteAirHurt();
	}

	public void ExecuteCameraMoveDown()
	{
		R.Camera.Controller.CameraShake(0.166666672f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this._baseHurt.HitEffect(this._baseHurt.center.localPosition, "Atk1");
		R.Camera.Controller.CameraMoveToBySpeed(base.transform.position - Vector3.up * 9f, 25f, false, Ease.Linear);
	}

	public void ExecuteShadeAtk(int dir)
	{
		Transform transform = R.Effect.Generate(177, null, this._baseHurt.center.position + Vector3.down, default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x = (float)dir;
		transform.localScale = localScale;
		R.Camera.Controller.CameraShake(0.166666672f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void ExecutePlayerRoll()
	{
		this.player.Transform.position = base.transform.position + Vector3.up * 16f;
		this.player.Action.ChangeState(PlayerAction.StateEnum.DahalRoll, 1f);
	}

	public void ExecuteCameraShake()
	{
		R.Camera.Controller.CameraShake(0.166666672f, 0.3f, CameraController.ShakeTypeEnum.Rect, true);
	}

	public void ExecuteAirHurt()
	{
		this._baseHurt.HitEffect(this._baseHurt.center.localPosition, "Atk1");
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.GameObject, args);
	}

	public void ExecuteCameraRecover()
	{
		R.Camera.Controller.CameraZoomFinished();
	}

	public float MaxFlyHeight;

	private DahalAction _eAction;

	private EnemyAttribute _eAttr;

	private EnemyAtk _atk;

	[SerializeField]
	private TextAsset atkParm;

	private JsonData _atkData;

	private Vector3 _whitePoint;

	private bool _pushDown;

	private EnemyBaseHurt _baseHurt;

	[SerializeField]
	private Transform SparkL;

	[SerializeField]
	private Transform SparkR;

	[SerializeField]
	private Transform SparkLGun;

	[SerializeField]
	private Transform SparkRGun;

	[SerializeField]
	private Transform DahalAtk4Effect;

	[SerializeField]
	private Transform DahalAtk6Effect;

	[SerializeField]
	private Transform LBlade;

	[SerializeField]
	private Transform RBlade;

	private OnionCreator _onion;

	[SerializeField]
	private GameObject[] onionObj;

	private AnimMoveController _animMoveController;

	[SerializeField]
	private int[] atk2Sound;
}
