using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public class PlayerHitGroundAbility : CharacterState
{
	public override void Start()
	{
		this._atkBox = this.pac.GetComponentInChildren<PlayerAtk>().transform;
		this._platform = R.Player.GetComponent<PlatformMovement>();
	}

	public override void Update()
	{
		if (this.pAttr.isDead)
		{
			return;
		}
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (this.listener.checkHitGround && this.pAttr.isOnGround)
		{
			R.Player.TimeController.SetSpeed(Vector2.zero);
			this.listener.PhysicReset();
			this.listener.checkHitGround = false;
			Vector3 position = new Vector3(0f, this._platform.GetDistanceToGround(), 0f);
			R.Effect.Generate(22, this.pac.transform, position, Vector3.zero, default(Vector3), true);
			string currentState = this.stateMachine.currentState;
			switch (currentState)
			{
			case "Roll":
			case "DahalRoll":
				this._atkBox.localScale = Vector2.zero;
				this.pac.ChangeState(PlayerAction.StateEnum.RollEnd, 1f);
				break;
			case "HitGround":
				this.pac.ChangeState(PlayerAction.StateEnum.HitGround2, 1f);
				break;
			case "RollEndFrame":
				this.pac.ChangeState(PlayerAction.StateEnum.RollFrameEnd, 1f);
				break;
			case "QTEHitGround":
				this.pac.ChangeState(PlayerAction.StateEnum.QTEHitGround2, 1f);
				break;
			case "NewExecuteAir1_2":
				SingletonMono<CameraController>.Instance.KillTweening();
				SingletonMono<CameraController>.Instance.CloseMotionBlur();
				base.StartCoroutine(this.ExecuteHitGround());
				break;
			case "QTERoll":
				this.pac.ChangeState(PlayerAction.StateEnum.QTERollEnd, 1f);
				break;
			}
		}
	}

	private IEnumerator ExecuteHitGround()
	{
		yield return new WaitForSeconds(0.05f);
		this.listener.StartExecute();
		yield break;
	}

	public void HitGround()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (!this.pAttr.isOnGround && this.stateMachine.currentState.IsInArray(PlayerHitGroundAbility.CanHitGroundSta) && R.Player.Enhancement.HitGround != 0)
		{
			this.listener.StopIEnumerator("FlashPositionSet");
			if (this.pac.tempDir != 3)
			{
				this.pac.TurnRound(this.pac.tempDir);
			}
			this.weapon.HandleHitGround();
		}
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.nextState == "Roll")
		{
			Vector2 speed = new Vector2((float)(4 * this.pAttr.faceDir), 0f);
			this.listener.AirPhysic(0.8f);
			R.Player.TimeController.SetSpeed(speed);
		}
	}

	private static readonly string[] CanHitGroundSta = new string[]
	{
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2",
		"Flash1",
		"FlashDown1",
		"FlashUp1",
		"UpRising",
		"Jump",
		"Jump2",
		"RollJump",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
		"AirAtk1",
		"AirAtk2",
		"AirAtk3",
		"AirAtk4",
		"AirAtk6",
		"AirAtkHv1",
		"AirAtkHv2",
		"AirAtkHv3",
		"AirAtkHv4",
		"AirAtkHv5",
		"AirAtkHv1Push",
		"RollReady",
		"Roll",
		"Execute2ToFall"
	};

	private Transform _atkBox;

	private PlatformMovement _platform;
}
