using System;
using GameWorld;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OpenGateOnSceneInit : BaseBehaviour
{
	private void Awake()
	{
		this._animator = base.GetComponent<Animator>();
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<PassGateEventArgs>("PassGate", new EventManager.FBEventHandler<PassGateEventArgs>(this.OnPassGate), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<PassGateEventArgs>("PassGate", new EventManager.FBEventHandler<PassGateEventArgs>(this.OnPassGate), EventManager.ListenerQueue.Game);
	}

	private bool OnPassGate(string eventName, object sender, PassGateEventArgs passGateEventArgs)
	{
		if (passGateEventArgs.Status == PassGateEventArgs.PassGateStatus.Exit && this._gate != null && passGateEventArgs.MyId == this._gate.data.MyId)
		{
			this._animator.Play("Open");
		}
		return true;
	}

	[SerializeField]
	private SceneGate _gate;

	private Animator _animator;
}
