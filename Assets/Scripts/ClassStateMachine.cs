using System;
using System.Collections.Generic;
using UnityEngine;

public class ClassStateMachine
{
	public ClassStateMachine(Type type)
	{
		this.states = new Dictionary<string, ClassStateMachine.State>(5);
		this.AddState(this.emptyState);
		this._currentState = this.emptyState;
		this.AddStates(type);
	}

	public string currentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			this.SetState(value);
		}
	}

	public ClassStateMachine.State GetState(string name)
	{
		return this.states[name];
	}

	public ClassStateMachine.State GetState(Enum name)
	{
		return this.GetState(name.ToString());
	}

	public void SetState(Enum nextState)
	{
		this.SetState(nextState.ToString());
	}

	public void SetState(string nextState)
	{
		this.Info(string.Format("当前状态为{0},准备转换状态为{1}", this._currentState, nextState));
		this._SetState(nextState);
	}

	private void _SetState(string nextState)
	{
		if (this._currentState.Equals(this.emptyState))
		{
			this.StateEventHandler(this.states[nextState].OnEnter, this.states[nextState]);
			this.StateEventHandler(this.OnEnter, this.states[nextState]);
			this._currentState = nextState;
			return;
		}
		this.StateEventHandler(this.states[this._currentState].OnExit, this.states[this._currentState]);
		this.StateEventHandler(this.OnExit, this.states[this._currentState]);
		ClassStateMachine.TransferEventArgs transferEventArgs = new ClassStateMachine.TransferEventArgs();
		transferEventArgs.lastState = this._currentState;
		transferEventArgs.nextState = nextState;
		EventHandler<ClassStateMachine.TransferEventArgs> onTransfer = this.OnTransfer;
		if (onTransfer != null)
		{
			onTransfer(this, transferEventArgs);
		}
		this.StateEventHandler(this.states[nextState].OnEnter, this.states[nextState]);
		this.StateEventHandler(this.OnEnter, this.states[nextState]);
		this._currentState = nextState;
		this.changedState = true;
	}

	private void StateEventHandler(EventHandler<ClassStateMachine.StateEventArgs> _handler, ClassStateMachine.State state)
	{
		if (_handler != null)
		{
			_handler(this, new ClassStateMachine.StateEventArgs
			{
				state = state.name
			});
		}
	}

	private ClassStateMachine.State AddState(string name)
	{
		if (this.states.ContainsKey(name))
		{
			Log.Error("在状态机中添加了同名方法" + name);
		}
		ClassStateMachine.State state = new ClassStateMachine.State();
		state.name = name;
		this.states[name] = state;
		return state;
	}

	private ClassStateMachine.State AddState(Enum state)
	{
		return this.AddState(state.ToString());
	}

	private void AddStates(string[] names)
	{
		foreach (string name in names)
		{
			this.AddState(name);
		}
	}

	private void AddStates(Type type)
	{
		this.AddStates(Enum.GetNames(type));
	}

	private void Info(object obj)
	{
		if (this._logLevel <= Log.LogLevel.Info)
		{
			Log.Info(obj);
		}
	}

	private readonly string emptyState = "__Empty__";

	[SerializeField]
	private string _currentState;

	private bool changedState;

	private Enum nextState;

	public EventHandler<ClassStateMachine.StateEventArgs> OnEnter;

	public EventHandler<ClassStateMachine.StateEventArgs> OnExit;

	public EventHandler<ClassStateMachine.TransferEventArgs> OnTransfer;

	public Dictionary<string, ClassStateMachine.State> states;

	private Log.LogLevel _logLevel = Log.LogLevel.Warning;

	public class State
	{
		public string name;

		public EventHandler<ClassStateMachine.StateEventArgs> OnEnter;

		public EventHandler<ClassStateMachine.StateEventArgs> OnExit;
	}

	public class TransferEventArgs : EventArgs
	{
		public string lastState;

		public string nextState;
	}

	public class StateEventArgs : EventArgs
	{
		public string state;
	}
}
