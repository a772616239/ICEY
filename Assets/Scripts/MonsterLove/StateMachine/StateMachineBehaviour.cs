using System;
using UnityEngine;

namespace MonsterLove.StateMachine
{
	[RequireComponent(typeof(StateMachineEngine))]
	public class StateMachineBehaviour : MonoBehaviour
	{
		public StateMachineEngine stateMachine
		{
			get
			{
				if (this._stateMachine == null)
				{
					this._stateMachine = base.GetComponent<StateMachineEngine>();
				}
				if (this._stateMachine == null)
				{
					throw new Exception("Please make sure StateMachineEngine is also present on any StateMachineBehaviour objects");
				}
				return this._stateMachine;
			}
		}

		public void PauseStateMachine(bool isPause)
		{
			this.stateMachine.PauseStateMachine(isPause);
		}

		public Enum GetState()
		{
			return this.stateMachine.GetState();
		}

		protected void Initialize<T>()
		{
			this.stateMachine.Initialize<T>(this);
		}

		protected void ChangeState(Enum newState, object userData = null)
		{
			this.stateMachine.ChangeState(newState, userData);
		}

		private StateMachineEngine _stateMachine;
	}
}
