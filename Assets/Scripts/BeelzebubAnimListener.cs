using System;
using System.Collections;
using DatabaseModel;
using GameWorld;
using LitJson;
using UnityEngine;

public class BeelzebubAnimListener : BaseBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
		this._eAction = base.GetComponent<BeelzebubAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._atk = base.GetComponentInChildren<EnemyAtk>();
		this._atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.暴食];
		this._atk2HitLoopTime = 1;
		this.Atk2MoveLoopTime = 2;
	}

	public void ChangeState(BeelzebubAction.StateEnum sta)
	{
		this._eAction.AnimChangeState(sta, 1f);
	}

	public void EatAtkEnd()
	{
		BeelzebubAction.StateEnum stateEnum = (!this._eAction.EatSuccess) ? BeelzebubAction.StateEnum.Atk1Fail : BeelzebubAction.StateEnum.Atk1Success;
		this._eAction.AnimChangeState(stateEnum, 1f);
	}

	public void EatAtkSuccess()
	{
		this._eAction.EatSuccess = false;
		this.player.GetComponent<Collider2D>().enabled = false;
		this.player.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(this.player.gameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._atkData["Atk1Release"], true);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	public void SawAttackLoopEnd()
	{
		this._atk2HitLoopTime--;
		if (this._atk2HitLoopTime >= 0)
		{
			return;
		}
		this._atk2HitLoopTime = 1;
		this.SawAttackFinish();
	}

	public void SawAttackMoveLoopEnd()
	{
		this.Atk2MoveLoopTime--;
		if (this.Atk2MoveLoopTime < 0)
		{
			this.Atk2MoveLoopTime = 2;
			this.ChangeState(BeelzebubAction.StateEnum.Atk2End);
		}
		else
		{
			base.GetComponent<AnimMoveController>().AnimMoveLoopReset();
		}
	}

	public void SawAttackFinish()
	{
		this._eAction.SawSuccess = false;
		this.player.GetComponent<Collider2D>().enabled = false;
		this.player.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(this.player.gameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._atkData["Atk2Release"], true);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
		this._eAction.AnimChangeState(BeelzebubAction.StateEnum.Atk2End, 1f);
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

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayMoveSound()
	{
		R.Audio.PlayEffect(this.moveSound[UnityEngine.Random.Range(0, this.moveSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayEatAudio()
	{
		if (this._eAction.EatSuccess)
		{
			R.Audio.PlayEffect(84, new Vector3?(base.transform.position));
		}
	}

	public void Atk3Effect()
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(6);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, base.transform.position, Quaternion.identity);
		gameObject.transform.parent = base.transform;
	}

	public void AngeyEffect()
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		R.Camera.Controller.OpenMotionBlur(1.16666663f, 0.1f, base.transform.position + Vector3.up * 2f);
		R.Camera.Controller.CameraShake(1.16666663f, 0.1f, CameraController.ShakeTypeEnum.Rect, false);
		this.ShoutEffect();
	}

	private void ShoutEffect()
	{
		R.Effect.Generate(17, null, base.transform.Find("Center").position, default(Vector3), default(Vector3), true);
	}

	public void PlayAirDistort()
	{
		R.Effect.Generate(49, base.transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
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

	public void BackToIdle()
	{
		if (this._eAction.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._eAction.AnimChangeState(BeelzebubAction.StateEnum.WeakMod, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(BeelzebubAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 96; i++)
		{
			if (this._eAction.stateMachine.currentState == "WeakMod" && !this._eAction.IsInWeakSta())
			{
				this._eAction.AnimChangeState(BeelzebubAction.StateEnum.Idle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void PlayATK3Effect()
	{
		R.Effect.Generate(133, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(145, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
	}

	public void PlayATK1Effect()
	{
		R.Effect.Generate(134, base.transform, new Vector3(-0.4f, 1.65f, 0f), default(Vector3), default(Vector3), true);
	}

	public void Atk2Effect()
	{
		this.LSawUpper2.gameObject.SetActive(true);
	}

	public void StopAtk2Effect()
	{
		this.LSawUpper2.gameObject.SetActive(false);
	}

	public void PlayATK12Effect()
	{
		R.Effect.Generate(139, base.transform, new Vector3(-1f, 2f, 0f), default(Vector3), default(Vector3), true);
	}

	public void PlayATK2Effect()
	{
		this.BeelzebubATK2.gameObject.SetActive(true);
		this.BeelzebubATK2.rotation = Quaternion.Euler(new Vector3(0f, (float)((base.transform.localScale.x <= 0f) ? 180 : 0), 0f));
	}

	public void QTECameraZoomIn()
	{
		SingletonMono<CameraController>.Instance.CameraZoom(base.transform.position + Vector3.up * 3f, 0.166666672f, 3f);
		SingletonMono<CameraController>.Instance.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		SingletonMono<CameraController>.Instance.OpenMotionBlur(0.06666667f, 0.1f, base.transform.position + Vector3.up * 3f);
	}

	public void QTEPlayerAppear()
	{
		this.player.position = base.transform.position + Vector3.up * 8f;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.DahalRoll, 1f);
	}

	public void QTEShadeAtk()
	{
		this.QTENormalHurt();
		SingletonMono<CameraController>.Instance.CameraShake(0.166666672f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void QTENormalHurt()
	{
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public void ExecutePlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	private BeelzebubAction _eAction;

	private EnemyAttribute _eAttr;

	private EnemyAtk _atk;

	private JsonData _atkData;

	private int _atk2HitLoopTime;

	public int Atk2MoveLoopTime;

	[SerializeField]
	public Transform LSawUpper2;

	[SerializeField]
	public Transform BeelzebubATK2;

	[SerializeField]
	private int[] moveSound;
}
