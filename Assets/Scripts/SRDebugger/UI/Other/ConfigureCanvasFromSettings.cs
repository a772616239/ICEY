using System;
using SRDebugger.Internal;
using SRF;
using UnityEngine;

namespace SRDebugger.UI.Other
{
	[RequireComponent(typeof(Canvas))]
	public class ConfigureCanvasFromSettings : SRMonoBehaviour
	{
		private void Start()
		{
			Canvas component = base.GetComponent<Canvas>();
			SRDebuggerUtil.ConfigureCanvas(component);
			UnityEngine.Object.Destroy(this);
		}
	}
}
