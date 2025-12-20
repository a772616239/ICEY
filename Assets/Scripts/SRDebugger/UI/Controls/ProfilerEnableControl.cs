using System;
using SRDebugger.Internal;
using SRF;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class ProfilerEnableControl : SRMonoBehaviourEx
	{
		protected override void Start()
		{
			base.Start();
			if (!UnityEngine.Profiling.Profiler.supported)
			{
				this.Text.text = SRDebugStrings.Current.Profiler_NotSupported;
				this.EnableButton.gameObject.SetActive(false);
				base.enabled = false;
				return;
			}
			if (!Application.HasProLicense())
			{
				this.Text.text = SRDebugStrings.Current.Profiler_NoProInfo;
				this.EnableButton.gameObject.SetActive(false);
				base.enabled = false;
				return;
			}
			this.UpdateLabels();
		}

		protected void UpdateLabels()
		{
			if (!UnityEngine.Profiling.Profiler.enabled)
			{
				this.Text.text = SRDebugStrings.Current.Profiler_EnableProfilerInfo;
				this.ButtonText.text = "Enable";
			}
			else
			{
				this.Text.text = SRDebugStrings.Current.Profiler_DisableProfilerInfo;
				this.ButtonText.text = "Disable";
			}
			this._previousState = UnityEngine.Profiling.Profiler.enabled;
		}

		protected override void Update()
		{
			base.Update();
			if (UnityEngine.Profiling.Profiler.enabled != this._previousState)
			{
				this.UpdateLabels();
			}
		}

		public void ToggleProfiler()
		{
			UnityEngine.Debug.Log("Toggle Profiler");
            UnityEngine.Profiling.Profiler.enabled = !UnityEngine.Profiling.Profiler.enabled;
		}

		private bool _previousState;

		[RequiredField]
		public Text ButtonText;

		[RequiredField]
		public Button EnableButton;

		[RequiredField]
		public Text Text;
	}
}
