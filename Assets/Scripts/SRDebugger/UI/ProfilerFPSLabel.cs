using System;
using SRDebugger.Services;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI
{
	public class ProfilerFPSLabel : SRMonoBehaviourEx
	{
		protected override void Update()
		{
			base.Update();
			if (Time.realtimeSinceStartup > this._nextUpdate)
			{
				this.Refresh();
			}
		}

		private void Refresh()
		{
			this._text.text = "FPS: {0:0.00}".Fmt(new object[]
			{
				1f / this._profilerService.AverageFrameTime
			});
			this._nextUpdate = Time.realtimeSinceStartup + this.UpdateFrequency;
		}

		private float _nextUpdate;

		[Import]
		private IProfilerService _profilerService;

		public float UpdateFrequency = 1f;

		[RequiredField]
		[SerializeField]
		private Text _text;
	}
}
