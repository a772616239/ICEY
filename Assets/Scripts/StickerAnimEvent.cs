using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class StickerAnimEvent : BaseBehaviour
{
	private GameObject player
	{
		get
		{
			return R.Player.GameObject;
		}
	}

	private void Awake()
	{
		this._action = base.GetComponent<StickerAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.巨柱];
	}

	public void ChangeState(StickerAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[base.GetComponent<StateMachine>().currentState];
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void CameraShake(int frame)
	{
		R.Camera.Controller.CameraShake((float)frame / 60f, 0.2f, CameraController.ShakeTypeEnum.Vertical, false);
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(StickerAction.StateEnum.IdleToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(StickerAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 115; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(StickerAction.StateEnum.WeakModToIdle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void DestroySelf()
	{
		R.Enemy.RemoveEnemy(this._eAttr);
		base.gameObject.SetActive(false);
		base.Invoke("DestroyGameObject", Time.deltaTime);
	}

	private void DestroyGameObject()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void QTEHurtShadeAtk()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position + Vector3.up * 2f, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)this._eAttr.faceDir;
		transform.localScale = one;
	}

	public void QTEHurtShadeAtkBack()
	{
		Transform transform = R.Effect.Generate(180, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)this._eAttr.faceDir;
		transform.localScale = one;
	}

	public void QTEHitEffect()
	{
		R.Audio.PlayEffect(258, new Vector3?(base.transform.position));
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
		R.Camera.Controller.CameraShake(0.166666672f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void PlayerRollAttack()
	{
		this.player.transform.position = base.transform.position + Vector3.up * 6f;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTERoll, 1f);
		this.player.GetComponent<PlayerTimeController>().SetSpeed(Vector2.left * (float)this._eAttr.faceDir * 3f);
		R.Player.Rigidbody2D.gravityScale = 0.25f;
	}

	public void PlayerRollAttackEffect()
	{
		R.Audio.PlayEffect(258, new Vector3?(base.transform.position));
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(this.player.transform.position - base.transform.position, "Atk1");
		R.Camera.Controller.CameraShake(0.06666667f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player, args);
	}

	public void PlayerBackToIdle()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.EndAtk, 0.5f);
	}

	public void QTEDieShadeAtk()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position + Vector3.up * 1.5f, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)(-(float)this._eAttr.faceDir);
		transform.localScale = one;
	}

	public void WeaponFly()
	{
		R.Audio.PlayEffect(258, new Vector3?(base.transform.position));
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.weaponPrefab, this.rightHand.position, Quaternion.identity);
		transform.GetComponent<EnemyArm>().SetAngularSpeed((float)(-30 * this._eAttr.faceDir));
	}

	public void HeadFly()
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.headPrefab, this.headPos.position, Quaternion.identity);
		transform.GetComponent<Rigidbody2D>().velocity = new Vector2((float)(-3 * this._eAttr.faceDir), 20f);
		transform.GetComponent<EnemyArm>().SetAngularSpeed((float)(50 * this._eAttr.faceDir));
	}

	public void CameraZoomIn()
	{
		R.Camera.Controller.CameraZoom(this.headPos.position, 0.166666672f, 3f);
	}

	public void PlayerRush()
	{
		this.player.transform.position += Vector3.up * 2.5f;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.AirQTEPush, 1f);
	}

	public void Cameraback()
	{
		R.Camera.Controller.ZoomFinished();
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
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

	public void PlayATK2Effect()
	{
		Transform transform = R.Effect.Generate(209, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x *= Mathf.Sign(base.transform.localScale.x);
		transform.localScale = localScale;
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData["Atk2"]);
		componentInChildren.origin = base.gameObject;
		componentInChildren.damage = this._eAttr.atk;
	}

	private StickerAction _action;

	private EnemyAttribute _eAttr;

	private EnemyAtk _enemyAtk;

	private JsonData _jsonData;

	[SerializeField]
	private Transform rightHand;

	[SerializeField]
	private Transform headPos;

	[SerializeField]
	private Transform weaponPrefab;

	[SerializeField]
	private Transform headPrefab;

	[SerializeField]
	private Transform weaponPoint;

	[SerializeField]
	private int[] moveAudio;
}
