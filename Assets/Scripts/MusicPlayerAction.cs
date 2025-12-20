using System;
using DG.Tweening;
using UnityEngine;

public class MusicPlayerAction : BaseBehaviour
{
	public int currentHP
	{
		get
		{
			return this._currentHP;
		}
		set
		{
			this._currentHP = Mathf.Clamp(value, this._limited, this.maxHP);
		}
	}

	private Animator Anim
	{
		get
		{
			Animator result;
			if ((result = this._anim) == null)
			{
				result = (this._anim = base.GetComponent<Animator>());
			}
			return result;
		}
	}

	private void Start()
	{
		this._limited = 0;
		this.currentHP = this.maxHP;
		this.ChangeState(MusicPlayerAction.State.Appear);
	}

	public void ChangeState(MusicPlayerAction.State state)
	{
		this.Anim.Play(state.ToString());
	}

	public void SetLimited(int value)
	{
		this._limited = value;
	}

	public void StartShake()
	{
		Vector3 strength = new Vector3(0.2f, 0.2f, 0f);
		base.transform.DOShakePosition(1f, strength, 10, 90f, false, true).SetLoops(-1);
	}

	public void StopShake()
	{
		base.transform.DOKill(false);
	}

	private Animator _anim;

	private int _currentHP;

	private int _limited;

	public int maxHP;

	public enum State
	{
		Appear,
		Play,
		ChangeMusic,
		Disappear,
		Blast
	}
}
