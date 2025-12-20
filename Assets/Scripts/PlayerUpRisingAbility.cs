using System;
using ExtensionMethods;
using UnityEngine;

public class PlayerUpRisingAbility : CharacterState
{
	public void UpJumpAttack()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (this.pAttr.isOnGround)
		{
			R.Player.TimeController.SetSpeed(Vector2.zero);
			if (R.Player.Enhancement.Combo1 != 0 && this.stateMachine.currentState.IsInArray(this._canUpRisingSta))
			{
				this.weapon.HandleUpRising();
			}
		}
	}

	private readonly string[] _canUpRisingSta = new string[]
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
		"AtkHv1Push",
		"EndAtk",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
		"ExecuteToIdle",
		"FlashGround"
	};
}
