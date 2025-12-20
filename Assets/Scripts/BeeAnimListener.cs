using System;
using System.Collections;
using LitJson;
using OnionSkin;
using UnityEngine;

public class BeeAnimListener : BaseBehaviour
{
	public BeeAnimListener(float angle)
	{
		this.angle = angle;
	}

	private void Start()
	{
		this._eAction = base.GetComponent<BeeAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._onion = base.GetComponent<OnionCreator>();
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.蜜蜂];
	}

	public void ChangeState(BeeAction.StateEnum sta)
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

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
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
			this._eAction.AnimChangeState(BeeAction.StateEnum.WeakMod, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(BeeAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 58; i++)
		{
			if (this._eAction.stateMachine.currentState == "WeakMod" && !this._eAction.IsInWeakSta())
			{
				this._eAction.AnimChangeState(BeeAction.StateEnum.Idle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void OpenOnion()
	{
		this._onion.Open(true, 0.7f, this.onionObj);
	}

	public void RotateLaser()
	{
		this.beeLaserHalo.gameObject.SetActive(true);
		Vector3 position = R.Player.Transform.position;
		Vector3 position2 = base.transform.position;
		float num = Vector3.Angle(position - position2, Vector3.down);
		int faceDir = this._eAttr.faceDir;
		Transform transform = this.bone.transform.Find("Eye");
		Transform transform2 = R.Effect.Generate(154, transform, default(Vector3), default(Vector3), default(Vector3), true);
		Laser component = transform2.GetComponent<Laser>();
		component.RotateByPoint((float)(28 * -(float)faceDir), transform.position, (180f - num / 2f) * (float)faceDir, 0.85f, base.gameObject);
		component.SetAtkData(this._jsonData[this._eAction.stateMachine.currentState], this._eAttr.atk);
		R.Audio.PlayEffect(110, new Vector3?(position));
	}

	public void NormalLaser()
	{
		this.beeLaserHalo.gameObject.SetActive(true);
		Vector3 position = R.Player.Transform.position;
		Transform transform = this.bone.transform.Find("Eye");
		float num = Vector3.Angle(position - transform.position, Vector3.down);
		int faceDir = this._eAttr.faceDir;
		Transform transform2 = R.Effect.Generate(155, transform, default(Vector3), default(Vector3), default(Vector3), true);
		Laser component = transform2.GetComponent<Laser>();
		component.AppearLaser(transform.position, (180f - num) * (float)faceDir, base.gameObject);
		component.SetAtkData(this._jsonData[this._eAction.stateMachine.currentState], this._eAttr.atk);
		R.Audio.PlayEffect(109, new Vector3?(position));
	}

	public void DisappearHalo()
	{
		this.beeLaserHalo.gameObject.SetActive(false);
	}

	private BeeAction _eAction;

	private EnemyAttribute _eAttr;

	private float angle;

	public SkeletonUtilityBone bone;

	public Transform beeLaserHalo;

	private OnionCreator _onion;

	[SerializeField]
	private GameObject[] onionObj;

	private JsonData _jsonData;
}
