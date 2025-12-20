using System;
using Core;
using LitJson;
using UnityEngine;

public class GamePlayerInputAssistance : BaseBehaviour
{
	private void Start()
	{
		this.pAttr = R.Player.Attribute;
		this.pAction = base.GetComponent<PlayerAction>();
		base.GetComponent<StateMachine>().OnTransfer += this.OnMyStateTransfer;
		this.atkData = JsonMapper.ToObject(this.atkParm.text);
	}

	private void Update()
	{
		if (!InputSetting.Assistant)
		{
			return;
		}
		if (!this.openAssistant)
		{
			return;
		}
		GameObject nearestAim = this.GetNearestAim(this.assisData.assistanceCheckType);
		if (nearestAim != null)
		{
			this.aimObj = nearestAim;
		}
		if (this.aimObj == null)
		{
			return;
		}
		Vector2 vector = this.GetAssistanceSpeed();
		bool flag = InputSetting.JudgeDir(base.transform.position.x, this.aimObj.transform.position.x) * this.pAttr.faceDir > 0;
		bool flag2 = (this.pAttr.faceDir != -1) ? Core.Input.Game.MoveRight.Pressed : Core.Input.Game.MoveLeft.Pressed;
		if (flag)
		{
			if (flag2)
			{
				this.MovePlayer(vector);
			}
			else
			{
				vector = ((!this.pAttr.isOnGround) ? (vector / 2f) : Vector2.zero);
				this.MovePlayer(vector);
			}
		}
	}

