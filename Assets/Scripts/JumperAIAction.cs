using System;
using BehaviorDesigner.Runtime.Tasks;
using ExtensionMethods;
using UnityEngine;

public class JumperAIAction
{
	[TaskDescription("跳拳攻击")]
	[TaskCategory("Enemy/Jumper")]
	public class JumperAttack : BehaviorDesigner.Runtime.Tasks.Action
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
			this.action = base.GetComponent<JumperAction>();
		}

		public override void OnStart()
		{
			int dir = InputSetting.JudgeDir(this.transform.position, this.player.position);
			switch (this.atkType)
			{
			case JumperAIAction.JumperAttack.AtkType.Atk1:
				this.action.Attack1(dir);
				break;
			case JumperAIAction.JumperAttack.AtkType.Atk2:
				this.action.Attack2(dir);
				break;
			case JumperAIAction.JumperAttack.AtkType.Atk3:
				this.action.Attack3(dir);
				break;
			case JumperAIAction.JumperAttack.AtkType.Atk4:
				this.action.Attack4(dir);
				break;
			case JumperAIAction.JumperAttack.AtkType.Atk5:
				this.action.Attack5(dir);
				break;
			}
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.stateMachine.currentState.IsInArray(JumperAction.AttackSta)) ? TaskStatus.Success : TaskStatus.Running;
		}

		public JumperAIAction.JumperAttack.AtkType atkType;

		private JumperAction action;

		public enum AtkType
		{
			Atk1,
			Atk2,
			Atk3,
			Atk4,
			Atk5
		}
	}

	[TaskDescription("跳拳防御")]
	[TaskCategory("Enemy/Jumper")]
	public class JumperDefence : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JumperAction>();
		}

		public override void OnStart()
		{
			this.action.Defence();
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.action.IsInDefenceState()) ? TaskStatus.Success : TaskStatus.Running;
		}

		private JumperAction action;
	}
}
