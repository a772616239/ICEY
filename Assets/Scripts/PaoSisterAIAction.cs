using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class PaoSisterAIAction
{
	[TaskCategory("Enemy/PaoSister")]
	[TaskDescription("炮姐攻击")]
	public class PaoAtk : BehaviorDesigner.Runtime.Tasks.Action
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
			this.action = base.GetComponent<DaoAction>();
		}

		public override void OnStart()
		{
			int dir = InputSetting.JudgeDir(this.transform.position.x, this.player.position.x);
			this.action.ChangeFace(dir);
			PaoSisterAIAction.PaoAtk.PaoAtkType paoAtkType = this.paoAtkType;
			if (paoAtkType != PaoSisterAIAction.PaoAtk.PaoAtkType.Atk1)
			{
				if (paoAtkType == PaoSisterAIAction.PaoAtk.PaoAtkType.Atk2)
				{
					this.action.Attack3(dir);
				}
			}
			else
			{
				this.action.Attack3(dir);
			}
		}

		public override TaskStatus OnUpdate()
		{
			string currentState = this.action.stateMachine.currentState;
			if (currentState == DaoAction.State.PaoAtk1)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		public PaoSisterAIAction.PaoAtk.PaoAtkType paoAtkType;

		private DaoAction action;

		public enum PaoAtkType
		{
			Atk1,
			Atk2
		}
	}

	[TaskCategory("Enemy/PaoSister")]
	[TaskDescription("炮姐在主角两个身位外")]
	public class InAttackRange : BehaviorDesigner.Runtime.Tasks.Action
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
			this.ai = base.GetComponent<EnemyAIAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			EnemyAIAttribute.ExpectationParam expectationParam = this.ai.attackExpectations[0];
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num > expectationParam.distance + expectationParam.range)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAIAttribute ai;
	}

	[TaskDescription("炮姐闪避")]
	[TaskCategory("Enemy/PaoSister")]
	public class PaoRunAway : BehaviorDesigner.Runtime.Tasks.Action
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
			this.action = base.GetComponent<DaoAction>();
		}

		public override void OnStart()
		{
			this.action.SideStep();
		}

		public override TaskStatus OnUpdate()
		{
			string currentState = this.action.stateMachine.currentState;
			if (currentState == DaoAction.State.RunAwayReady)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		private DaoAction action;
	}

	[TaskCategory("Enemy/PaoSister")]
	[TaskDescription("移动到两个身位外")]
	public class FarAwayPlayer : BehaviorDesigner.Runtime.Tasks.Action
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
			this.action = base.GetComponent<DaoAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
			this.paoAtk = base.GetComponent<EnemyAIAttribute>().attackExpectations[0];
			this.atkRange = this.paoAtk.distance + this.paoAtk.range;
		}

		public override void OnStart()
		{
			this.randomMoveLen = UnityEngine.Random.Range(0f, 1f);
			int dir = this.JudeMoveDir();
			this.action.ChangeFace(dir);
			this.aimPos = this.transform.position.x + (this.randomMoveLen + this.atkRange) * (float)this.eAttr.faceDir;
			this.aimPos = Mathf.Clamp(this.aimPos, GameArea.EnemyRange.min.x + this.eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this.eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.action.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.action.transform.position.x, this.aimPos) == this.eAttr.faceDir;
			if (flag)
			{
				return TaskStatus.Running;
			}
			return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private int JudeMoveDir()
		{
			float num = this.atkRange + this.randomMoveLen;
			if (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) < num || Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.max.x) < num)
			{
				return (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) >= num) ? -1 : 1;
			}
			return (this.transform.position.x - this.player.position.x <= 0f) ? -1 : 1;
		}

		private DaoAction action;

		private EnemyAttribute eAttr;

		private EnemyAIAttribute.ExpectationParam paoAtk;

		private float atkRange;

		private float randomMoveLen;

		private float aimPos;
	}
}
