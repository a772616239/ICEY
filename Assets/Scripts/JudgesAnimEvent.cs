using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class JudgesAnimEvent : BaseBehaviour
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
		this._action = base.GetComponent<JudgesAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._spineAnim = base.GetComponent<SpineAnimationController>();
		this._baseHurt = base.GetComponent<EnemyBaseHurt>();
		this._animEventListener = this.player.GetComponent<PlayerAnimEventListener>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.犹大];
	}

	public void ChangeState(JudgesAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		EnemyAtk[] componentsInChildren = base.GetComponentsInChildren<EnemyAtk>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].atkData = this._jsonData[this._action.stateMachine.currentState];
			componentsInChildren[i].atkId = Incrementor.GetNextId();
		}
	}

	public void SetAtkId()
	{
		EnemyAtk[] componentsInChildren = base.GetComponentsInChildren<EnemyAtk>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].atkId = Incrementor.GetNextId();
		}
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void Atk1Summon()
	{
		if (R.GameData.Difficulty <= 1)
		{
			this.SummonInEasyMode();
		}
		else
		{
			this.SummonInHardMode();
		}
	}

	private void SummonInEasyMode()
	{
		if (R.Enemy.GetEnemyCountByType(EnemyType.大脚) < 1)
		{
			this.SummonEnemy(EnemyType.大脚);
		}
		if (R.Enemy.GetEnemyCountByType(EnemyType.跳跃者) < 1)
		{
			this.SummonEnemy(EnemyType.跳跃者);
		}
	}

	private void SummonInHardMode()
	{
		if (R.Enemy.GetEnemyCountByType(EnemyType.跳拳大脚组合) < 1)
		{
			this.SummonEnemy(EnemyType.跳拳大脚组合);
		}
		int enemyCountByType = R.Enemy.GetEnemyCountByType(EnemyType.刀炮混合);
		if (enemyCountByType != 0)
		{
			if (enemyCountByType == 1)
			{
				this.SummonEnemy(EnemyType.刀炮混合);
			}
		}
		else
		{
			this.SummonEnemy(EnemyType.刀炮混合);
			this.SummonEnemy(EnemyType.刀炮混合);
		}
	}

	private void SummonEnemy(EnemyType type)
	{
		Vector2 value = new Vector2(UnityEngine.Random.Range(GameArea.EnemyRange.xMin + 3f, GameArea.EnemyRange.xMax - 3f), base.transform.position.y);
		GameObject gameObject = Singleton<EnemyGenerator>.Instance.GenerateEnemy(type, new Vector2?(value), true, true);
		gameObject.GetComponent<EnemyAttribute>().playerInView = true;
	}

	public void Atk2Effect()
	{
		Transform transform = R.Effect.Generate(211, null, this.thunderL.position, default(Vector3), default(Vector3), true);
		Transform transform2 = R.Effect.Generate(211, null, this.thunderR.position, default(Vector3), default(Vector3), true);
		transform2.localScale = new Vector3(-1f, 1f, 1f);
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData["Thunder"]);
		componentInChildren.origin = base.gameObject;
		EnemyBullet componentInChildren2 = transform2.GetComponentInChildren<EnemyBullet>();
		componentInChildren2.SetAtkData(this._jsonData["Thunder"]);
		componentInChildren2.origin = base.gameObject;
	}

	public void LaserAttack(int frame)
	{
		this.halo_L.SetActive(true);
		this.halo_R.SetActive(true);
		base.StartCoroutine(this.LaserRotate(this.eye_L_F, true, frame));
		base.StartCoroutine(this.LaserRotate(this.eye_R_F, false, frame));
	}

	private IEnumerator LaserRotate(Transform eye, bool left, int frame)
	{
		Vector2 dir = (!left) ? (this.eye_R_F.position - this.eye_R.position) : (this.eye_L_F.position - this.eye_L.position);
		Transform laser = R.Effect.Generate(205, eye, default(Vector3), default(Vector3), default(Vector3), true);
		laser.parent = eye;
		Laser i = laser.GetComponent<Laser>();
		i.AppearLaser(dir, base.gameObject, false);
		i.SetAtkData(this._jsonData["Laser"], this._eAttr.atk);
		yield return base.StartCoroutine(i.LaserFadeIn());
		for (int j = 0; j < frame; j++)
		{
			if (this._action.IsInWeakSta())
			{
				break;
			}
			i.Rotate(this.GetLaserDir(left));
			yield return null;
		}
		yield return base.StartCoroutine(i.LaserFadeOut());
		UnityEngine.Object.Destroy(laser.gameObject);
		this.DisappearHalo();
		yield break;
	}

	private Vector2 GetLaserDir(bool left)
	{
		return (!left) ? (this.eye_R_F.position - this.eye_R.position) : (this.eye_L_F.position - this.eye_L.position);
	}

	private void DisappearHalo()
	{
		this.halo_L.SetActive(false);
		this.halo_R.SetActive(false);
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 78; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(JudgesAction.StateEnum.WeakToIdle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void DestroySelf()
	{
		R.Enemy.RemoveEnemy(this._eAttr);
		base.Invoke("RealDestroy", 2f);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void QTEHurtEnd()
	{
		string currentState = this._action.stateMachine.currentState;
		if (currentState != null)
		{
			if (!(currentState == "HitQTE1"))
			{
				if (!(currentState == "HitQTE2"))
				{
					if (!(currentState == "HitQTE3"))
					{
						if (currentState == "HitQTE4")
						{
							UnityEngine.Object.Destroy(this.atkBoxes[3].gameObject);
							this._spineAnim.Skin = "default";
						}
					}
					else
					{
						UnityEngine.Object.Destroy(this.atkBoxes[2].gameObject);
						this._spineAnim.Skin = "Cut3";
					}
				}
				else
				{
					UnityEngine.Object.Destroy(this.atkBoxes[1].gameObject);
					this._spineAnim.Skin = "Cut2";
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.atkBoxes[0].gameObject);
				this._spineAnim.Skin = "Cut1";
			}
		}
		this.ChangeState(JudgesAction.StateEnum.Idle);
	}

	public void QTECameraZoomIn()
	{
		string currentState = this._action.stateMachine.currentState;
		if (currentState != null)
		{
			if (!(currentState == "HitQTE1"))
			{
				if (!(currentState == "HitQTE2"))
				{
					if (!(currentState == "HitQTE3"))
					{
						if (currentState == "HitQTE4")
						{
							R.Camera.Controller.CameraZoom(this.brokenPoints[3].position, 0.13333334f, 3f);
						}
					}
					else
					{
						R.Camera.Controller.CameraZoom(this.brokenPoints[2].position, 0.13333334f, 3f);
					}
				}
				else
				{
					R.Camera.Controller.CameraZoom(this.brokenPoints[1].position, 0.13333334f, 3f);
				}
			}
			else
			{
				R.Camera.Controller.CameraZoom(this.brokenPoints[0].position, 0.13333334f, 3f);
			}
		}
	}

	public void QTEShadeAtk()
	{
		string currentState = this._action.stateMachine.currentState;
		if (currentState != null)
		{
			if (!(currentState == "HitQTE1"))
			{
				if (!(currentState == "HitQTE2"))
				{
					if (!(currentState == "HitQTE3"))
					{
						if (currentState == "HitQTE4")
						{
							R.Effect.Generate(177, null, this.brokenPoints[3].position, default(Vector3), default(Vector3), true);
						}
					}
					else
					{
						R.Effect.Generate(177, null, this.brokenPoints[2].position, default(Vector3), default(Vector3), true);
					}
				}
				else
				{
					R.Effect.Generate(177, null, this.brokenPoints[1].position, default(Vector3), default(Vector3), true);
				}
			}
			else
			{
				R.Effect.Generate(177, null, this.brokenPoints[0].position, default(Vector3), default(Vector3), true);
			}
		}
	}

	public void QTEFinalHurt()
	{
		string currentState = this._action.stateMachine.currentState;
		if (currentState != null)
		{
			if (!(currentState == "HitQTE1"))
			{
				if (!(currentState == "HitQTE2"))
				{
					if (!(currentState == "HitQTE3"))
					{
						if (currentState == "HitQTE4")
						{
							R.Effect.Generate(156, null, this.brokenPoints[3].position, default(Vector3), default(Vector3), true);
							Vector3 position = this.brokenPoints[3].position;
							position.z = LayerManager.ZNum.Fx;
							Transform transform = UnityEngine.Object.Instantiate<Transform>(this.RArmPrefab, position, Quaternion.identity);
							transform.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 4f);
						}
					}
					else
					{
						R.Effect.Generate(156, null, this.brokenPoints[2].position, default(Vector3), default(Vector3), true);
						Vector3 position = this.brokenPoints[2].position;
						position.z = LayerManager.ZNum.Fx;
						Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.LArmPrefab, position, Quaternion.identity);
						transform2.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 4f);
					}
				}
				else
				{
					R.Effect.Generate(156, null, this.brokenPoints[1].position, default(Vector3), default(Vector3), true);
					Vector3 position = this.brokenPoints[1].position;
					position.z = LayerManager.ZNum.Fx;
					Transform transform = UnityEngine.Object.Instantiate<Transform>(this.RArmPrefab, position, Quaternion.identity);
					transform.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 6f);
				}
			}
			else
			{
				R.Effect.Generate(156, null, this.brokenPoints[0].position, default(Vector3), default(Vector3), true);
				Vector3 position = this.brokenPoints[0].position;
				position.z = LayerManager.ZNum.Fx;
				Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.LArmPrefab, position, Quaternion.identity);
				transform2.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 6f);
			}
		}
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public void QTEPlayerRecover()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.EndAtk, 1f);
		R.Camera.Controller.CameraZoomFinished();
	}

	public void PlayerChargeStart()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTECharge1Ready, 1f);
	}

	public void PlayerChargeSuccess()
	{
		this._animEventListener.chargeAnim.ChargeOneOver();
	}

	public void PlayerChargeRelease()
	{
		this._animEventListener.ChargeEffectDisappear();
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTECharge1End, 1f);
		R.Player.Action.pab.charge.ChargeReset();
	}

	public void PlayerChargeHurt()
	{
		R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
		R.Camera.Controller.CameraShake(0.5f, 0.4f, CameraController.ShakeTypeEnum.Rect, false);
		this._baseHurt.HitEffect(this.core.localPosition, "Atk1");
	}

	public void PlayerEndAtk()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEEndAtk, 1f);
	}

	public void PlayerPush()
	{
		this.player.position = base.transform.position + new Vector3((float)(-2 * R.Player.Attribute.faceDir), 3f, 0f);
		R.Player.Action.ChangeState(PlayerAction.StateEnum.AirQTEPush, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	private JudgesAction _action;

	private EnemyAttribute _eAttr;

	private SpineAnimationController _spineAnim;

	[SerializeField]
	private Transform[] atkBoxes;

	[SerializeField]
	private Transform[] brokenPoints;

	private JsonData _jsonData;

	[SerializeField]
	private Transform LArmPrefab;

	[SerializeField]
	private Transform RArmPrefab;

	[SerializeField]
	private Transform eye_L;

	[SerializeField]
	private Transform eye_L_F;

	[SerializeField]
	private Transform eye_R;

	[SerializeField]
	private Transform eye_R_F;

	[SerializeField]
	private Transform thunderL;

	[SerializeField]
	private Transform thunderR;

	[SerializeField]
	private Transform core;

	public GameObject halo_L;

	public GameObject halo_R;

	private EnemyBaseHurt _baseHurt;

	private PlayerAnimEventListener _animEventListener;
}
