using System;
using UnityEngine;

namespace SRDebugger.Services
{
	public class ConsoleEntry
	{
		public ConsoleEntry()
		{
		}

		public ConsoleEntry(ConsoleEntry other)
		{
			this.Message = other.Message;
			this.StackTrace = other.StackTrace;
			this.LogType = other.LogType;
			this.Count = other.Count;
		}

		public string MessagePreview
		{
			get
			{
				if (this._messagePreview != null)
				{
					return this._messagePreview;
				}
				if (string.IsNullOrEmpty(this.Message))
				{
					return string.Empty;
				}
				this._messagePreview = this.Message.Split(new char[]
				{
					'\n'
				})[0];
				this._messagePreview = this._messagePreview.Substring(0, Mathf.Min(this._messagePreview.Length, 180));
				return this._messagePreview;
			}
		}

		public string StackTracePreview
		{
			get
			{
				if (this._stackTracePreview != null)
				{
					return this._stackTracePreview;
				}
				if (string.IsNullOrEmpty(this.StackTrace))
				{
					return string.Empty;
				}
				this._stackTracePreview = this.StackTrace.Split(new char[]
				{
					'\n'
				})[0];
				this._stackTracePreview = this._stackTracePreview.Substring(0, Mathf.Min(this._stackTracePreview.Length, 120));
				return this._stackTracePreview;
			}
		}

		public bool Matches(ConsoleEntry other)
		{
			return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (string.Equals(this.Message, other.Message) && string.Equals(this.StackTrace, other.StackTrace) && this.LogType == other.LogType));
		}

		private const int MessagePreviewLength = 180;

		private const int StackTracePreviewLength = 120;

		private string _messagePreview;

		private string _stackTracePreview;

		public int Count = 1;

		public LogType LogType;

		public string Message;

		public string StackTrace;
	}
}
