using System;
using Core;
using ExtensionMethods;
using GameWorld;
using UnityEngine;

public class PlayerMoveAbility : CharacterState
{
	public override void Update()
	{
		this.moveStopCounter--;
		if (this.moveStopCounter == 0)
		{
			this.Move(0);
		}
	}

	public override void FixedUpdate()
	{
		if ((this.stateMachine.currentState.IsInArray(PlayerAction.JumpSta) || this.stateMachine.currentState.IsInArray(PlayerAction.FlySta)) && !this.pAttr.isOnGround)
		{
			this.DealAirFric(this.pAttr.moveSpeed, true);
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.UpRisingSta) && !this.pAttr.isOnGround)
		{
			this.DealAirFric(this.pAttr.moveSpeed / 2f, false);
		}
		if (this.stateMachine.currentState == "BladeStorm")
		{
			this.DealAirFric(this.pAttr.moveSpeed / 4f, false);
		}
	}

	private void DealAirFric(float maxSpeed, bool canTurn)
	{
		Vector2 currentSpeed = R.Player.TimeController.GetCurrentSpeed();
		float num = currentSpeed.x;
		num = Mathf.Clamp(Mathf.Abs(num) - this.airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(num);
		if (Core.Input.Game.MoveLeft.Pressed || Core.Input.Game.MoveRight.Pressed)
		{
			int num2 = (!Core.Input.Game.MoveLeft.Pressed) ? 1 : -1;
			int num3 = (!Core.Input.Game.MoveLeft.Pressed) ? 1 : -1;
			if (num2 != this.pAttr.faceDir && canTurn)
			{
				this.pac.TurnRound(num2);
			}
			num += (float)num3 * this.extraFric * Time.fixedDeltaTime;
		}
		currentSpeed.x = Mathf.Clamp(Mathf.Abs(num) - this.airFric * Time.fixedDeltaTime, 0f, maxSpeed) * Mathf.Sign(num);
		if (!this.pAttr.isOnGround)
		{
			Collider2D[] array = Physics2D.OverlapAreaAll(this.pac.transform.position + new Vector3(0.5f * (float)this.pAttr.faceDir, 0f, 0f), this.pac.transform.position + new Vector3(0.6f * (float)this.pAttr.faceDir, 2.2f, 0f), LayerManager.WallMask);
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.layer == LayerManager.WallLayerID || array[i].gameObject.layer == LayerManager.GroundLayerID)
				{
					num4++;
				}
			}
			bool flag = num4 > 0;
			if (flag)
			{
				currentSpeed.x = Mathf.Sign(currentSpeed.x) * -1f * 0.1f;
			}
			num4 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].gameObject.layer == LayerManager.CeilingLayerID)
				{
					num4++;
				}
			}
			bool flag2 = num4 > 0;
			if (flag2 && currentSpeed.y > 0f)
			{
				currentSpeed.y = 0f;
			}
		}
		R.Player.TimeController.SetSpeed(currentSpeed);
	}

	public void Move(int dir)
	{
		if (this.pAttr.isDead)
		{
			return;
		}
		if (dir == 0)
		{
			this.pac.tempDir = 3;
			if (this.stateMachine.currentState == PlayerAction.CanRunSlow)
			{
				R.Player.TimeController.SetSpeed(Vector2.zero);
				this.pac.ChangeState(PlayerAction.StateEnum.RunSlow, 1f);
			}
			if (this.stateMachine.currentState.IsInArray(PlayerAction.AttackSta))
			{
				R.Player.TimeController.SetSpeed(Vector2.zero);
			}
		}
		else
		{
			this.pac.tempDir = dir;
			if (this.stateMachine.currentState.IsInArray(PlayerAction.NormalSta) || this.pac.canChangeAnim)
			{
				this.PlayerMove(dir == -1, this.pAttr.moveSpeed, Vector2.zero, true);
			}
			if (this.stateMachine.currentState.IsInArray(PlayerMoveAbility.AttackSta) && dir == this.pAttr.faceDir)
			{
				this.PlayerMove(dir == -1, this.pAttr.moveSpeed / 4f, this.addSpeed, false);
			}
			if ((this.stateMachine.currentState.IsInArray(PlayerAction.AirLightAttackSta) || this.stateMachine.currentState == "AirAtkHv1" || this.stateMachine.currentState == "AirAtkHv2") && dir == this.pAttr.faceDir)
			{
				this.PlayerMove(dir == -1, this.pAttr.moveSpeed / 4f, this.addSpeed, false);
			}
			if (this.stateMachine.currentState == "BladeStorm")
			{
				this.PlayerMove(dir == -1, this.pAttr.moveSpeed / 2f, Vector2.zero, false);
			}
		}
	}

	private void PlayerMove(bool isLeft, float walkSpeed, Vector2 aSpeed, bool playRun = true)
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		walkSpeed = this.AirWallCheck(walkSpeed);
		int num = (!isLeft) ? 1 : -1;
		Vector2 vector = new Vector2(walkSpeed * (float)num, R.Player.TimeController.GetCurrentSpeed().y);
		vector += aSpeed;
		vector = this.EdgeCheck(vector);
		vector = this.SlopeCheck(vector);
		R.Player.TimeController.SetSpeed(vector);
		if (playRun)
		{
			this.pac.TurnRound((!isLeft) ? 1 : -1);
			this.pac.ChangeState(PlayerAction.StateEnum.Run, 1f);
		}
		this.moveStopCounter = 4;
	}

	private float AirWallCheck(float walkSpeed)
	{
		if (!this.pAttr.isOnGround && Physics2D.OverlapAreaAll(this.pac.transform.position + new Vector3(0.5f * (float)this.pAttr.faceDir, 0f, 0f), this.pac.transform.position + new Vector3(0.6f * (float)this.pAttr.faceDir, 2.2f, 0f), LayerManager.GroundMask).Length > 0)
		{
			walkSpeed = 0f;
		}
		return walkSpeed;
	}

	private Vector2 EdgeCheck(Vector2 speed)
	{
		Vector3 position = this.pac.transform.position;
		if (position.x >= GameArea.PlayerRange.max.x - this.pAttr.bounds.size.x / 2f)
		{
			speed.x = ((speed.x <= 0f) ? speed.x : 0f);
		}
		if (position.x <= GameArea.PlayerRange.min.x + this.pAttr.bounds.size.x / 2f)
		{
			speed.x = ((speed.x >= 0f) ? speed.x : 0f);
		}
		return speed;
	}

	private Vector2 SlopeCheck(Vector2 speed)
	{
		if (this.pac.IsInNormalState())
		{
			PlatformMovement component = this.pac.GetComponent<PlatformMovement>();
			Vector2 groundNormal = component.GetGroundNormal();
			Vector2 vector = Vector3.ProjectOnPlane(speed, groundNormal);
			float d = Mathf.Clamp(Mathf.Abs(vector.x), Mathf.Abs(speed.x / 2f), Mathf.Abs(speed.x));
			if (speed.y > 0f)
			{
				speed = vector.normalized * d;
			}
			else
			{
				speed = vector;
			}
		}
		return speed;
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.nextState.IsInArray(PlayerAction.NormalSta) && !args.lastState.IsInArray(PlayerAction.NormalSta))
		{
			EventManager.PostEvent<PlayerMoveAbility, AssessmentEventArgs>("Assessment", this, new AssessmentEventArgs(AssessmentEventArgs.EventType.CurrentComboFinish));
		}
	}

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk5",
		"Atk6",
		"Atk7",
		"Atk8",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk15",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"AtkHv1Push",
		"AtkRollReady",
		"AtkRollEnd"
	};

	private const int maxMoveStopCount = 4;

	private int moveStopCounter;

	private float airFric = 8f;

	private float extraFric = 60f;

	public Vector2 addSpeed = Vector2.zero;
}
