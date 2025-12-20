using System;
using UnityEngine;

public class FaustAnimEvent : BaseBehaviour
{
	private void Awake()
	{
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._action = base.GetComponent<FaustAction>();
	}

	private void Update()
	{
		if (!this._start && this._eAttr.isOnGround)
		{
			this._startPos = base.transform.position;
			this._start = true;
		}
	}

	public void ChangeState(FaustAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void Disappear()
	{
		base.Invoke("Appear", 0.75f);
	}

	public void Appear()
	{
		int disappearTimes = this._disappearTimes;
		if (disappearTimes != 0)
		{
			if (disappearTimes == 1)
			{
				base.transform.position = this._startPos + Vector3.right * 2f;
				this._call = true;
			}
		}
		else
		{
			base.transform.position = this._startPos + Vector3.left * 8f;
			this._disappearTimes++;
		}
		this._action.FaceToPlayer();
		this._action.AnimChangeState(FaustAction.StateEnum.Appear, 1f);
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

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void AppearEnd()
	{
		this._action.AnimChangeState((!this._call) ? FaustAction.StateEnum.Idle : FaustAction.StateEnum.IdleToCall, 1f);
	}

	private FaustAction _action;

	[SerializeField]
	private Vector3 _startPos;

	private bool _start;

	private EnemyAttribute _eAttr;

	private int _disappearTimes;

	private bool _call;
}
