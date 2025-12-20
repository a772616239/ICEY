using System;
using BehaviorDesigner.Runtime.Tasks;
using ExtensionMethods;
using UnityEngine;

public class DahalFAIAction
{
	[TaskCategory("Enemy/DahalF")]
	[TaskDescription("Dahal闪避")]
	public class DahalFSideStep : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this._action = base.GetComponent<DahalFAction>();
		}

		public override void OnStart()
		{
			this._action.SideStep();
		}

		public override TaskStatus OnUpdate()
		{
			return (!(this._action.stateMachine.currentState == "GroundDodge")) ? TaskStatus.Success : TaskStatus.Running;
		}

		private DahalFAction _action;
	}

	[TaskDescription("向前移动一段距离,距离参数在属性里设置")]
	[TaskCategory("Enemy/DahalF")]
	public class DahalFMoveSomeDistance : BehaviorDesigner.Runtime.Tasks.Action
	{
		public override void OnAwake()
		{
			this._eAttr = base.GetComponent<EnemyAttribute>();
			this._enemy = base.GetComponent<DahalFAction>();
		}

		public override void OnStart()
		{
			if (this.randomDis)
			{
				this.dis = UnityEngine.Random.Range(this.randomMin, this.randomMax);
			}
			this.aimPosX = this._enemy.transform.position.x + (float)this._eAttr.faceDir * this.dis;
			this.aimPosX = Mathf.Clamp(this.aimPosX, GameArea.EnemyRange.min.x + this._eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this._eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._enemy.DahyalAutoMove(this.moveType))
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this._enemy.transform.position.x, this.aimPosX) == this._eAttr.faceDir;
			if (flag)
			{
				return TaskStatus.Running;
			}
			if (!this.isArrivedStopAnim)
			{
				return TaskStatus.Success;
			}
			if (this._enemy.StopMoveToIdle())
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

		[BehaviorDesigner.Runtime.Tasks.Tooltip("1普通移动 2快速移动")]
		public int moveType;

		private EnemyAttribute _eAttr;

		private DahalFAction _enemy;

		private float aimPosX;
	}

	[TaskDescription("移动距离主角的某个位置范围")]
	[TaskCategory("Enemy/DahalF")]
	public class DahalFMoveDistanceRange : BehaviorDesigner.Runtime.Tasks.Action
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
			this._action = base.GetComponent<DahalFAction>();
			this._eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			this._finalMoveLen = UnityEngine.Random.Range(this.moveRangeMin, this.moveRangeMax);
			int num = (UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1;
			this._aimPos = this.player.transform.position.x + this._finalMoveLen * (float)num;
			int dir = InputSetting.JudgeDir(this.transform.position.x, this._aimPos);
			this._action.TurnRound(dir);
			this._aimPos = Mathf.Clamp(this._aimPos, GameArea.EnemyRange.min.x + this._eAttr.bounds.size.x, GameArea.EnemyRange.max.x - this._eAttr.bounds.size.x);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.DahyalAutoMove(this.moveType))
			{
				return TaskStatus.Failure;
			}
			bool flag = InputSetting.JudgeDir(this._action.transform.position.x, this._aimPos) == this._eAttr.faceDir;
			if (!flag)
			{
				return (!this._action.StopMoveToIdle()) ? TaskStatus.Failure : TaskStatus.Success;
			}
			float x = Mathf.Abs(this.player.position.x - this.transform.position.x);
			if (MathfX.isInRange(x, this.moveRangeMin, this.moveRangeMax))
			{
				this._action.StopMoveToIdle();
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		private DahalFAction _action;

		private EnemyAttribute _eAttr;

		public float moveRangeMin;

		public float moveRangeMax;

		[BehaviorDesigner.Runtime.Tasks.Tooltip("1普通移动 2快速移动")]
		public int moveType;

		private float _aimPos;

		private float _finalMoveLen;
	}

	[TaskCategory("Enemy/DahalF")]
	[TaskDescription("Atk2")]
	public class Atk2 : BehaviorDesigner.Runtime.Tasks.Action
	{
		private int overTimes
		{
			get
			{
				return (R.GameData.Difficulty > 1) ? 4 : 2;
			}
		}

		public override void OnAwake()
		{
			this._action = base.GetComponent<DahalFAction>();
			this._eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			this._action.FaceToPlayer();
			this._action.Attack2(this._eAttr.faceDir);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.stateMachine.currentState.IsInArray(DahalFAction.AttackSta))
			{
				return TaskStatus.Failure;
			}
			if (this._eAttr.faceDir == -1)
			{
				if (this.transform.position.x > GameArea.EnemyRange.min.x + 0.9f)
				{
					return TaskStatus.Running;
				}
				this._action.Atk2HitWallTimes++;
				if (this._action.Atk2HitWallTimes >= this.overTimes)
				{
					this._action.AnimChangeState(DahalFAction.StateEnum.Atk2End, 1f);
					return TaskStatus.Success;
				}
				this.TurnBack();
				return TaskStatus.Running;
			}
			else
			{
				if (this.transform.position.x < GameArea.EnemyRange.max.x - 0.9f)
				{
					return TaskStatus.Running;
				}
				this._action.Atk2HitWallTimes++;
				if (this._action.Atk2HitWallTimes >= this.overTimes)
				{
					this._action.AnimChangeState(DahalFAction.StateEnum.Atk2End, 1f);
					return TaskStatus.Success;
				}
				this.TurnBack();
				return TaskStatus.Running;
			}
		}

		private void TurnBack()
		{
			int dir = (this._eAttr.faceDir != -1) ? -1 : 1;
			this._action.ChangeFace(dir);
		}

		private DahalFAction _action;

		private EnemyAttribute _eAttr;
	}

	[TaskCategory("Enemy/DahalF")]
	[TaskDescription("Atk6")]
	public class Atk6 : BehaviorDesigner.Runtime.Tasks.Action
	{
		private bool Atk6End
		{
			get
			{
				return R.GameData.Difficulty <= 1 || this._action.Atk6HitWall;
			}
		}

		public override void OnAwake()
		{
			this._action = base.GetComponent<DahalFAction>();
			this._eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override void OnStart()
		{
			this._action.FaceToPlayer();
			this._action.Attack6(this._eAttr.faceDir);
		}

		public override TaskStatus OnUpdate()
		{
			if (!this._action.stateMachine.currentState.IsInArray(DahalFAction.AttackSta))
			{
				return TaskStatus.Failure;
			}
			if (this._eAttr.faceDir == -1)
			{
				if (this.transform.position.x > GameArea.EnemyRange.min.x + 0.9f)
				{
					return TaskStatus.Running;
				}
				if (this.Atk6End)
				{
					this._action.AnimChangeState(DahalFAction.StateEnum.Atk6End, 1f);
					return TaskStatus.Success;
				}
				this._action.Atk6HitWall = true;
				return TaskStatus.Running;
			}
			else
			{
				if (this.transform.position.x < GameArea.EnemyRange.max.x - 0.9f)
				{
					return TaskStatus.Running;
				}
				if (this.Atk6End)
				{
					this._action.AnimChangeState(DahalFAction.StateEnum.Atk6End, 1f);
					return TaskStatus.Success;
				}
				this._action.Atk6HitWall = true;
				return TaskStatus.Running;
			}
		}

		private DahalFAction _action;

		private EnemyAttribute _eAttr;
	}
}
