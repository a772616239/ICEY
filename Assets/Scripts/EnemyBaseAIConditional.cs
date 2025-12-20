using System;
using BehaviorDesigner.Runtime.Tasks;
using ExtensionMethods;
using GameWorld;
using UnityEngine;

public class EnemyBaseAIConditional
{
	[TaskDescription("判断敌人是否在正常状态")]
	[TaskCategory("Enemy/Base")]
	public class IsInNormalState : Conditional
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			if (this.action == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有EnmeyBaseAction的对象上");
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.IsInNormalState())
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyBaseAction action;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("判断玩家是否在我后面")]
	public class PlayerIsInBackGround : Conditional
	{
		public override void OnAwake()
		{
			this.enemy = base.GetComponent<EnemyAttribute>();
			if (this.enemy == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有Enmey的对象上");
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (InputSetting.JudgeDir(this.transform.position, R.Player.Transform.position) == this.enemy.faceDir)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		private EnemyAttribute enemy;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("判断是否在Attack的范围内")]
	public class PlayerIsInAttackRange : Conditional
	{
		public override void OnAwake()
		{
			this.aiAttr = base.GetComponent<EnemyAIAttribute>();
			if (this.aiAttr == null)
			{
				global::Log.Error("必须使用在有EnemyAIAttribute的对象上");
			}
			this.ex = this.aiAttr.attackExpectations[(int)this.atk];
			if (this.ex == null)
			{
				global::Log.Error("不存在Attack");
			}
		}

		public override TaskStatus OnUpdate()
		{
			bool flag = MathfX.isInMiddleRange(R.Player.Transform.position.x, (this.ex.distance + this.ex.range / 2f) * (float)this.aiAttr.GetComponent<EnemyAttribute>().faceDir + this.aiAttr.transform.position.x, this.ex.range);
			if (flag)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		protected EnemyAIAttribute.ExpectationParam ex;

		protected EnemyAIAttribute aiAttr;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择的攻击类型")]
		public EnemyBaseAIConditional.PlayerIsInAttackRange.ATKType atk;

		public enum ATKType
		{
			ATK1,
			ATK2,
			ATK3,
			ATK4,
			ATK5
		}
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("在两种攻击范围内")]
	public class InTwoAtkRange : Conditional
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
			this.aiAttr = base.GetComponent<EnemyAIAttribute>();
			this.firstEx = this.aiAttr.attackExpectations[(int)this.firstAtk];
			this.secondEx = this.aiAttr.attackExpectations[(int)this.secondAtk];
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Min(this.firstEx.distance + this.firstEx.range, this.secondEx.distance + this.secondEx.range);
			float num2 = Mathf.Max(this.firstEx.distance + this.firstEx.range, this.secondEx.distance + this.secondEx.range);
			float num3 = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (num3 > num && num3 <= num2)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择的第一种攻击类型")]
		public EnemyBaseAIConditional.InTwoAtkRange.ATKType firstAtk;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择的第二种攻击类型")]
		public EnemyBaseAIConditional.InTwoAtkRange.ATKType secondAtk;

		private EnemyAIAttribute aiAttr;

		private EnemyAIAttribute.ExpectationParam firstEx;

		private EnemyAIAttribute.ExpectationParam secondEx;

		public enum ATKType
		{
			ATK1,
			ATK2,
			ATK3,
			ATK4,
			ATK5
		}
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("距离判断")]
	public class DistanceJudge : Conditional
	{
		private Transform player
		{
			get
			{
				return R.Player.Transform;
			}
		}

		public override TaskStatus OnUpdate()
		{
			float num = Mathf.Abs(this.player.position.x - this.transform.position.x);
			EnemyBaseAIConditional.DistanceJudge.JudgeState judgeState = this.judge;
			if (judgeState != EnemyBaseAIConditional.DistanceJudge.JudgeState.Between)
			{
				if (judgeState != EnemyBaseAIConditional.DistanceJudge.JudgeState.Greater)
				{
					if (judgeState != EnemyBaseAIConditional.DistanceJudge.JudgeState.Less)
					{
						return TaskStatus.Failure;
					}
					if (num < this.distanceMax)
					{
						return TaskStatus.Success;
					}
					return TaskStatus.Failure;
				}
				else
				{
					if (num > this.distanceMax)
					{
						return TaskStatus.Success;
					}
					return TaskStatus.Failure;
				}
			}
			else
			{
				if (num >= this.distanceMin && num <= this.distanceMax)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
		}

		public EnemyBaseAIConditional.DistanceJudge.JudgeState judge;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("第一个距离")]
		public float distanceMax;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("仅判断两个距离之间时使用,表示比较近的距离")]
		public float distanceMin;

		public enum JudgeState
		{
			Greater,
			Between,
			Less
		}
	}

	[TaskDescription("判断是否在玩家视野的范围内")]
	[TaskCategory("Enemy/Base")]
	public class IsInPlayerView : Conditional
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
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.eAttr.playerInView)
			{
				return TaskStatus.Success;
			}
			float x = (this.transform.localScale.x <= 0f) ? (this.transform.position.x - this.eAttr.viewXBack) : (this.transform.position.x - this.eAttr.viewXFront);
			Rect rect = new Rect(x, this.transform.position.y - this.eAttr.viewYDown, this.eAttr.viewXFront + this.eAttr.viewXBack, this.eAttr.viewYTop + this.eAttr.viewYDown);
			bool flag = rect.Contains(this.player.position + Vector3.up);
			bool flag2 = this.eAttr.currentHp < this.eAttr.maxHp;
			if (flag)
			{
				EventManager.PostEvent<GameObject, MoveAreaLimitArgs>("EnemyFindPlayer", this.gameObject, new MoveAreaLimitArgs
				{
					battleZoneId = base.GetComponent<MoveAreaLimit>().battleZoneId
				});
			}
			if (flag || flag2)
			{
				this.eAttr.playerInView = true;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAttribute eAttr;
	}

	[TaskDescription("判断AI开启")]
	[TaskCategory("Enemy/Base")]
	public class IsAIOpen : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			return (base.GetComponent<EnemyAttribute>().isDead || !R.SceneData.CanAIRun || R.Mode.IsInStoryMode()) ? TaskStatus.Failure : TaskStatus.Success;
		}
	}

	[TaskDescription("判断是否面向右边")]
	[TaskCategory("Enemy/Base")]
	public class IsTurnRight : Conditional
	{
		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			return (this.eAttr.faceDir != 1) ? TaskStatus.Failure : TaskStatus.Success;
		}

		protected EnemyAttribute eAttr;
	}

	[TaskDescription("能否反击")]
	[TaskCategory("Enemy/Base")]
	public class Counterattack : Conditional
	{
		public override void OnAwake()
		{
			this.hurt = base.GetComponent<EnemyBaseHurt>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			if (this.hurt.canCounterattack)
			{
				this.hurt.canCounterattack = false;
				this.eAttr.paBody = true;
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyBaseHurt hurt;

		private EnemyAttribute eAttr;
	}

	[TaskDescription("判断主角在攻击状态")]
	[TaskCategory("Enemy/Base")]
	public class PlayerInAttack : Conditional
	{
		private StateMachine state
		{
			get
			{
				return R.Player.StateMachine;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.state.currentState.IsInArray(PlayerAction.AirAttackSta) || this.state.currentState.IsInArray(PlayerAction.AttackSta) || this.state.currentState.IsInArray(PlayerAction.UpRisingSta) || this.state.currentState.IsInArray(PlayerAction.HitGroundSta) || this.state.currentState.IsInArray(PlayerAction.FlashAttackSta) || this.state.currentState.IsInArray(PlayerAction.ChargeSta))
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}

	[TaskDescription("判断主角在地面")]
	[TaskCategory("Enemy/Base")]
	public class playerOnGround : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			if (R.Player.Attribute.isOnGround)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}

	[TaskDescription("判断游戏难度，困难为成功")]
	[TaskCategory("Enemy/Base")]
	public class GameDifficulty : Conditional
	{
		public override TaskStatus OnUpdate()
		{
			int difficulty = R.GameData.Difficulty;
			if (difficulty != 1 && difficulty != 3)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}
	}
}
