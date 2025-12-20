using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Tabs
{
	public class ConsoleTabController : SRMonoBehaviourEx
	{
		protected override void Start()
		{
			base.Start();
			this._consoleCanvas = base.GetComponent<Canvas>();
			this.ToggleErrors.onValueChanged.AddListener(delegate(bool isOn)
			{
				this._isDirty = true;
			});
			this.ToggleWarnings.onValueChanged.AddListener(delegate(bool isOn)
			{
				this._isDirty = true;
			});
			this.ToggleInfo.onValueChanged.AddListener(delegate(bool isOn)
			{
				this._isDirty = true;
			});
			this.PinToggle.onValueChanged.AddListener(new UnityAction<bool>(this.PinToggleValueChanged));
			this.FilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.FilterToggleValueChanged));
			this.FilterBarContainer.SetActive(this.FilterToggle.isOn);
			this.FilterField.onValueChanged.AddListener(new UnityAction<string>(this.FilterValueChanged));
			this.ConsoleLogControl.SelectedItemChanged = new Action<ConsoleEntry>(this.ConsoleLogSelectedItemChanged);
			Service.Console.Updated += this.ConsoleOnUpdated;
			Service.Panel.VisibilityChanged += this.PanelOnVisibilityChanged;
			this.StackTraceText.supportRichText = Settings.Instance.RichTextInConsole;
			this.PopulateStackTraceArea(null);
			this.Refresh();
		}

		private void FilterToggleValueChanged(bool isOn)
		{
			if (isOn)
			{
				this.FilterBarContainer.SetActive(true);
				this.ConsoleLogControl.Filter = this.FilterField.text;
			}
			else
			{
				this.ConsoleLogControl.Filter = null;
				this.FilterBarContainer.SetActive(false);
			}
		}

		private void FilterValueChanged(string filterText)
		{
			if (this.FilterToggle.isOn && !string.IsNullOrEmpty(filterText) && filterText.Trim().Length != 0)
			{
				this.ConsoleLogControl.Filter = filterText;
			}
			else
			{
				this.ConsoleLogControl.Filter = null;
			}
		}

		private void PanelOnVisibilityChanged(IDebugPanelService debugPanelService, bool b)
		{
			if (this._consoleCanvas == null)
			{
				return;
			}
			if (b)
			{
				this._consoleCanvas.enabled = true;
			}
			else
			{
				this._consoleCanvas.enabled = false;
			}
		}

		private void PinToggleValueChanged(bool isOn)
		{
			Service.DockConsole.IsVisible = isOn;
		}

		protected override void OnDestroy()
		{
			if (Service.Console != null)
			{
				Service.Console.Updated -= this.ConsoleOnUpdated;
			}
			base.OnDestroy();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this._isDirty = true;
		}

		private void ConsoleLogSelectedItemChanged(object item)
		{
			ConsoleEntry entry = item as ConsoleEntry;
			this.PopulateStackTraceArea(entry);
		}

		protected override void Update()
		{
			base.Update();
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		private void PopulateStackTraceArea(ConsoleEntry entry)
		{
			if (entry == null)
			{
				this.StackTraceText.text = string.Empty;
			}
			else
			{
				string text = entry.Message + Environment.NewLine + (string.IsNullOrEmpty(entry.StackTrace) ? SRDebugStrings.Current.Console_NoStackTrace : entry.StackTrace);
				if (text.Length > 2600)
				{
					text = text.Substring(0, 2600);
					text = text + "\n" + SRDebugStrings.Current.Console_MessageTruncated;
				}
				this.StackTraceText.text = text;
			}
			this.StackTraceScrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		private void Refresh()
		{
			this.ToggleInfoText.text = SRDebuggerUtil.GetNumberString(Service.Console.InfoCount, 999, "999+");
			this.ToggleWarningsText.text = SRDebuggerUtil.GetNumberString(Service.Console.WarningCount, 999, "999+");
			this.ToggleErrorsText.text = SRDebuggerUtil.GetNumberString(Service.Console.ErrorCount, 999, "999+");
			this.ConsoleLogControl.ShowErrors = this.ToggleErrors.isOn;
			this.ConsoleLogControl.ShowWarnings = this.ToggleWarnings.isOn;
			this.ConsoleLogControl.ShowInfo = this.ToggleInfo.isOn;
			this.PinToggle.isOn = Service.DockConsole.IsVisible;
			this._isDirty = false;
		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			this._isDirty = true;
		}

		public void Clear()
		{
			Service.Console.Clear();
			this._isDirty = true;
		}

		private const int MaxLength = 2600;

		private Canvas _consoleCanvas;

		private bool _isDirty;

		[RequiredField]
		public ConsoleLogControl ConsoleLogControl;

		[RequiredField]
		public Toggle PinToggle;

		[RequiredField]
		public ScrollRect StackTraceScrollRect;

		[RequiredField]
		public Text StackTraceText;

		[RequiredField]
		public Toggle ToggleErrors;

		[RequiredField]
		public Text ToggleErrorsText;

		[RequiredField]
		public Toggle ToggleInfo;

		[RequiredField]
		public Text ToggleInfoText;

		[RequiredField]
		public Toggle ToggleWarnings;

		[RequiredField]
		public Text ToggleWarningsText;

		[RequiredField]
		public Toggle FilterToggle;

		[RequiredField]
		public InputField FilterField;

		[RequiredField]
		public GameObject FilterBarContainer;
	}
}
