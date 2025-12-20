using System;
using SRF;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class ProfilerMonoBlock : SRMonoBehaviourEx
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			this._isSupported = (UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() > 0L);
			this.NotSupportedMessage.SetActive(!this._isSupported);
			this.CurrentUsedText.gameObject.SetActive(this._isSupported);
			this.TriggerRefresh();
		}

		protected override void Update()
		{
			base.Update();
			if (SRDebug.Instance.IsDebugPanelVisible && Time.realtimeSinceStartup - this._lastRefresh > 1f)
			{
				this.TriggerRefresh();
				this._lastRefresh = Time.realtimeSinceStartup;
			}
		}

		public void TriggerRefresh()
		{
			long num = (!this._isSupported) ? GC.GetTotalMemory(false) : UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong();
			long monoUsedSizeLong = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
			long num2 = num >> 10;
			num2 /= 1024L;
			long num3 = monoUsedSizeLong >> 10;
			num3 /= 1024L;
			this.Slider.maxValue = (float)num2;
			this.Slider.value = (float)num3;
			this.TotalAllocatedText.text = "Total: <color=#FFFFFF>{0}</color>MB".Fmt(new object[]
			{
				num2
			});
			if (num3 > 0L)
			{
				this.CurrentUsedText.text = "<color=#FFFFFF>{0}</color>MB".Fmt(new object[]
				{
					num3
				});
			}
		}

		public void TriggerCollection()
		{
			GC.Collect();
			this.TriggerRefresh();
		}

		private float _lastRefresh;

		[RequiredField]
		public Text CurrentUsedText;

		[RequiredField]
		public GameObject NotSupportedMessage;

		[RequiredField]
		public Slider Slider;

		[RequiredField]
		public Text TotalAllocatedText;

		private bool _isSupported;
	}
}
