using System;
using BehaviorDesigner.Runtime.Tasks;

public class ThiefBombAIAction
{
	[TaskDescription("转头看玩家")]
	[TaskCategory("Enemy/ThiefBomb")]
	public class ThiefBombLookRound : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<ThiefBombAction>();
		}

		public override void OnStart()
		{
			this.action.LookRound();
		}

		public override TaskStatus OnUpdate()
		{
			ThiefBombAction.StateEnum stateEnum = EnumTools.ToEnum<ThiefBombAction.StateEnum>(this.action.stateMachine.currentState, false);
			if (stateEnum == ThiefBombAction.StateEnum.LookGround)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		private ThiefBombAction action;
	}
}
