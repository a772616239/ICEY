using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class ConsoleTabQuickViewControl : SRMonoBehaviourEx
	{
		protected override void Awake()
		{
			base.Awake();
			this.ErrorCountText.text = "0";
			this.WarningCountText.text = "0";
			this.InfoCountText.text = "0";
		}

		protected override void Update()
		{
			base.Update();
			if (this.ConsoleService == null)
			{
				return;
			}
			if (ConsoleTabQuickViewControl.HasChanged(this.ConsoleService.ErrorCount, ref this._prevErrorCount, 1000))
			{
				this.ErrorCountText.text = SRDebuggerUtil.GetNumberString(this.ConsoleService.ErrorCount, 1000, ConsoleTabQuickViewControl.MaxString);
			}
			if (ConsoleTabQuickViewControl.HasChanged(this.ConsoleService.WarningCount, ref this._prevWarningCount, 1000))
			{
				this.WarningCountText.text = SRDebuggerUtil.GetNumberString(this.ConsoleService.WarningCount, 1000, ConsoleTabQuickViewControl.MaxString);
			}
			if (ConsoleTabQuickViewControl.HasChanged(this.ConsoleService.InfoCount, ref this._prevInfoCount, 1000))
			{
				this.InfoCountText.text = SRDebuggerUtil.GetNumberString(this.ConsoleService.InfoCount, 1000, ConsoleTabQuickViewControl.MaxString);
			}
		}

		private static bool HasChanged(int newCount, ref int oldCount, int max)
		{
			int num = Mathf.Clamp(newCount, 0, max);
			int num2 = Mathf.Clamp(oldCount, 0, max);
			bool result = num != num2;
			oldCount = newCount;
			return result;
		}

		private const int Max = 1000;

		private static readonly string MaxString = 999 + "+";

		private int _prevErrorCount = -1;

		private int _prevInfoCount = -1;

		private int _prevWarningCount = -1;

		[Import]
		public IConsoleService ConsoleService;

		[RequiredField]
		public Text ErrorCountText;

		[RequiredField]
		public Text InfoCountText;

		[RequiredField]
		public Text WarningCountText;
	}
}
