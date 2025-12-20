using System;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	[RequireComponent(typeof(RectTransform))]
	public class ProfilerGraphAxisLabel : SRMonoBehaviourEx
	{
		protected override void Update()
		{
			base.Update();
			if (this._queuedFrameTime != null)
			{
				this.SetValueInternal(this._queuedFrameTime.Value);
				this._queuedFrameTime = null;
			}
		}

		public void SetValue(float frameTime, float yPosition)
		{
			if (this._prevFrameTime == frameTime && this._yPosition == yPosition)
			{
				return;
			}
			this._queuedFrameTime = new float?(frameTime);
			this._yPosition = yPosition;
		}

		private void SetValueInternal(float frameTime)
		{
			this._prevFrameTime = frameTime;
			int num = Mathf.FloorToInt(frameTime * 1000f);
			int num2 = Mathf.RoundToInt(1f / frameTime);
			this.Text.text = "{0}ms ({1}FPS)".Fmt(new object[]
			{
				num,
				num2
			});
			RectTransform rectTransform = (RectTransform)base.CachedTransform;
			rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width * 0.5f + 10f, this._yPosition);
		}

		private float _prevFrameTime;

		private float? _queuedFrameTime;

		private float _yPosition;

		[RequiredField]
		public Text Text;
	}
}
