using System;
using UnityEngine;

public abstract class DebugGuiBase : MonoBehaviour
{
	private void Start()
	{
		this._id = UIDebug.Register(this.UIDebugButtonName, new EventDelegate.Callback(this.OpenDebugUI));
		DebugGuiHost.Register(this);
		this.GUIStart();
	}

	private void OnDestroy()
	{
		UIDebug.Unregister(this._id);
		DebugGuiHost.Unregister(this);
	}

	private void OpenDebugUI()
	{
		this.InVisible = !this.InVisible;
	}

	protected abstract string UIDebugButtonName { get; }

	public abstract void OnDebugGUI();

	protected abstract void GUIStart();

	[Header("是否开启，可以通过设置该值默认开启")]
	[SerializeField]
	public bool InVisible;

	private int _id;
}
