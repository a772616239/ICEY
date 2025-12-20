using System;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyBaseAIAction
{
	[TaskDescription("移动到当前选择的攻击可以攻击的位置")]
	[TaskCategory("Enemy/Base")]
	public class MoveAtkExAim : BehaviorDesigner.Runtime.Tasks.Action
	{
		private GameObject player
		{
			get
			{
				return R.Player.GameObject;
			}
		}

		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
			this.enemy = this.eAttr.GetComponent<EnemyBaseAction>();
			this.aiAttr = this.eAttr.GetComponent<EnemyAIAttribute>();
		}

		public override void OnStart()
		{
			if (this.randomAtkEx)
			{
				this.atkNum = this.RandomRange[UnityEngine.Random.Range(0, this.RandomRange.Length)];
				this.aiAttr.currentEx = this.aiAttr.attackExpectations[(int)this.atkNum];
			}
			else
			{
				this.aiAttr.currentEx = this.aiAttr.attackExpectations[(int)this.atkNum];
			}
			this.aimPos = this.MoveAim();
			this.aimPos = new Vector2(Mathf.Clamp(this.aimPos.x, GameArea.EnemyRange.min.x, GameArea.EnemyRange.max.x), this.aimPos.y);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.enemy.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.enemy.transform.position.x, this.aimPos.x) == this.eAttr.faceDir;
			Vector2 dis = this.aimPos - (Vector2)R.Player.Transform.position;
			bool flag2 = this.MoveStop(dis);
			if (flag && !flag2)
			{
				return TaskStatus.Running;
			}
			if (!this.isArrivedStopAnim)
			{
				return TaskStatus.Success;
			}
			if (this.enemy.StopMoveToIdle())
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		protected virtual Vector2 MoveAim()
		{
			EnemyAIAttribute.ExpectationParam currentEx = this.aiAttr.currentEx;
			Vector3 v = R.Player.Transform.position - UnityEngine.Random.Range(currentEx.distance, currentEx.range + currentEx.distance) * Vector3.right;
			Vector3 v2 = R.Player.Transform.position + UnityEngine.Random.Range(currentEx.distance, currentEx.range + currentEx.distance) * Vector3.right;
			if (v.x < GameArea.EnemyRange.min.x)
			{
				return v2;
			}
			if (v2.x > GameArea.EnemyRange.max.x)
			{
				return v;
			}
			float num = Mathf.Abs(this.transform.position.x - v.x);
			float num2 = Mathf.Abs(this.transform.position.x - v2.x);
			if (num < num2)
			{
				return v;
			}
			return v2;
		}

		protected virtual bool MoveStop(Vector2 dis)
		{
			EnemyAIAttribute.ExpectationParam currentEx = this.aiAttr.currentEx;
			Vector2 vector = this.enemy.transform.position - R.Player.Transform.position;
			bool flag = MathfX.isInMiddleRange(R.Player.Transform.position.x, (currentEx.distance + currentEx.range / 2f) * (float)this.aiAttr.facePlayerDir + this.enemy.transform.position.x, currentEx.range);
			if (flag)
			{
				if (dis.x > 0f && vector.x > 0f && vector.x < dis.x)
				{
					return true;
				}
				if (dis.x < 0f && vector.x < 0f && vector.x > dis.x)
				{
					return true;
				}
			}
			return !this.enemy.IsInNormalState();
		}

		[BehaviorDesigner.Runtime.Tasks.Tooltip("到达了后是否停止播放移动动画")]
		public bool isArrivedStopAnim = true;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("是否随机选择攻击目标")]
		public bool randomAtkEx;

		public EnemyBaseAIAction.MoveAtkExAim.ATK[] RandomRange;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择ATK几")]
		public EnemyBaseAIAction.MoveAtkExAim.ATK atkNum;

		private EnemyAttribute eAttr;

		private EnemyBaseAction enemy;

		private EnemyAIAttribute aiAttr;

		private Vector2 aimPos;

		public enum ATK
		{
			ATK1,
			ATK2,
			ATK3,
			ATK4,
			ATK5,
			ATK6,
			ATK7,
			ATK8,
			ATK9,
			ATK10
		}
	}

	[TaskDescription("远离主角，以攻击范围为准")]
	[TaskCategory("Enemy/Base")]
	public class FarAwayPlayer : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		private bool nearToPlayer
		{
			get
			{
				EnemyAIAttribute.ExpectationParam expectationParam = base.GetComponent<EnemyAIAttribute>().attackExpectations[(int)this.atkNum];
				float num = Mathf.Abs(this.transform.position.x - R.Player.Transform.position.x);
				return num < expectationParam.range + expectationParam.distance && num > expectationParam.distance;
			}
		}

		public override void OnStart()
		{
			EnemyAIAttribute.ExpectationParam expectationParam = base.GetComponent<EnemyAIAttribute>().attackExpectations[(int)this.atkNum];
			this.atkRange = UnityEngine.Random.Range(expectationParam.distance, expectationParam.distance + expectationParam.range);
			int num = this.JudeMoveDir();
			this.aimPos = R.Player.Transform.position.x + this.atkRange * (float)num;
			this.action.TurnRound(InputSetting.JudgeDir(this.transform.position.x, this.aimPos));
			this.aimPos = Mathf.Clamp(this.aimPos, GameArea.EnemyRange.min.x + this.eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this.eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.action.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.action.transform.position.x, this.aimPos) == this.eAttr.faceDir;
			if (!flag)
			{
				return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			if (this.nearToPlayer)
			{
				return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		private int JudeMoveDir()
		{
			float num = this.atkRange;
			if (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) < num || Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.max.x) < num)
			{
				return (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) >= num) ? -1 : 1;
			}
			return (this.transform.position.x - R.Player.Transform.position.x <= 0f) ? -1 : 1;
		}

		private EnemyBaseAction action;

		private EnemyAttribute eAttr;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择ATK几")]
		public EnemyBaseAIAction.FarAwayPlayer.ATK atkNum;

		private float atkRange;

		private float aimPos;

		public enum ATK
		{
			ATK1,
			ATK2,
			ATK3,
			ATK4,
			ATK5,
			ATK6,
			ATK7,
			ATK8,
			ATK9,
			ATK10
		}
	}

	[TaskDescription("远离主角，以攻击范围为准,方向随机")]
	[TaskCategory("Enemy/Base")]
	public class RandomMovePlayer : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		private bool nearToPlayer
		{
			get
			{
				EnemyAIAttribute.ExpectationParam expectationParam = base.GetComponent<EnemyAIAttribute>().attackExpectations[(int)this.atkNum];
				float num = Mathf.Abs(this.transform.position.x - R.Player.Transform.position.x);
				return num < expectationParam.range + expectationParam.distance && num > expectationParam.distance;
			}
		}

		public override void OnStart()
		{
			EnemyAIAttribute.ExpectationParam expectationParam = base.GetComponent<EnemyAIAttribute>().attackExpectations[(int)this.atkNum];
			this.atkRange = UnityEngine.Random.Range(expectationParam.distance, expectationParam.distance + expectationParam.range);
			int num = this.JudeMoveDir();
			this.aimPos = R.Player.Transform.position.x + this.atkRange * (float)num;
			this.action.TurnRound(InputSetting.JudgeDir(this.transform.position.x, this.aimPos));
			this.aimPos = Mathf.Clamp(this.aimPos, GameArea.EnemyRange.min.x + this.eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this.eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.action.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.action.transform.position.x, this.aimPos) == this.eAttr.faceDir;
			if (!flag)
			{
				return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			if (this.nearToPlayer)
			{
				return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		private int JudeMoveDir()
		{
			float num = this.atkRange;
			if (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) < num || Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.max.x) < num)
			{
				return (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) >= num) ? -1 : 1;
			}
			return (UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1;
		}

		private EnemyBaseAction action;

		private EnemyAttribute eAttr;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("选择ATK几")]
		public EnemyBaseAIAction.RandomMovePlayer.ATK atkNum;

		private float atkRange;

		private float aimPos;

		public enum ATK
		{
			ATK1,
			ATK2,
			ATK3,
			ATK4,
			ATK5,
			ATK6,
			ATK7,
			ATK8,
			ATK9,
			ATK10
		}
	}

	[TaskDescription("移动距离主角的某个位置")]
	[TaskCategory("Enemy/Base")]
	public class MoveDistance : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			int dir = this.JudeMoveDir();
			this.action.TurnRound(dir);
			this.aimPos = R.Player.Transform.position.x + this.moveRange * (float)this.eAttr.faceDir;
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
			float num = this.moveRange;
			if (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) < num || Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.max.x) < num)
			{
				return (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) >= num) ? -1 : 1;
			}
			return (UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1;
		}

		private EnemyBaseAction action;

		private EnemyAttribute eAttr;

		public float moveRange;

		private float aimPos;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("向前移动一段距离,距离参数在属性里设置")]
	public class MoveSomeDistance : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
			if (this.eAttr == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有Enmey的对象上");
			}
			this.enemy = base.GetComponent<EnemyBaseAction>();
			if (this.enemy == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有Enmey的对象上");
			}
		}

		public override void OnStart()
		{
			if (this.randomDis)
			{
				this.dis = UnityEngine.Random.Range(this.randomMin, this.randomMax);
			}
			this.aimPosX = this.enemy.transform.position.x + (float)this.eAttr.faceDir * this.dis;
			this.aimPosX = Mathf.Clamp(this.aimPosX, GameArea.EnemyRange.min.x + this.eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this.eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.enemy.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.enemy.transform.position.x, this.aimPosX) == this.eAttr.faceDir;
			if (flag)
			{
				return TaskStatus.Running;
			}
			if (!this.isArrivedStopAnim)
			{
				return TaskStatus.Success;
			}
			if (this.enemy.StopMoveToIdle())
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		[BehaviorDesigner.Runtime.Tasks.Tooltip("移动距离")]
		public float dis;

		public bool randomDis;

		public float randomMin;

		public float randomMax;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("到达了后是否停止播放移动动画")]
		public bool isArrivedStopAnim = true;

		private EnemyAttribute eAttr;

		private EnemyBaseAction enemy;

		private float aimPosX;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("移动距离主角的某个位置范围")]
	public class MoveDistanceRange : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			this.finalMoveLen = UnityEngine.Random.Range(this.moveRangeMin, this.moveRangeMax);
			int num = (UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1;
			this.aimPos = R.Player.Transform.position.x + this.finalMoveLen * (float)num;
			int dir = InputSetting.JudgeDir(this.transform.position.x, this.aimPos);
			this.action.TurnRound(dir);
			this.aimPos = Mathf.Clamp(this.aimPos, GameArea.EnemyRange.min.x + this.eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this.eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this.action.AutoMove())
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this.action.transform.position.x, this.aimPos) == this.eAttr.faceDir;
			if (!flag)
			{
				return (!this.action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			float x = Mathf.Abs(R.Player.Transform.position.x - this.transform.position.x);
			if (MathfX.isInRange(x, this.moveRangeMin, this.moveRangeMax))
			{
				this.action.StopMoveToIdle();
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		private int JudeMoveDir()
		{
			float num = this.finalMoveLen;
			if (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) < num || Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.max.x) < num)
			{
				return (Mathf.Abs(this.transform.position.x - GameArea.EnemyRange.min.x) >= num) ? -1 : 1;
			}
			return (UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1;
		}

		private EnemyBaseAction action;

		private EnemyAttribute eAttr;

		public float moveRangeMin;

		public float moveRangeMax;

		private float aimPos;

		private float finalMoveLen;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("移动到主角位置")]
	public class MoveToPlayerPos : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			this.aimPos = R.Player.Transform.position.x;
			int dir = InputSetting.JudgeDir(this.transform.position.x, this.aimPos);
			this.action.TurnRound(dir);
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

		private EnemyBaseAction action;

		private EnemyAttribute eAttr;

		private float aimPos;

		private float finalMoveLen;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("转向玩家")]
	public class FaceToPlayer : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.enemy = base.GetComponent<EnemyBaseAction>();
		}

		public override void OnStart()
		{
			this.enemy.FaceToPlayer();
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		private EnemyBaseAction enemy;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("在一般状态")]
	public class InNormalState : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.enemy = base.GetComponent<EnemyBaseAction>();
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.enemy.IsInNormalState()) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private EnemyBaseAction enemy;
	}

	[TaskDescription("AI启动")]
	[TaskCategory("Enemy/Base")]
	public class StartAI : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override TaskStatus OnUpdate()
		{
			if (!R.Mode.IsInStoryMode() && R.SceneData.CanAIRun)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}
	}

	[TaskDescription("转身")]
	[TaskCategory("Enemy/Base")]
	public class TurnRound : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
			if (this.eAttr == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有Enmey的对象上");
			}
			this.enemy = base.GetComponent<EnemyBaseAction>();
			if (this.enemy == null)
			{
				global::Log.Error("EnemyBaseAIAction必须使用在有EnemyBaseAction的对象上");
			}
		}

		public override TaskStatus OnUpdate()
		{
			int dir = 1;
			switch (this.turnDir)
			{
			case EnemyBaseAIAction.TurnRound.Dir.Back:
				if (this.eAttr.faceDir == 1)
				{
					dir = -1;
				}
				if (this.eAttr.faceDir == -1)
				{
					dir = 1;
				}
				break;
			case EnemyBaseAIAction.TurnRound.Dir.Left:
				dir = -1;
				break;
			case EnemyBaseAIAction.TurnRound.Dir.Right:
				dir = 1;
				break;
			case EnemyBaseAIAction.TurnRound.Dir.Random:
				dir = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : -1);
				break;
			case EnemyBaseAIAction.TurnRound.Dir.None:
				dir = this.eAttr.faceDir;
				break;
			}
			if (this.enemy.TurnRound(dir))
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		[BehaviorDesigner.Runtime.Tasks.Tooltip("转向方式")]
		public EnemyBaseAIAction.TurnRound.Dir turnDir;

		private Transform playerTransform;

		private EnemyAttribute eAttr;

		private EnemyBaseAction enemy;

		public enum Dir
		{
			Back,
			Left,
			Right,
			Random,
			None
		}
	}

	[TaskDescription("待机")]
	[TaskCategory("Enemy/Base")]
	public class Idle : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.enemy = base.GetComponent<EnemyBaseAction>();
			if (this.enemy == null)
			{
				global::Log.Error("Player没有生成");
			}
		}

		public override void OnStart()
		{
			switch (this.type)
			{
			case EnemyBaseAIAction.Idle.IdleType.Idle1:
				this.enemy.Idle1();
				break;
			case EnemyBaseAIAction.Idle.IdleType.Idle2:
				this.enemy.Idle2();
				break;
			case EnemyBaseAIAction.Idle.IdleType.Idle3:
				this.enemy.Idle3();
				break;
			case EnemyBaseAIAction.Idle.IdleType.Idle4:
				this.enemy.Idle4();
				break;
			case EnemyBaseAIAction.Idle.IdleType.Idle5:
				this.enemy.Idle5();
				break;
			}
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		private EnemyBaseAction enemy;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("待机类型")]
		public EnemyBaseAIAction.Idle.IdleType type;

		public enum IdleType
		{
			Idle1,
			Idle2,
			Idle3,
			Idle4,
			Idle5
		}
	}

	[TaskDescription("等待")]
	[TaskCategory("Enemy/Base")]
	public class Wait : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnStart()
		{
			this.startTime = Time.time;
		}

		public override TaskStatus OnUpdate()
		{
			if (Time.time - this.startTime > this.waitTime)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		[BehaviorDesigner.Runtime.Tasks.Tooltip("等待多久")]
		public float waitTime;

		private float startTime;
	}

	[TaskDescription("等待扫描时间")]
	[TaskCategory("Enemy/Base")]
	public class WaitScanSpeed : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnStart()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
			this.startTime = Time.time;
		}

		public override TaskStatus OnUpdate()
		{
			if (Time.time - this.startTime > this.eAttr.scanSpeed)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		private EnemyAttribute eAttr;

		private float startTime;
	}

	[TaskCategory("Enemy/Base")]
	[TaskDescription("怪物攻击")]
	public class EnemyAttack : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this.action = base.GetComponent<EnemyBaseAction>();
		}

		public override void OnStart()
		{
			int dir = InputSetting.JudgeDir(this.transform.position, R.Player.Transform.position);
			switch (this.atkType)
			{
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk1:
				this.action.Attack1(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk2:
				this.action.Attack2(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk3:
				this.action.Attack3(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk4:
				this.action.Attack4(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk5:
				this.action.Attack5(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk6:
				this.action.Attack6(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk7:
				this.action.Attack7(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk8:
				this.action.Attack8(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk9:
				this.action.Attack9(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk10:
				this.action.Attack10(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk11:
				this.action.Attack11(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk12:
				this.action.Attack12(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk13:
				this.action.Attack13(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk14:
				this.action.Attack14(dir);
				break;
			case EnemyBaseAIAction.EnemyAttack.AtkType.Atk15:
				this.action.Attack15(dir);
				break;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (this.action.IsInAttackState())
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		public EnemyBaseAIAction.EnemyAttack.AtkType atkType;

		private EnemyBaseAction action;

		public enum AtkType
		{
			Atk1,
			Atk2,
			Atk3,
			Atk4,
			Atk5,
			Atk6,
			Atk7,
			Atk8,
			Atk9,
			Atk10,
			Atk11,
			Atk12,
			Atk13,
			Atk14,
			Atk15
		}
	}
}
