using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RiderAIAction
{
	[TaskCategory("Enemy/Rider")]
	[TaskDescription("Rider攻击")]
	public class RiderAttack : BehaviorDesigner.Runtime.Tasks.Action
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
			this.action = base.GetComponent<RiderAction>();
		}

		public override void OnStart()
		{
			int dir = InputSetting.JudgeDir(this.transform.position, this.player.position);
			RiderAIAction.RiderAttack.AtkType atkType = this.atkType;
			if (atkType != RiderAIAction.RiderAttack.AtkType.Atk1)
			{
				if (atkType != RiderAIAction.RiderAttack.AtkType.Atk2)
				{
					if (atkType == RiderAIAction.RiderAttack.AtkType.Atk3)
					{
						this.action.Attack3(dir);
					}
				}
				else
				{
					this.action.Attack2(dir);
				}
			}
			else
			{
				this.action.Attack1(dir);
			}
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		public RiderAIAction.RiderAttack.AtkType atkType;

		private RiderAction action;

		public enum AtkType
		{
			Atk1,
			Atk2,
			Atk3
		}
	}
}
