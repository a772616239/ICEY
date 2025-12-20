using System;
using System.Diagnostics;
using SRDebugger.Internal;
using SRDebugger.UI;
using SRF;
using SRF.Service;
using SRF.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IDebugService))]
	public class SRDebugService : IDebugService
	{
		public SRDebugService()
		{
			SRServiceManager.RegisterService<IDebugService>(this);
			SRServiceManager.GetService<IProfilerService>();
			this._debugTrigger = SRServiceManager.GetService<IDebugTriggerService>();
			this._informationService = SRServiceManager.GetService<ISystemInformationService>();
			this._pinnedUiService = SRServiceManager.GetService<IPinnedUIService>();
			this._optionsService = SRServiceManager.GetService<IOptionsService>();
			this._debugPanelService = SRServiceManager.GetService<IDebugPanelService>();
			this._debugPanelService.VisibilityChanged += this.DebugPanelServiceOnVisibilityChanged;
			this._debugTrigger.IsEnabled = (this.Settings.EnableTrigger == Settings.TriggerEnableModes.Enabled || (this.Settings.EnableTrigger == Settings.TriggerEnableModes.MobileOnly && Application.isMobilePlatform) || (this.Settings.EnableTrigger == Settings.TriggerEnableModes.DevelopmentBuildsOnly && UnityEngine.Debug.isDebugBuild));
			this._debugTrigger.Position = this.Settings.TriggerPosition;
			if (this.Settings.EnableKeyboardShortcuts)
			{
				SRServiceManager.GetService<KeyboardShortcutListenerService>();
			}
			this._entryCodeEnabled = (Settings.Instance.RequireCode && Settings.Instance.EntryCode.Count == 4);
			if (Settings.Instance.RequireCode && !this._entryCodeEnabled)
			{
				UnityEngine.Debug.LogError("[SRDebugger] RequireCode is enabled, but pin is not 4 digits");
			}
			Transform transform = Hierarchy.Get("SRDebugger");
			UnityEngine.Object.DontDestroyOnLoad(transform.gameObject);
		}

		public Settings Settings
		{
			get
			{
				return Settings.Instance;
			}
		}

		public bool IsDebugPanelVisible
		{
			get
			{
				return this._debugPanelService.IsVisible;
			}
		}

		public bool IsTriggerEnabled
		{
			get
			{
				return this._debugTrigger.IsEnabled;
			}
			set
			{
				this._debugTrigger.IsEnabled = value;
			}
		}

		public bool IsProfilerDocked
		{
			get
			{
				return Service.PinnedUI.IsProfilerPinned;
			}
			set
			{
				Service.PinnedUI.IsProfilerPinned = value;
			}
		}

		public void AddSystemInfo(InfoEntry entry, string category = "Default")
		{
			this._informationService.Add(entry, category);
		}

		public void ShowDebugPanel(bool requireEntryCode = true)
		{
			if (requireEntryCode && this._entryCodeEnabled && !this._hasAuthorised)
			{
				this.PromptEntryCode();
				return;
			}
			this._debugPanelService.IsVisible = true;
		}

		public void ShowDebugPanel(DefaultTabs tab, bool requireEntryCode = true)
		{
			if (requireEntryCode && this._entryCodeEnabled && !this._hasAuthorised)
			{
				this._queuedTab = new DefaultTabs?(tab);
				this.PromptEntryCode();
				return;
			}
			this._debugPanelService.IsVisible = true;
			this._debugPanelService.OpenTab(tab);
		}

		public void HideDebugPanel()
		{
			this._debugPanelService.IsVisible = false;
		}

		public void DestroyDebugPanel()
		{
			this._debugPanelService.IsVisible = false;
			this._debugPanelService.Unload();
		}

		public void AddOptionContainer(object container)
		{
			this._optionsService.AddContainer(container);
		}

		public void RemoveOptionContainer(object container)
		{
			this._optionsService.RemoveContainer(container);
		}

		public void PinAllOptions(string category)
		{
			foreach (OptionDefinition optionDefinition in this._optionsService.Options)
			{
				if (optionDefinition.Category == category)
				{
					this._pinnedUiService.Pin(optionDefinition, -1);
				}
			}
		}

		public void UnpinAllOptions(string category)
		{
			foreach (OptionDefinition optionDefinition in this._optionsService.Options)
			{
				if (optionDefinition.Category == category)
				{
					this._pinnedUiService.Unpin(optionDefinition);
				}
			}
		}

		public void PinOption(string name)
		{
			foreach (OptionDefinition optionDefinition in this._optionsService.Options)
			{
				if (optionDefinition.Name == name)
				{
					this._pinnedUiService.Pin(optionDefinition, -1);
				}
			}
		}

		public void UnpinOption(string name)
		{
			foreach (OptionDefinition optionDefinition in this._optionsService.Options)
			{
				if (optionDefinition.Name == name)
				{
					this._pinnedUiService.Unpin(optionDefinition);
				}
			}
		}

		public void ClearPinnedOptions()
		{
			this._pinnedUiService.UnpinAll();
		}

		public void ShowBugReportSheet(ActionCompleteCallback onComplete = null, bool takeScreenshot = true, string descriptionContent = null)
		{
			BugReportPopoverService service = SRServiceManager.GetService<BugReportPopoverService>();
			if (service.IsShowingPopover)
			{
				return;
			}
			service.ShowBugReporter(delegate(bool succeed, string message)
			{
				if (onComplete != null)
				{
					onComplete(succeed);
				}
			}, takeScreenshot, descriptionContent);
		}

		public IDockConsoleService DockConsole
		{
			get
			{
				return Service.DockConsole;
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event VisibilityChangedDelegate PanelVisibilityChanged;

		private void DebugPanelServiceOnVisibilityChanged(IDebugPanelService debugPanelService, bool b)
		{
			if (this.PanelVisibilityChanged == null)
			{
				return;
			}
			try
			{
				this.PanelVisibilityChanged(b);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Event target threw exception (IDebugService.PanelVisiblityChanged)");
				UnityEngine.Debug.LogException(exception);
			}
		}

		private void PromptEntryCode()
		{
			SRServiceManager.GetService<IPinEntryService>().ShowPinEntry(Settings.Instance.EntryCode, SRDebugStrings.Current.PinEntryPrompt, delegate(bool entered)
			{
				if (entered)
				{
					if (!Settings.Instance.RequireEntryCodeEveryTime)
					{
						this._hasAuthorised = true;
					}
					if (this._queuedTab != null)
					{
						DefaultTabs value = this._queuedTab.Value;
						this._queuedTab = null;
						this.ShowDebugPanel(value, false);
					}
					else
					{
						this.ShowDebugPanel(false);
					}
				}
				this._queuedTab = null;
			}, true);
		}

		public RectTransform EnableWorldSpaceMode()
		{
			if (this._worldSpaceTransform != null)
			{
				return this._worldSpaceTransform;
			}
			if (Settings.Instance.UseDebugCamera)
			{
				throw new InvalidOperationException("UseDebugCamera cannot be enabled at the same time as EnableWorldSpaceMode.");
			}
			this._debugPanelService.IsVisible = true;
			DebugPanelRoot rootObject = ((DebugPanelServiceImpl)this._debugPanelService).RootObject;
			rootObject.Canvas.gameObject.RemoveComponentIfExists<SRRetinaScaler>();
			rootObject.Canvas.gameObject.RemoveComponentIfExists<CanvasScaler>();
			rootObject.Canvas.renderMode = RenderMode.WorldSpace;
			RectTransform component = rootObject.Canvas.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(1024f, 768f);
			component.position = Vector3.zero;
			return this._worldSpaceTransform = component;
		}

		private readonly IDebugPanelService _debugPanelService;

		private readonly IDebugTriggerService _debugTrigger;

		private readonly ISystemInformationService _informationService;

		private readonly IOptionsService _optionsService;

		private readonly IPinnedUIService _pinnedUiService;

		private bool _entryCodeEnabled;

		private bool _hasAuthorised;

		private DefaultTabs? _queuedTab;

		private RectTransform _worldSpaceTransform;
	}
}
