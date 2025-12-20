using System;
using SRF;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Tabs
{
	public class ProfilerTabController : SRMonoBehaviourEx
	{
		protected override void Start()
		{
			base.Start();
			this.PinToggle.onValueChanged.AddListener(new UnityAction<bool>(this.PinToggleValueChanged));
			this.Refresh();
		}

		private void PinToggleValueChanged(bool isOn)
		{
			SRDebug.Instance.IsProfilerDocked = isOn;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this._isDirty = true;
		}

		protected override void Update()
		{
			base.Update();
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		private void Refresh()
		{
			this.PinToggle.isOn = SRDebug.Instance.IsProfilerDocked;
			this._isDirty = false;
		}

		private bool _isDirty;

		[RequiredField]
		public Toggle PinToggle;
	}
}
