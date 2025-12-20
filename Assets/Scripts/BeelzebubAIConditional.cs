using System;
using BehaviorDesigner.Runtime.Tasks;

public class BeelzebubAIConditional
{
	[TaskDescription("判断能否嘲讽")]
	[TaskCategory("Enemy/Beelzebub")]
	public class AnrgyJudge : Conditional
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<BeelzebubAction>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this._action.Angry)
			{
				this._action.Angry = false;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private BeelzebubAction _action;
	}
}
