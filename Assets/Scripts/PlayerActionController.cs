using System;
using UnityEngine;

public class PlayerActionController : BaseBehaviour
{
	private void Awake()
	{
		this._playerAbilities = base.GetComponent<PlayerAbilities>();
		this._playerAction = base.GetComponent<PlayerAction>();
		this._stateMachine = base.GetComponent<StateMachine>();
	}

	public void ChangeState(PlayerAction.StateEnum nextState)
	{
		this._stateMachine.SetState(nextState);
	}

	public void TurnRound(int dir)
	{
		this._playerAction.TurnRound(dir);
	}

	public void StartMove()
	{
		this._move = true;
	}

	public void StopMove()
	{
		this._move = false;
	}

	public void Jump()
	{
		this._playerAbilities.jump.Jump();
	}

	public void Flash()
	{
		this._playerAbilities.flash.FlashOnce();
	}

	private void Update()
	{
		if (this.openPos)
		{
			base.transform.position = this.position;
		}
		if (this._move)
		{
			this._playerAbilities.move.Move(R.Player.Attribute.faceDir);
		}
	}

	private PlayerAction _playerAction;

	private PlayerAbilities _playerAbilities;

	private StateMachine _stateMachine;

	public Vector3 position;

	public bool openPos;

	private bool _move;
}
