using System;
using SRDebugger.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	[RequireComponent(typeof(ScrollRect))]
	public class ScrollSettingsBehaviour : MonoBehaviour
	{
		private void Awake()
		{
			ScrollRect component = base.GetComponent<ScrollRect>();
			component.scrollSensitivity = 40f;
			if (!SRDebuggerUtil.IsMobilePlatform)
			{
				component.movementType = ScrollRect.MovementType.Clamped;
				component.inertia = false;
			}
		}

		public const float ScrollSensitivity = 40f;
	}
}
