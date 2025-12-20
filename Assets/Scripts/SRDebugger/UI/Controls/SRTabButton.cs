using System;
using SRF;
using SRF.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class SRTabButton : SRMonoBehaviourEx
	{
		public bool IsActive
		{
			get
			{
				return this.ActiveToggle.enabled;
			}
			set
			{
				this.ActiveToggle.enabled = value;
			}
		}

		[RequiredField]
		public Behaviour ActiveToggle;

		[RequiredField]
		public Button Button;

		[RequiredField]
		public RectTransform ExtraContentContainer;

		[RequiredField]
		public StyleComponent IconStyleComponent;

		[RequiredField]
		public Text TitleText;
	}
}
