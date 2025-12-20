using System;
using System.Collections;
using System.Collections.Generic;
using CIS;
using Core;
using UnityEngine;

public class InputMgr : SingletonMonoBehaviourClass<InputMgr>
{
	public bool IsChannelOpen(IInputEventHandler handler)
	{
		return this.channel.ContainsKey(handler) && (bool)this.channel[handler];
	}

	public void RegisterInputEventHandler(IInputEventHandler handler)
	{
		this.handlers.Add(handler);
		if (!this.channel.ContainsKey(handler))
		{
			this.channel[handler] = true;
		}
	}

	public void UnRegisterInputEvent(IInputEventHandler handler)
	{
		for (int i = 0; i < this.handlers.Count; i++)
		{
			if (this.handlers[i] == handler)
			{
				this.handlers.RemoveAt(i);
				break;
			}
		}
		if (!this.channel.ContainsKey(handler))
		{
			this.channel.Remove(handler);
		}
	}

	public void EnableInput(bool enable)
	{
		this.enableInput = enable;
	}

	public void EnableInput(IInputEventHandler handler, bool enable)
	{
		this.channel[handler] = enable;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.N))
		{
			IDictionaryEnumerator enumerator = this.channel.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"key : ",
						dictionaryEntry.Key,
						" value: ",
						dictionaryEntry.Value
					}));
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		if (Core.Input.Shi.Left.OnPressed)
		{
			foreach (IInputEventHandler inputEventHandler in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler])
				{
					inputEventHandler.KeyDown(KeyCode.A);
				}
			}
		}
		if (Core.Input.Shi.Left.OnReleased)
		{
			foreach (IInputEventHandler inputEventHandler2 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler2])
				{
					inputEventHandler2.KeyUp(KeyCode.A);
				}
			}
		}
		if (Core.Input.Shi.Right.OnPressed)
		{
			foreach (IInputEventHandler inputEventHandler3 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler3])
				{
					inputEventHandler3.KeyDown(KeyCode.D);
				}
			}
		}
		if (Core.Input.Shi.Right.OnReleased)
		{
			foreach (IInputEventHandler inputEventHandler4 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler4])
				{
					inputEventHandler4.KeyUp(KeyCode.D);
				}
			}
		}
		if (Core.Input.Shi.Up.OnPressed)
		{
			foreach (IInputEventHandler inputEventHandler5 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler5])
				{
					inputEventHandler5.KeyDown(KeyCode.W);
				}
			}
		}
		if (Core.Input.Shi.Up.OnReleased)
		{
			foreach (IInputEventHandler inputEventHandler6 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler6])
				{
					inputEventHandler6.KeyUp(KeyCode.W);
				}
			}
		}
		if (Core.Input.Shi.Down.OnPressed)
		{
			foreach (IInputEventHandler inputEventHandler7 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler7])
				{
					inputEventHandler7.KeyDown(KeyCode.S);
				}
			}
		}
		if (Core.Input.Shi.Down.OnReleased)
		{
			foreach (IInputEventHandler inputEventHandler8 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler8])
				{
					inputEventHandler8.KeyUp(KeyCode.S);
				}
			}
		}
		if (Core.Input.Shi.Jump.OnPressed)
		{
			foreach (IInputEventHandler inputEventHandler9 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler9])
				{
					inputEventHandler9.KeyDown(KeyCode.J);
				}
			}
		}
		if (Core.Input.Shi.Jump.OnReleased)
		{
			foreach (IInputEventHandler inputEventHandler10 in this.handlers)
			{
				if ((bool)this.channel[inputEventHandler10])
				{
					inputEventHandler10.KeyUp(KeyCode.J);
				}
			}
		}
	}

	private List<IInputEventHandler> handlers = new List<IInputEventHandler>();

	private Hashtable channel = new Hashtable();

	private bool enableInput = true;
}