	private void OnMyStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (this.atkData.Contains(args.nextState))
		{
			JsonData data = this.atkData[args.nextState];
			this.assisData = new GamePlayerInputAssistance.InputAssistanceData(data);
			if (this.assisData.assistanceCheckType != GamePlayerInputAssistance.InputAssistanceData.CheckType.None)
			{
				this.StartAssistance();
			}
			else
			{
				this.StopAssistance();
			}
		}
		else
		{
			this.StopAssistance();
		}
	}

	private void MovePlayer(Vector2 speed)
	{
		GamePlayerInputAssistance.InputAssistanceData.AssistanceType assistanceType = this.assisData.assistanceType;
		if (assistanceType != GamePlayerInputAssistance.InputAssistanceData.AssistanceType.X)
		{
			if (assistanceType != GamePlayerInputAssistance.InputAssistanceData.AssistanceType.Y)
			{
				if (assistanceType == GamePlayerInputAssistance.InputAssistanceData.AssistanceType.XY)
				{
					speed.y = Mathf.Clamp(speed.y, float.MinValue, 0f);
				}
			}
			else
			{
				speed.y = Mathf.Clamp(speed.y, float.MinValue, 0f);
				speed.x = R.Player.TimeController.GetCurrentSpeed().x;
			}
		}
		else
		{
			speed.y = 0f;
		}
		this.pAction.pab.move.addSpeed = speed;
	}

	private Vector2 GetAssistanceSpeed()
	{
		if (this.aimObj != null)
		{
			float num = Vector3.Distance(this.aimObj.transform.position, base.transform.position);
			num -= this.aimObj.GetComponent<EnemyAttribute>().bounds.size.x - 1f;
			num = Mathf.Clamp(num, 0f, float.MaxValue);
			Vector2 a = (this.aimObj.transform.position - base.transform.position).normalized;
			Vector2 result = Vector2.zero;
			if (this.pAttr.isOnGround)
			{
				if (num <= 4f)
				{
					result = 0.8f * a * num * num;
				}
			}
			else if (num <= 8f && num > 3f)
			{
				result = 0.5f * a * num * num;
			}
			return result;
		}
		return Vector2.zero;
	}

	public void StartAssistance()
	{
		GameObject nearestAim = this.GetNearestAim(this.assisData.assistanceCheckType);
		if (nearestAim != null)
		{
			this.aimObj = nearestAim;
			this.openAssistant = true;
		}
		else
		{
			this.StopAssistance();
		}
	}

	public void StopAssistance()
	{
		this.openAssistant = false;
		this.pAction.pab.move.addSpeed = Vector2.zero;
	}

	private GameObject GetNearestAim(GamePlayerInputAssistance.InputAssistanceData.CheckType _animType)
	{
		GameObject nearestRangeEnemyWithDir = R.Enemy.GetNearestRangeEnemyWithDir(base.transform.position + new Vector3((float)(this.pAttr.faceDir * 3), 2f, 0f), 6f, 6f, this.pAttr.faceDir, true, true);
		GameObject nearestRangeEnemyWithDir2 = R.Enemy.GetNearestRangeEnemyWithDir(base.transform.position + new Vector3((float)(this.pAttr.faceDir * 3), 2f, 0f), 6f, 6f, this.pAttr.faceDir, true, false);
		switch (_animType)
		{
		case GamePlayerInputAssistance.InputAssistanceData.CheckType.GroundOnly:
			return nearestRangeEnemyWithDir;
		case GamePlayerInputAssistance.InputAssistanceData.CheckType.AirOnly:
			return nearestRangeEnemyWithDir2;
		case GamePlayerInputAssistance.InputAssistanceData.CheckType.GroundFirst:
			return (!(nearestRangeEnemyWithDir == null)) ? nearestRangeEnemyWithDir : nearestRangeEnemyWithDir2;
		case GamePlayerInputAssistance.InputAssistanceData.CheckType.AirFirst:
			return (!(nearestRangeEnemyWithDir2 == null)) ? nearestRangeEnemyWithDir2 : nearestRangeEnemyWithDir;
		case GamePlayerInputAssistance.InputAssistanceData.CheckType.DisFirst:
		{
			if (nearestRangeEnemyWithDir == null)
			{
				return nearestRangeEnemyWithDir2;
			}
			if (nearestRangeEnemyWithDir2 == null)
			{
				return nearestRangeEnemyWithDir;
			}
			float num = Vector2.Distance(base.transform.position, nearestRangeEnemyWithDir.transform.position);
			float num2 = Vector2.Distance(base.transform.position, nearestRangeEnemyWithDir2.transform.position);
			return (num >= num2) ? nearestRangeEnemyWithDir2 : nearestRangeEnemyWithDir;
		}
		default:
			return null;
		}
	}

	private void OnDrawGizmos()
	{
		Rect rect = new Rect(this.localJudgeRect);
		rect.min = this.LocalToWorldDir(rect.min);
		rect.max = this.LocalToWorldDir(rect.max);
		rect.position += (Vector2)base.transform.position;
		DebugX.DrawRect(rect, this.gizmosColor, 0f);
	}

	private Vector2 LocalToWorldDir(Vector2 loc)
	{
		Vector2 result = Vector2.Scale(loc, base.transform.localScale);
		result.Scale(new Vector2(-1f, 1f));
		return result;
	}

	private PlayerAttribute pAttr;

	private PlayerAction pAction;

	public Rect localJudgeRect = new Rect(-0.5f, -2.5f, 6f, 5f);

	public GameObject aimObj;

	private bool openAssistant;

	[SerializeField]
	private TextAsset atkParm;

	private JsonData atkData;

	private GamePlayerInputAssistance.InputAssistanceData assisData;

	private Color gizmosColor = Color.blue;

	private Color gizmosOffsetPointColor = Color.blue;

	public class InputAssistanceData
	{
		public InputAssistanceData(JsonData data)
		{
			this.assistanceType = (GamePlayerInputAssistance.InputAssistanceData.AssistanceType)data.Get<int>("assistanceType", 0);
			this.assistanceCheckType = (GamePlayerInputAssistance.InputAssistanceData.CheckType)data.Get<int>("assistanceCheckType", 0);
			this.autoTurn = data.Get<bool>("autoTurn", false);
			this.keepDis = data.Get<bool>("keepDis", false);
		}

		private InputAssistanceData()
		{
		}

		public GamePlayerInputAssistance.InputAssistanceData.AssistanceType assistanceType;

		public GamePlayerInputAssistance.InputAssistanceData.CheckType assistanceCheckType;

		public bool autoTurn;

		public bool keepDis;

		public enum AssistanceType
		{
			X,
			Y,
			XY
		}

		public enum CheckType
		{
			None,
			GroundOnly,
			AirOnly,
			GroundFirst,
			AirFirst,
			DisFirst
		}
	}
}
