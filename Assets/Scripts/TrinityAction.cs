using System;
using UnityEngine;

public class TrinityAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(TrinityAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		base.AnimChangeState(TrinityAction.StateEnum.Idle, 1f);
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (!(state == "Idle"))
			{
				if (state == "ExecuteDie")
				{
					this.spineAnim.Play(args.state, false, true, 1f);
				}
			}
			else
			{
				this.spineAnim.Play(args.state, true, false, 1f);
			}
		}
	}

	public override void AnimMove()
	{
	}

	public override void AnimReady()
	{
		base.AnimChangeState(TrinityAction.StateEnum.Idle, 1f);
	}

	public override bool IsInAttackState()
	{
		return false;
	}

	public override bool IsInDeadState(string state)
	{
		return state == "ExecuteDie";
	}

	public override bool IsInWeakSta()
	{
		return false;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return false;
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return false;
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState == "Idle" && base.IsInNormalState();
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

	public enum StateEnum
	{
		Idle,
		Execute,
		ExecuteDie
	}
}
