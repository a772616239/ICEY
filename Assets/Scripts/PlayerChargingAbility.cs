using System;
using Core;
using ExtensionMethods;
using UnityEngine;

public class PlayerChargingAbility : CharacterState
{
	public override void Update()
	{
		if (this.pAttr.isInCharging && Core.Input.Game.Atk.OnReleased)
		{
			bool flag = this.stateMachine.currentState == "AirCharging";
			if (this.weapon.canChargeAttack)
			{
				this.pac.TurnRound(this.pac.tempDir);
				this.ReleaseCharge(flag);
			}
			else
			{
				this.pac.ChangeState((!flag) ? PlayerAction.StateEnum.EndAtk : PlayerAction.StateEnum.Fall1, 1f);
				this.CancelCharge();
			}
		}
	}

	public void Charging()
	{
		if (this.stateMachine.currentState.IsInArray(PlayerChargingAbility.CanChargeSta) && R.Player.Enhancement.Charging != 0 && ChipManager.HasChipInScene())
		{
			if (this.pAttr.isOnGround)
			{
				this.StartChargeGround();
			}
			else
			{
				this.StartChargeInAir();
			}
		}
	}

	private void StartChargeGround()
	{
		if (!this._chargeReset)
		{
			return;
		}
		R.Player.TimeController.SetSpeed(Vector2.zero);
		this.weapon.StartCharge(false);
		this._chargeReset = false;
	}

	private void StartChargeInAir()
	{
		if (!this._chargeReset)
		{
			return;
		}
		R.Player.TimeController.SetSpeed(Vector2.zero);
		this.listener.AirPhysic(0f);
		this.weapon.StartCharge(true);
		this._chargeReset = false;
	}

	public void CancelCharge()
	{
		this.ChargeReset();
		this.weapon.ChargeCancel();
	}

	public void ChargeReset()
	{
		this._chargeReset = true;
	}

	private void ReleaseCharge(bool inAir)
	{
		this.weapon.ReleaseCharge(inAir);
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.lastState.IsInArray(PlayerAction.ChargeSta) && !args.nextState.IsInArray(PlayerAction.ChargeSta))
		{
			this.CancelCharge();
			this.pac.absorbNum = 0;
		}
	}

	public override void Start()
	{
		this.ChargeReset();
	}

	private bool _chargeReset;

	private static readonly string[] CanChargeSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk6",
		"Atk7",
		"Atk8",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk23",
		"Atk15",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"Atk16",
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
		"AirAtkRollReady",
		"AirAtkRoll",
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2",
		"AtkHv1Push",
		"Flash1",
		"FlashDown1",
		"FlashUp1",
		"HitGround",
		"HitGround2",
		"RollReady",
		"Roll",
		"RollEnd",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
		"EndAtk",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow",
		"ExecuteToIdle",
		"Execute2ToFall",
		"FlashGround"
	};
}
