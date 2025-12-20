using System;
using System.Diagnostics;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IConsoleService))]
	public class StandardConsoleService : IConsoleService
	{
		public StandardConsoleService()
		{
			Application.logMessageReceivedThreaded += this.UnityLogCallback;
			SRServiceManager.RegisterService<IConsoleService>(this);
			this._collapseEnabled = Settings.Instance.CollapseDuplicateLogEntries;
			this._allConsoleEntries = new CircularBuffer<ConsoleEntry>(Settings.Instance.MaximumConsoleEntries);
		}

		public int ErrorCount { get; private set; }

		public int WarningCount { get; private set; }

		public int InfoCount { get; private set; }

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event ConsoleUpdatedEventHandler Updated;

		public IReadOnlyList<ConsoleEntry> Entries
		{
			get
			{
				if (!this._hasCleared)
				{
					return this._allConsoleEntries;
				}
				return this._consoleEntries;
			}
		}

		public IReadOnlyList<ConsoleEntry> AllEntries
		{
			get
			{
				return this._allConsoleEntries;
			}
		}

		public void Clear()
		{
			object threadLock = this._threadLock;
			lock (threadLock)
			{
				this._hasCleared = true;
				if (this._consoleEntries == null)
				{
					this._consoleEntries = new CircularBuffer<ConsoleEntry>(Settings.Instance.MaximumConsoleEntries);
				}
				int num = 0;
				this.InfoCount = num;
				num = num;
				this.WarningCount = num;
				this.ErrorCount = num;
			}
			this.OnUpdated();
		}

		protected void OnEntryAdded(ConsoleEntry entry)
		{
			if (this._hasCleared)
			{
				if (this._consoleEntries.IsFull)
				{
					this.AdjustCounter(this._consoleEntries.Front().LogType, -1);
					this._consoleEntries.PopFront();
				}
				this._consoleEntries.PushBack(entry);
			}
			else if (this._allConsoleEntries.IsFull)
			{
				this.AdjustCounter(this._allConsoleEntries.Front().LogType, -1);
				this._allConsoleEntries.PopFront();
			}
			this._allConsoleEntries.PushBack(entry);
			this.OnUpdated();
		}

		protected void OnEntryDuplicated(ConsoleEntry entry)
		{
			entry.Count++;
			this.OnUpdated();
			if (this._hasCleared && this._consoleEntries.Count == 0)
			{
				this.OnEntryAdded(new ConsoleEntry(entry)
				{
					Count = 1
				});
			}
		}

		private void OnUpdated()
		{
			if (this.Updated != null)
			{
				try
				{
					this.Updated(this);
				}
				catch
				{
				}
			}
		}

		private void UnityLogCallback(string condition, string stackTrace, LogType type)
		{
			object threadLock = this._threadLock;
			lock (threadLock)
			{
				ConsoleEntry consoleEntry = (!this._collapseEnabled || this._allConsoleEntries.Count <= 0) ? null : this._allConsoleEntries[this._allConsoleEntries.Count - 1];
				if (consoleEntry != null && consoleEntry.LogType == type && consoleEntry.Message == condition && consoleEntry.StackTrace == stackTrace)
				{
					this.OnEntryDuplicated(consoleEntry);
				}
				else
				{
					ConsoleEntry entry = new ConsoleEntry
					{
						LogType = type,
						StackTrace = stackTrace,
						Message = condition
					};
					this.OnEntryAdded(entry);
				}
				this.AdjustCounter(type, 1);
			}
		}

		private void AdjustCounter(LogType type, int amount)
		{
			switch (type)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				this.ErrorCount += amount;
				break;
			case LogType.Warning:
				this.WarningCount += amount;
				break;
			case LogType.Log:
				this.InfoCount += amount;
				break;
			}
		}

		private readonly bool _collapseEnabled;

		private bool _hasCleared;

		private readonly CircularBuffer<ConsoleEntry> _allConsoleEntries;

		private CircularBuffer<ConsoleEntry> _consoleEntries;

		private readonly object _threadLock = new object();
	}
}
