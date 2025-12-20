using System;
using System.Collections;
using SRF;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class ProfilerMemoryBlock : SRMonoBehaviourEx
	{
		protected override void OnEnable()
		{
			base.OnEnable();
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
			long totalReservedMemoryLong = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
			long totalAllocatedMemoryLong = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
			long num = totalReservedMemoryLong >> 10;
			num /= 1024L;
			long num2 = totalAllocatedMemoryLong >> 10;
			num2 /= 1024L;
			this.Slider.maxValue = (float)num;
			this.Slider.value = (float)num2;
			this.TotalAllocatedText.text = "Reserved: <color=#FFFFFF>{0}</color>MB".Fmt(new object[]
			{
				num
			});
			this.CurrentUsedText.text = "<color=#FFFFFF>{0}</color>MB".Fmt(new object[]
			{
				num2
			});
		}

		public void TriggerCleanup()
		{
			base.StartCoroutine(this.CleanUp());
		}

		private IEnumerator CleanUp()
		{
			GC.Collect();
			yield return Resources.UnloadUnusedAssets();
			GC.Collect();
			this.TriggerRefresh();
			yield break;
		}

		private float _lastRefresh;

		[RequiredField]
		public Text CurrentUsedText;

		[RequiredField]
		public Slider Slider;

		[RequiredField]
		public Text TotalAllocatedText;
	}
}
