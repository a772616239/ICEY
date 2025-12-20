using System;
using UnityEngine;

public class BattleZoneGateTools : BaseBehaviour
{
	public void GateAppear()
	{
		this.gate.Appear();
	}

	public void GateDisappear()
	{
		this.gate.DisAppear();
	}

	[SerializeField]
	private BattleZoneGate gate;
}
