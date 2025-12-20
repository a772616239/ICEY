using System;
using SRF;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class VersionTextBehaviour : SRMonoBehaviourEx
	{
		protected override void Start()
		{
			base.Start();
			this.Text.text = string.Format(this.Format, "1.6.2");
		}

		public string Format = "SRDebugger {0}";

		[RequiredField]
		public Text Text;
	}
}
