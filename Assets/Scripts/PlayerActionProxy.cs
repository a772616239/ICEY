using System;
using UnityEngine;

public class PlayerActionProxy : ControllerProxyBase<PlayerActionController>
{
	public void ChangeState(PlayerAction.StateEnum nextState)
	{
		base.effect.ChangeState(nextState);
	}

	public void TurnRound(int dir)
	{
		base.effect.TurnRound(dir);
	}

	public void StartMove()
	{
		base.effect.StartMove();
	}

	public void StopMove()
	{
		base.effect.StopMove();
	}

	private void Update()
	{
		base.effect.openPos = this.openPos;
		base.effect.position = this.position;
	}

	public Vector3 position;

	public bool openPos;
}
