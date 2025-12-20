using System;
using System.Collections.Generic;
using SRDebugger.Internal;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(KeyboardShortcutListenerService))]
	public class KeyboardShortcutListenerService : SRServiceBase<KeyboardShortcutListenerService>
	{
		protected override void Awake()
		{
			base.Awake();
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));
			this._shortcuts = new List<Settings.KeyboardShortcut>(Settings.Instance.KeyboardShortcuts);
		}

		private void ToggleTab(DefaultTabs t)
		{
			DefaultTabs? activeTab = Service.Panel.ActiveTab;
			if (Service.Panel.IsVisible && activeTab != null && activeTab.Value == t)
			{
				SRDebug.Instance.HideDebugPanel();
			}
			else
			{
				SRDebug.Instance.ShowDebugPanel(t, true);
			}
		}

		private void ExecuteShortcut(Settings.KeyboardShortcut shortcut)
		{
			switch (shortcut.Action)
			{
			case Settings.ShortcutActions.OpenSystemInfoTab:
				this.ToggleTab(DefaultTabs.SystemInformation);
				break;
			case Settings.ShortcutActions.OpenConsoleTab:
				this.ToggleTab(DefaultTabs.Console);
				break;
			case Settings.ShortcutActions.OpenOptionsTab:
				this.ToggleTab(DefaultTabs.Options);
				break;
			case Settings.ShortcutActions.OpenProfilerTab:
				this.ToggleTab(DefaultTabs.Profiler);
				break;
			case Settings.ShortcutActions.OpenBugReporterTab:
				this.ToggleTab(DefaultTabs.BugReporter);
				break;
			case Settings.ShortcutActions.ClosePanel:
				SRDebug.Instance.HideDebugPanel();
				break;
			case Settings.ShortcutActions.OpenPanel:
				SRDebug.Instance.ShowDebugPanel(true);
				break;
			case Settings.ShortcutActions.TogglePanel:
				if (SRDebug.Instance.IsDebugPanelVisible)
				{
					SRDebug.Instance.HideDebugPanel();
				}
				else
				{
					SRDebug.Instance.ShowDebugPanel(true);
				}
				break;
			case Settings.ShortcutActions.ShowBugReportPopover:
				SRDebug.Instance.ShowBugReportSheet(null, true, null);
				break;
			case Settings.ShortcutActions.ToggleDockedConsole:
				SRDebug.Instance.DockConsole.IsVisible = !SRDebug.Instance.DockConsole.IsVisible;
				break;
			case Settings.ShortcutActions.ToggleDockedProfiler:
				SRDebug.Instance.IsProfilerDocked = !SRDebug.Instance.IsProfilerDocked;
				break;
			default:
				UnityEngine.Debug.LogWarning("[SRDebugger] Unhandled keyboard shortcut: " + shortcut.Action);
				break;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (Settings.Instance.KeyboardEscapeClose && UnityEngine.Input.GetKeyDown(KeyCode.Escape) && Service.Panel.IsVisible)
			{
				SRDebug.Instance.HideDebugPanel();
			}
			bool flag = UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl);
			bool flag2 = UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(KeyCode.RightAlt);
			bool flag3 = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
			for (int i = 0; i < this._shortcuts.Count; i++)
			{
				Settings.KeyboardShortcut keyboardShortcut = this._shortcuts[i];
				if (!keyboardShortcut.Control || flag)
				{
					if (!keyboardShortcut.Shift || flag3)
					{
						if (!keyboardShortcut.Alt || flag2)
						{
							if (UnityEngine.Input.GetKeyDown(keyboardShortcut.Key))
							{
								this.ExecuteShortcut(keyboardShortcut);
								break;
							}
						}
					}
				}
			}
		}

		private List<Settings.KeyboardShortcut> _shortcuts;
	}
}
