using System;
using UnityEngine;

public class AirBombAction : BaseBehaviour
{
	private void Awake()
	{
		this.stateMachine = base.GetComponent<StateMachine>();
		this.spineAnim = base.GetComponent<SpineAnimationController>();
	}

	private void Start()
	{
		this.stateMachine.AddStates(typeof(AirBombAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.AnimChangeState(AirBombAction.StateEnum.Idle);
	}

	private void Update()
	{
		float num = Vector2.Distance(base.transform.position, R.Player.Transform.position + Vector3.up);
		if (num < 3.5f && !this.attack1)
		{
			this.Attack1();
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		AirBombAction.StateEnum stateEnum = EnumTools.ToEnum<AirBombAction.StateEnum>(args.state, false);
		if (stateEnum != AirBombAction.StateEnum.Idle)
		{
			if (stateEnum == AirBombAction.StateEnum.Atk1)
			{
				this.spineAnim.Play(stateEnum, false, true, 1f);
			}
		}
		else
		{
			this.spineAnim.Play(stateEnum, true, false, 1f);
		}
	}

	public void Attack1()
	{
		this.attack1 = true;
		this.AnimChangeState(AirBombAction.StateEnum.Atk1);
	}

	private void AnimChangeState(AirBombAction.StateEnum state)
	{
		this.stateMachine.SetState(state);
	}

	private StateMachine stateMachine;

	private SpineAnimationController spineAnim;

	private bool attack1;

	public enum StateEnum
	{
		Idle,
		Atk1
	}

	public enum NormalSta
	{
		Idle
	}

	public enum DieSta
	{
		Atk1
	}
}
