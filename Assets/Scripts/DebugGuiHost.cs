using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugGuiHost : MonoBehaviour
{
	public static void Register(DebugGuiBase debugGui)
	{
		DebugGuiHost._debugGuis.Add(debugGui);
	}

	public static void Unregister(DebugGuiBase debugGui)
	{
		DebugGuiHost._debugGuis.Remove(debugGui);
	}

	private void OnGUI()
	{
		foreach (DebugGuiBase debugGuiBase in DebugGuiHost._debugGuis)
		{
			if (debugGuiBase.InVisible)
			{
				debugGuiBase.OnDebugGUI();
			}
		}
	}

	private static List<DebugGuiBase> _debugGuis = new List<DebugGuiBase>();
}
