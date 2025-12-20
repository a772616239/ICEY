using System;
using UnityEngine;

public class UICallback : MonoBehaviour
{
	public void OnCall()
	{
		ICallBackable[] components = base.GetComponents<ICallBackable>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].OnCall();
		}
	}
}
