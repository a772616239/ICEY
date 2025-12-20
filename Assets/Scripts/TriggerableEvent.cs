using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerableEvent : MonoBehaviour
{
	public void OnActive(int index)
	{
		this.events.Invoke();
	}

	public UnityEvent events;
}
