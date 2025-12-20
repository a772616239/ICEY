using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class RiderAnimListener : BaseBehaviour
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
		this._eAction = base.GetComponent<RiderAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._atk = base.GetComponentInChildren<EnemyAtk>();
		this._atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.骑兵];
		this._atk2LoopTimes = 2;
	}

	public void ChangeState(RiderAction.StateEnum sta)
	{
		this._eAction.AnimChangeState(sta, 1f);
	}

	public void Atk2End()
	{
		this._atk2LoopTimes--;
		if (this._atk2LoopTimes <= 0)
		{
			this._atk2LoopTimes = 2;
			this._eAction.AnimChangeState(RiderAction.StateEnum.Atk2End, 1f);
		}
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

	public void SpwanEffect()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.atkEffect, base.transform.position + Vector3.back * 0.01f, Quaternion.identity);
		gameObject.GetComponent<SkeletonAnimation>().state.SetAnimation(0, this._eAction.stateMachine.currentState, false);
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.AddComponent<AutoDestroy>();
		UnityEngine.Object.Destroy(gameObject.GetComponent<EnemyEffectSync>());
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayMoveSound()
	{
		R.Audio.PlayEffect(this.moveSound[UnityEngine.Random.Range(0, this.moveSound.Length)], new Vector3?(base.transform.position));
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
			this._eAction.AnimChangeState(RiderAction.StateEnum.WeakMod, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 116; i++)
		{
			if (this._eAction.stateMachine.currentState == "WeakMod" && !this._eAction.IsInWeakSta())
			{
				this._eAction.AnimChangeState(RiderAction.StateEnum.Ready, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void QTECameraZoomIn()
	{
		R.Camera.Controller.CameraZoom(base.transform.position + Vector3.up * 3f, 0.2f, 3f);
		R.Camera.Controller.CameraShake(0.2f, 0.4f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.13333334f, 1f, base.transform.position);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public void QTEPlyerAttack()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.RiderQTEHurt_2, 1f);
	}

	public void QTEPlayerBack()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.EndAtk, 1f);
	}

	public void ExecuteEffect()
	{
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
	}

	public void ExecuteShadeAttack()
	{
		R.Effect.Generate(180, null, base.transform.position, default(Vector3), default(Vector3), true);
	}

	public void ExecutePlyerAttack()
	{
		Vector3 position = base.transform.position + Vector3.up * 2.1f;
		position.z = LayerManager.ZNum.MMiddle_P;
		this.player.position = position;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.RiderQTEHurt_3, 2f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	private RiderAction _eAction;

	private EnemyAttribute _eAttr;

	private int _atk2LoopTimes;

	private EnemyAtk _atk;

	private JsonData _atkData;

	[SerializeField]
	private GameObject atkEffect;

	[SerializeField]
	private int[] moveSound;
}
