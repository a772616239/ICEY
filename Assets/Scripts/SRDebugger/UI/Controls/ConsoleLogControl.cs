using System;
using System.Collections;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRF;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class ConsoleLogControl : SRMonoBehaviourEx
	{
		public bool ShowErrors
		{
			get
			{
				return this._showErrors;
			}
			set
			{
				this._showErrors = value;
				this.SetIsDirty();
			}
		}

		public bool ShowWarnings
		{
			get
			{
				return this._showWarnings;
			}
			set
			{
				this._showWarnings = value;
				this.SetIsDirty();
			}
		}

		public bool ShowInfo
		{
			get
			{
				return this._showInfo;
			}
			set
			{
				this._showInfo = value;
				this.SetIsDirty();
			}
		}

		public bool EnableSelection
		{
			get
			{
				return this._consoleScrollLayoutGroup.EnableSelection;
			}
			set
			{
				this._consoleScrollLayoutGroup.EnableSelection = value;
			}
		}

		public string Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				if (this._filter != value)
				{
					this._filter = value;
					this._isDirty = true;
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this._consoleScrollLayoutGroup.SelectedItemChanged.AddListener(new UnityAction<object>(this.OnSelectedItemChanged));
			Service.Console.Updated += this.ConsoleOnUpdated;
		}

		protected override void Start()
		{
			base.Start();
			this.SetIsDirty();
			base.StartCoroutine(this.ScrollToBottom());
		}

		private IEnumerator ScrollToBottom()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			this._scrollPosition = new Vector2?(new Vector2(0f, 0f));
			yield break;
		}

		protected override void OnDestroy()
		{
			if (Service.Console != null)
			{
				Service.Console.Updated -= this.ConsoleOnUpdated;
			}
			base.OnDestroy();
		}

		private void OnSelectedItemChanged(object arg0)
		{
			ConsoleEntry obj = arg0 as ConsoleEntry;
			if (this.SelectedItemChanged != null)
			{
				this.SelectedItemChanged(obj);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this._scrollPosition != null)
			{
				this._consoleScrollRect.normalizedPosition = this._scrollPosition.Value;
				this._scrollPosition = null;
			}
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		private void Refresh()
		{
			if (this._consoleScrollRect.normalizedPosition.y.ApproxZero())
			{
				this._scrollPosition = new Vector2?(this._consoleScrollRect.normalizedPosition);
			}
			this._consoleScrollLayoutGroup.ClearItems();
			IReadOnlyList<ConsoleEntry> entries = Service.Console.Entries;
			for (int i = 0; i < entries.Count; i++)
			{
				ConsoleEntry consoleEntry = entries[i];
				if ((consoleEntry.LogType == LogType.Error || consoleEntry.LogType == LogType.Exception || consoleEntry.LogType == LogType.Assert) && !this.ShowErrors)
				{
					if (consoleEntry == this._consoleScrollLayoutGroup.SelectedItem)
					{
						this._consoleScrollLayoutGroup.SelectedItem = null;
					}
				}
				else if (consoleEntry.LogType == LogType.Warning && !this.ShowWarnings)
				{
					if (consoleEntry == this._consoleScrollLayoutGroup.SelectedItem)
					{
						this._consoleScrollLayoutGroup.SelectedItem = null;
					}
				}
				else if (consoleEntry.LogType == LogType.Log && !this.ShowInfo)
				{
					if (consoleEntry == this._consoleScrollLayoutGroup.SelectedItem)
					{
						this._consoleScrollLayoutGroup.SelectedItem = null;
					}
				}
				else if (!string.IsNullOrEmpty(this.Filter) && consoleEntry.Message.IndexOf(this.Filter, StringComparison.OrdinalIgnoreCase) < 0)
				{
					if (consoleEntry == this._consoleScrollLayoutGroup.SelectedItem)
					{
						this._consoleScrollLayoutGroup.SelectedItem = null;
					}
				}
				else
				{
					this._consoleScrollLayoutGroup.AddItem(consoleEntry);
				}
			}
			this._isDirty = false;
		}

		private void SetIsDirty()
		{
			this._isDirty = true;
		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			this.SetIsDirty();
		}

		[RequiredField]
		[SerializeField]
		private VirtualVerticalLayoutGroup _consoleScrollLayoutGroup;

		[RequiredField]
		[SerializeField]
		private ScrollRect _consoleScrollRect;

		private bool _isDirty;

		private Vector2? _scrollPosition;

		private bool _showErrors = true;

		private bool _showInfo = true;

		private bool _showWarnings = true;

		public Action<ConsoleEntry> SelectedItemChanged;

		private string _filter;
	}
}
