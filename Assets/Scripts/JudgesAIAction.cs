using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class JudgesAIAction
{
	[TaskDescription("选择攻击类型")]
	[TaskCategory("Enemy/Judges")]
	public class SetNextAttackType : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<JudgesAction>();
			this.hurt = base.GetComponent<JudgesHurt>();
		}

		public override void OnStart()
		{
			JudgesAction.AttackType attackType = this.action.attackType;
			if (attackType != JudgesAction.AttackType.Saws)
			{
				if (attackType != JudgesAction.AttackType.Laser)
				{
					if (attackType == JudgesAction.AttackType.HitGround)
					{
						if (this.hurt.currentPhase < 4)
						{
							this.action.attackType = ((UnityEngine.Random.Range(0, 2) != 0) ? JudgesAction.AttackType.Saws : JudgesAction.AttackType.Laser);
						}
					}
				}
				else if (this.hurt.currentPhase != 4)
				{
					this.action.attackType = ((UnityEngine.Random.Range(0, 2) != 0) ? JudgesAction.AttackType.Saws : JudgesAction.AttackType.HitGround);
				}
				else
				{
					this.action.attackType = JudgesAction.AttackType.Saws;
				}
			}
			else if (this.hurt.currentPhase != 4)
			{
				this.action.attackType = ((UnityEngine.Random.Range(0, 2) != 0) ? JudgesAction.AttackType.HitGround : JudgesAction.AttackType.Laser);
			}
			else
			{
				this.action.attackType = JudgesAction.AttackType.Laser;
			}
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		private JudgesAction action;

		private JudgesHurt hurt;
	}
}
