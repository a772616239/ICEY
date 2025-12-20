using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using ExtensionMethods;
using GameWorld;
using LitJson;
using OnionSkin;
using UnityEngine;

public class PlayerAnimEventListener : BaseBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnPlayerDead;

	private void Awake()
	{
		this._box = base.GetComponent<BoxCollider2D>();
	}

	private void Start()
	{
		this.getUp = true;
		this.stateMachine = base.GetComponent<StateMachine>();
		this.pAction = base.GetComponent<PlayerAction>();
		this.pAttr = R.Player.Attribute;
		this.onion = base.GetComponent<OnionCreator>();
		this.atkData = JsonMapper.ToObject(this.atkParm.text);
		this._pAtk = this.atkBox.GetComponent<PlayerAtk>();
	}

	private void Update()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (this.checkFallDown && R.Player.TimeController.GetCurrentSpeed().y <= 3f)
		{
			this.checkFallDown = false;
			this.pAction.ChangeState(PlayerAction.StateEnum.Fall1, 1f);
			this.isFalling = true;
		}
		if (this.isFalling && this.pAttr.isOnGround)
		{
			this.isFalling = false;
			this.checkFallDown = false;
			this.PhysicReset();
			this.PlayerSound(21);
			this.pAction.ChangeState(PlayerAction.StateEnum.GetUp, 1f);
		}
		if (this.flyHitFlag && R.Player.TimeController.GetCurrentSpeed().y <= 0f && this.stateMachine.currentState.IsInArray(PlayerAction.HurtSta))
		{
			this.flyHitFlag = false;
			this.pAction.ChangeState(PlayerAction.StateEnum.UnderAtkFlyToFall, 1f);
			this.flyHitGround = true;
		}
		if (this.flyHitGround && this.pAttr.isOnGround)
		{
			this.hitJump = false;
			this.flyHitGround = false;
			this.PhysicReset();
			Vector2 currentSpeed = R.Player.TimeController.GetCurrentSpeed();
			currentSpeed.x /= 2f;
			R.Player.TimeController.SetSpeed(currentSpeed);
			this.pAction.ChangeState(PlayerAction.StateEnum.UnderAtkHitGround, 1f);
		}
		this.UpdateFlashEnd();
	}

	public void SetAtkData()
	{
		if (this.atkData.Contains(this.stateMachine.currentState))
		{
			this._pAtk.SetData(this.atkData[this.stateMachine.currentState], Incrementor.GetNextId());
		}
	}

	public void SetAttackId()
	{
		this._pAtk.attackId = Incrementor.GetNextId();
	}

	public void FallDown()
	{
		this.checkFallDown = true;
	}

	public void HitGroundCheck()
	{
		this.checkFallDown = false;
		this.isFalling = false;
		this.airAtkDown = false;
		this.checkHitGround = true;
	}

	public void Falling()
	{
		this.isFalling = true;
	}

	public void AirAtkDown()
	{
		this.isFalling = false;
		this.checkFallDown = false;
		this.checkHitGround = false;
		this.airAtkDown = true;
	}

	public void PlayAnim(PlayerAction.StateEnum sta)
	{
		this.pAction.ChangeState(sta, 1f);
	}

	public void CanChangeState()
	{
		this.pAction.canChangeAnim = true;
	}

	public void Speed(string speed)
	{
		JsonData jsonData = JsonMapper.ToObject(speed);
		float num = jsonData.Get<float>("x", 0f);
		float y = jsonData.Get<float>("y", 0f);
		Vector2 speed2 = new Vector2(base.transform.localScale.x * -num, y);
		R.Player.TimeController.SetSpeed(speed2);
	}

	public void PhysicReset()
	{
		R.Player.Rigidbody2D.WakeUp();
		R.Player.Rigidbody2D.gravityScale = 1f;
	}

	public void AirPhysic(float gravityScale)
	{
		R.Player.TimeController.SetSpeed(Vector2.zero);
		R.Player.Rigidbody2D.gravityScale = gravityScale;
	}

	public void MoveBoxSize(float xSize)
	{
		Vector2 offset = this._box.offset;
		offset.x = -xSize / 2f;
		this._box.offset = offset;
		Vector2 size = this._box.size;
		size.x = xSize;
		this._box.size = size;
	}

	public void BoxSizeRecover()
	{
		this._box.offset = new Vector2(0f, 1f);
		this._box.size = new Vector2(1f, 2f);
	}

	public IEnumerator FlashPositionSet()
	{
		float x = 0f;
		float y = 0f;
		PlayerAction.StateEnum nextSta = PlayerAction.StateEnum.Flash2;
		int num = this.flashDir;
		switch (num + 5)
		{
		case 0:
		case 1:
			x = 3.535f;
			y = -3.535f;
			break;
		case 3:
			y = -5f;
			nextSta = PlayerAction.StateEnum.FlashDown2;
			break;
		case 4:
		case 6:
			x = 5f;
			nextSta = PlayerAction.StateEnum.Flash2;
			break;
		case 7:
			y = 4f;
			nextSta = PlayerAction.StateEnum.FlashUp2;
			break;
		case 9:
		case 10:
			x = 3.535f;
			y = 3.535f;
			break;
		}
		Vector2 deltaPos = new Vector3(base.transform.localScale.x * -x, y);
		int clip = 8;
		for (int i = 0; i < clip; i++)
		{
			if (i == clip / 2)
			{
				float num2;
				if (deltaPos.x == 0f)
				{
					num2 = 1.57079637f * Mathf.Sign(deltaPos.y);
				}
				else
				{
					num2 = Mathf.Atan(deltaPos.y / deltaPos.x);
				}
				Vector3 position = Vector3.zero;
				if (y == 0f)
				{
					position = Vector3.up * 0.5f;
				}
				R.Effect.Generate(48, base.transform, position, new Vector3(0f, 0f, num2 * 180f / 3.14159274f), default(Vector3), true);
			}
			Vector3 nextPos = base.transform.position + new Vector3(deltaPos.x / 8f, deltaPos.y / 8f, 0f);
			R.Player.TimeController.NextPosition(nextPos);
			yield return new WaitForFixedUpdate();
		}
		this.atkBox.localScale = Vector3.zero;
		this.atkBox.localPosition = Vector3.zero;
		R.Player.Rigidbody2D.gravityScale = 1f;
		if (this.pAttr.isOnGround)
		{
			this.pAction.ChangeState(PlayerAction.StateEnum.FlashGround, 1f);
		}
		else
		{
			Vector2 speed = new Vector2((float)(5 * this.pAttr.faceDir), 10f);
			if (this.flashDir == 2 || this.flashDir == -2)
			{
				speed.x = 0f;
			}
			R.Player.TimeController.SetSpeed(speed);
			this.pAction.ChangeState(nextSta, 1.5f);
		}
		yield break;
	}

	public void PlayerSound(int audioIndex)
	{
		R.Audio.PlayEffect(audioIndex, new Vector3?(base.transform.position));
	}

	public void RandomPlaySound(int audioIndex)
	{
		if (UnityEngine.Random.Range(0, 100) > 60)
		{
			return;
		}
		this.PlayerSound(audioIndex);
	}

	public void PlayRunSound()
	{
		R.Audio.PlayEffect(this.runSound[UnityEngine.Random.Range(0, this.runSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayFlashSound()
	{
		R.Audio.PlayEffect(this.flashSound[UnityEngine.Random.Range(0, this.flashSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayShoutSoundLight()
	{
	}

	public void PlayShoutSoundHeavy()
	{
	}

	public void PlayHurtSoundLight()
	{
	}

	public void PlayHurtSoundHeavy()
	{
	}

	public void PlayAtkSound()
	{
		R.Audio.PlayEffect(this.atkSound[UnityEngine.Random.Range(0, this.atkSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayExecuteSound()
	{
	}

	public void FlyHit()
	{
		this.checkFallDown = false;
		this.isFalling = false;
		this.checkHitGround = false;
		this.flyHitFlag = true;
	}

	public void HitGetUp()
	{
		if (!this.pAttr.isDead && this.getUp)
		{
			this.getUp = true;
			this.pAction.ChangeState(PlayerAction.StateEnum.UnderAtkGetUp, 1f);
		}
	}

	public IEnumerator HitJumpBack()
	{
		yield return new WaitForSeconds(0.2f);
		this.hitJump = true;
		yield break;
	}

	public IEnumerator PlayerDieEnumerator()
	{
		R.Ui.Pause.Enabled = false;
		yield return new WaitForSeconds(2f);
		if (this.OnPlayerDead != null)
		{
			this.OnPlayerDead(this, null);
		}
		yield break;
	}

	public void PlayEffect(int effectId)
	{
		if (this.stateMachine.currentState != "QTECharge1End" && this.stateMachine.currentState != "Charge1End" && this.stateMachine.currentState != "AirChargeEnd")
		{
			R.Effect.Generate(effectId, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			return;
		}
		AtkEffector component = R.Effect.fxData[effectId].effect.GetComponent<AtkEffector>();
		Vector3 pos = component.pos;
		if (component.CanHitGround)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, -Vector2.up, 100f, LayerManager.GroundMask);
			pos = new Vector3(pos.x, pos.y - Mathf.Clamp(raycastHit2D.distance, 0f, float.PositiveInfinity), pos.z);
		}
		Transform transform = R.Effect.Generate(effectId, base.transform, pos, default(Vector3), default(Vector3), true);
		transform.localScale = new Vector3(transform.localScale.x * -Mathf.Sign(base.transform.localScale.x), transform.localScale.y, transform.localScale.z);
		if (component.UseAtkData)
		{
			if (this.charge)
			{
				this.charge = false;
				Vector3 position = Camera.main.transform.position.SetZ(-0.2f);
				transform.position = position;
				transform.GetComponent<AtkEffector>().SetData(this.atkData["Charge1EndLevel1"], Incrementor.GetNextId());
			}
			else
			{
				transform.GetComponent<AtkEffector>().SetData(this.atkData[this.stateMachine.currentState], Incrementor.GetNextId());
			}
		}
	}

	public void CloseEnemyEffect()
	{
		R.Effect.Generate(152, base.transform, Vector3.zero, default(Vector3), default(Vector3), true);
	}

	public void CloseEnemyBlast()
	{
		R.Effect.Generate(153, base.transform, new Vector3((float)this.pAttr.faceDir * 2.5f, 1f, 0f), default(Vector3), default(Vector3), true);
	}

	public void FlashStart()
	{
		this._flashEndCount = WorldTime.SecondToFrame(0.2f);
		this.pAttr.flashFlag = true;
	}

	private void UpdateFlashEnd()
	{
		if (this._flashEndCount > 0)
		{
			this._flashEndCount--;
			if (this._flashEndCount <= 0)
			{
				this.pAttr.flashFlag = false;
			}
		}
	}

	public void GetExecuteEnemy()
	{
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < this.executeEnemyList.Count; i++)
		{
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(this.executeEnemyList[i].gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.ExecuteFollow);
			EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", base.gameObject, args);
			zero.x = (base.transform.position.x + this.executeEnemyList[i].transform.position.x) / 2f;
		}
		zero.y = base.transform.position.y + 2f;
		R.Camera.Controller.CameraShake(0.25f, 0.6f, CameraController.ShakeTypeEnum.Rect, false);
		SingletonMono<CameraController>.Instance.OpenMotionBlur(0.13333334f, 1f, base.transform.position);
		SingletonMono<CameraController>.Instance.CameraZoom(new Vector3(zero.x, zero.y, Camera.main.transform.parent.position.z + 3f), 0.166666672f, 3f);
	}

	public void StartExecute()
	{
		for (int i = 0; i < this.executeEnemyList.Count; i++)
		{
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(this.executeEnemyList[i], EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, this.stateMachine.currentState);
			EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", base.gameObject, args);
		}
		this.executeEnemyList.Clear();
		this.pAction.QTEHPRecover(false);
	}

	public void ExecuteTimeSlow()
	{
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
	}

	public void Execute2_1Hit()
	{
		for (int i = 0; i < this.executeEnemyList.Count; i++)
		{
			EnemyBaseHurt component = this.executeEnemyList[i].GetComponent<EnemyBaseHurt>();
			component.StopFollowLeftHand();
			Vector3 position = this.executeEnemyList[i].transform.position;
			position.y = Mathf.Clamp(position.y + 1f, position.y, LayerManager.YNum.GetGroundHeight(this.executeEnemyList[i].gameObject) + 4f);
			this.executeEnemyList[i].transform.position = position;
			EnemyHurtAtkEventArgs.PlayerNormalAtkData attackData = new EnemyHurtAtkEventArgs.PlayerNormalAtkData(this.atkData[this.stateMachine.currentState], true)
			{
				camShakeFrame = 0,
				shakeStrength = 0f
			};
			Vector3 position2 = component.center.position;
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(this.executeEnemyList[i].gameObject, base.gameObject, Incrementor.GetNextId(), position2, HurtCheck.BodyType.Body, attackData, false);
			EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", base.gameObject, args);
			R.Camera.Controller.OpenMotionBlur(0.13333334f, 1f, base.transform.position);
			R.Camera.Controller.ZoomFinished();
			R.Camera.Controller.CameraMoveToBySpeed(Camera.main.transform.parent.position + Vector3.up * 4f, 20f, false, Ease.InOutExpo);
			R.Camera.Controller.CameraShake(0.266666681f, 0.4f, CameraController.ShakeTypeEnum.Horizon, false);
		}
	}

	public IEnumerator Execute2_1ChangeState()
	{
		GameObject enemy = this.executeEnemyList[0].gameObject;
		EnemyAttribute attr = enemy.GetComponent<EnemyAttribute>();
		while (attr.timeController.GetCurrentSpeed().y > 0f)
		{
			yield return null;
		}
		attr.timeController.SetSpeed(Vector2.zero);
		this.AirPhysic(0f);
		Vector3 pos = base.transform.position;
		pos.y = enemy.transform.position.y;
		base.transform.position = pos;
		this.pAction.ChangeState(PlayerAction.StateEnum.NewExecute2_2, 1f);
		yield break;
	}

	public void AirExecute1_2Hit()
	{
		for (int i = 0; i < this.executeEnemyList.Count; i++)
		{
			this.executeEnemyList[i].GetComponent<EnemyAttribute>().timeController.SetSpeed(Vector2.down * 60f);
			Vector3 position = Camera.main.transform.parent.position;
			position.y = LayerManager.YNum.GetGroundHeight(this.executeEnemyList[i]) + 2f;
			R.Camera.Controller.ZoomFinished();
			R.Camera.Controller.CameraMoveToBySpeed(position, 40f, false, Ease.InOutExpo);
			R.Camera.Controller.CameraShake(0.167f, 0.4f, CameraController.ShakeTypeEnum.Rect, true);
		}
	}

	public void QTECameraFinish()
	{
	}

	public void ChargeEffectAppear()
	{
		if (this.chargeAnim.gameObject.activeSelf)
		{
			return;
		}
		this.chargeAnim.gameObject.SetActive(true);
		this.chargeAnim.ChargeZeroToOne();
	}

	public void ChargeEffectDisappear()
	{
		this.chargeAnim.gameObject.SetActive(false);
	}

	public void ChargeEnemyFrozen()
	{
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(10, WorldTime.FrozenArgs.FrozenType.Enemy, true);
	}

	public void StopRoll()
	{
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = GameObject.Find("NewRoll(Clone)");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void StopIEnumerator(string name)
	{
		base.StopCoroutine(name);
	}

	public void OpenOnion()
	{
		this.onion.Open(true, 0.3f, this.onionObj);
	}

	public Transform atkBox;

	public bool checkFallDown;

	public bool isFalling;

	public bool airAtkDown;

	public int flashDir;

	private PlayerAction pAction;

	private PlayerAttribute pAttr;

	public bool checkHitGround;

	public bool flyHitFlag;

	public bool flyHitGround;

	public bool hitJump;

	private float hitJumpTime;

	[SerializeField]
	private TextAsset atkParm;

	private BoxCollider2D _box;

	public JsonData atkData;

	private OnionCreator onion;

	[SerializeField]
	private GameObject[] onionObj;

	private StateMachine stateMachine;

	public List<GameObject> executeEnemyList;

	public ChargeController chargeAnim;

	public bool getUp;

	public bool charge;

	private PlayerAtk _pAtk;

	private int _flashEndCount;

	[SerializeField]
	private int[] runSound;

	[SerializeField]
	private int[] flashSound;

	[SerializeField]
	private int[] hurtSoundLight;

	[SerializeField]
	private int[] hurtSoundHeavy;

	[SerializeField]
	private int[] atkSound;

	[SerializeField]
	private int[] executeSound;
}
