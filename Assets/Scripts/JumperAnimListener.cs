using System;
using DatabaseModel;
using GameWorld;
using LitJson;
using UnityEngine;

public class JumperAnimListener : BaseBehaviour
{
	private void Start()
	{
		this._eAction = base.GetComponent<JumperAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
		this._atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.跳跃者];
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
				this._eAction.AnimChangeState(JumperAction.StateEnum.FlyToFall, 1f);
			}
		}
		if (!this._eAttr.checkHitGround)
		{
			return;
		}
		if (!this._eAttr.isOnGround)
		{
			return;
		}
		this._eAttr.checkHitGround = false;
		if (this._eAction.stateMachine.currentState == "HitFall")
		{
			this.MaxFlyHeight = 4f;
			this._eAttr.timeController.SetSpeed(Vector2.up * 25f);
			this._eAction.AnimChangeState(JumperAction.StateEnum.HitToFly, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(JumperAction.StateEnum.HitGround, 1f);
		}
	}

	public void ChangeState(JumperAction.StateEnum sta)
	{
		this._eAction.AnimChangeState(sta, 1f);
	}

	public void StartPabody()
	{
		this._eAttr.paBody = true;
	}

	public void EndPabody()
	{
		this._eAttr.paBody = false;
	}

	public void Atk3End()
	{
		JumperAction.StateEnum stateEnum = (!this._eAction.catchFollow) ? JumperAction.StateEnum.Atk3Fail : JumperAction.StateEnum.Atk3Success;
		if (!this._eAction.catchFollow)
		{
			this.EndPabody();
		}
		this._eAction.AnimChangeState(stateEnum, 1f);
	}

	public void Atk3Success()
	{
		this._eAction.catchFollow = false;
		this.EndPabody();
		R.Player.Transform.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._atkData["Atk3Success"], true);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._atkData[this._eAction.stateMachine.currentState];
	}

	public void SetAtkId()
	{
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayMoveSound()
	{
		R.Audio.PlayEffect(this.moveSound[UnityEngine.Random.Range(0, this.moveSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayHitGroundSound()
	{
		R.Audio.PlayEffect(this.hitGroundSound[UnityEngine.Random.Range(0, this.hitGroundSound.Length)], new Vector3?(base.transform.position));
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
		if (this._eAttr.isDead)
		{
			this.DestroySelf();
		}
		else
		{
			this._eAction.AnimChangeState(JumperAction.StateEnum.GetUp, 1f);
		}
	}

	public void CameraShake(int id)
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(CameraEffectProxyPrefabData.GetPrefab(id));
	}

	public void BackToIdle(JumperAction.StateEnum sta)
	{
		if (this._eAction.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
		}
		this._eAction.AnimChangeState(sta, 1f);
	}

	public void DestroySelf()
	{
		this.RealDestroy();
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlayHitGroundEffect()
	{
		R.Effect.Generate(40, base.transform, Vector3.zero, new Vector3(0f, (float)(90 * this._eAttr.faceDir), 0f), default(Vector3), true);
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void HitGroundShake()
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(CameraEffectProxyPrefabData.GetPrefab(13));
	}

	private JumperAction _eAction;

	private EnemyAttribute _eAttr;

	public float MaxFlyHeight;

	private JsonData _atkData;

	private EnemyAtk _enemyAtk;

	[SerializeField]
	private int[] moveSound;

	[SerializeField]
	private int[] hitGroundSound;
}
