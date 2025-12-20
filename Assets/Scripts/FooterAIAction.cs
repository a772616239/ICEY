using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FooterAIAction
{
	[TaskCategory("Enemy/Footer")]
	[TaskDescription("大脚跳跃")]
	public class FooterJump : BehaviorDesigner.Runtime.Tasks.Action
	{
		private Transform player
		{
			get
			{
				return R.Player.Transform;
			}
		}

		public override void OnAwake()
		{
			this.action = base.GetComponent<FooterAction>();
		}

		public override void OnStart()
		{
			this.action.FaceToPlayer();
			if (this.back)
			{
				this.action.SideStep();
			}
			else
			{
				this.action.Jump();
			}
		}

		public override TaskStatus OnUpdate()
		{
			FooterAction.StateEnum stateEnum = EnumTools.ToEnum<FooterAction.StateEnum>(this.action.stateMachine.currentState, false);
			if (stateEnum == FooterAction.StateEnum.Jump || stateEnum == FooterAction.StateEnum.MoveAway)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		public bool back;

		private float moveDis;

		private float aimPos;

		private FooterAction action;
	}
}
