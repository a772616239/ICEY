using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRF;
using SRF.UI;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	[RequireComponent(typeof(RectTransform))]
	public class ConsoleEntryView : SRMonoBehaviourEx, IVirtualView
	{
		public void SetDataContext(object data)
		{
			ConsoleEntry consoleEntry = data as ConsoleEntry;
			if (consoleEntry == null)
			{
				throw new Exception("Data should be a ConsoleEntry");
			}
			if (consoleEntry.Count > 1)
			{
				if (!this._hasCount)
				{
					this.CountContainer.alpha = 1f;
					this._hasCount = true;
				}
				if (consoleEntry.Count != this._count)
				{
					this.Count.text = SRDebuggerUtil.GetNumberString(consoleEntry.Count, 999, "999+");
					this._count = consoleEntry.Count;
				}
			}
			else if (this._hasCount)
			{
				this.CountContainer.alpha = 0f;
				this._hasCount = false;
			}
			if (consoleEntry == this._prevData)
			{
				return;
			}
			this._prevData = consoleEntry;
			this.Message.text = consoleEntry.MessagePreview;
			this.StackTrace.text = consoleEntry.StackTracePreview;
			if (string.IsNullOrEmpty(this.StackTrace.text))
			{
				this.Message.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 2f, this._rectTransform.rect.height - 4f);
			}
			else
			{
				this.Message.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 12f, this._rectTransform.rect.height - 14f);
			}
			switch (consoleEntry.LogType)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				this.ImageStyle.StyleKey = "Console_Error_Blob";
				break;
			case LogType.Warning:
				this.ImageStyle.StyleKey = "Console_Warning_Blob";
				break;
			case LogType.Log:
				this.ImageStyle.StyleKey = "Console_Info_Blob";
				break;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = (base.CachedTransform as RectTransform);
			this.CountContainer.alpha = 0f;
			this.Message.supportRichText = Settings.Instance.RichTextInConsole;
		}

		public const string ConsoleBlobInfo = "Console_Info_Blob";

		public const string ConsoleBlobWarning = "Console_Warning_Blob";

		public const string ConsoleBlobError = "Console_Error_Blob";

		private int _count;

		private bool _hasCount;

		private ConsoleEntry _prevData;

		private RectTransform _rectTransform;

		[RequiredField]
		public Text Count;

		[RequiredField]
		public CanvasGroup CountContainer;

		[RequiredField]
		public StyleComponent ImageStyle;

		[RequiredField]
		public Text Message;

		[RequiredField]
		public Text StackTrace;
	}
}
