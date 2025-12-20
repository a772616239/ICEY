using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace SRDebugger
{
	public class Settings : ScriptableObject
	{
		public static Settings Instance
		{
			get
			{
				if (Settings._instance == null)
				{
					Settings._instance = Settings.GetOrCreateInstance();
				}
				if (Settings._instance._keyboardShortcuts != null && Settings._instance._keyboardShortcuts.Length > 0)
				{
					Settings._instance.UpgradeKeyboardShortcuts();
				}
				return Settings._instance;
			}
		}

		private static Settings.KeyboardShortcut[] GetDefaultKeyboardShortcuts()
		{
			return new Settings.KeyboardShortcut[]
			{
				new Settings.KeyboardShortcut
				{
					Control = true,
					Shift = true,
					Key = KeyCode.F1,
					Action = Settings.ShortcutActions.OpenSystemInfoTab
				},
				new Settings.KeyboardShortcut
				{
					Control = true,
					Shift = true,
					Key = KeyCode.F2,
					Action = Settings.ShortcutActions.OpenConsoleTab
				},
				new Settings.KeyboardShortcut
				{
					Control = true,
					Shift = true,
					Key = KeyCode.F3,
					Action = Settings.ShortcutActions.OpenOptionsTab
				},
				new Settings.KeyboardShortcut
				{
					Control = true,
					Shift = true,
					Key = KeyCode.F4,
					Action = Settings.ShortcutActions.OpenProfilerTab
				}
			};
		}

		private void UpgradeKeyboardShortcuts()
		{
			UnityEngine.Debug.Log("[SRDebugger] Upgrading Settings format");
			List<Settings.KeyboardShortcut> list = new List<Settings.KeyboardShortcut>();
			for (int i = 0; i < this._keyboardShortcuts.Length; i++)
			{
				Settings.KeyboardShortcut keyboardShortcut = this._keyboardShortcuts[i];
				list.Add(new Settings.KeyboardShortcut
				{
					Action = keyboardShortcut.Action,
					Key = keyboardShortcut.Key,
					Alt = this._keyboardModifierAlt,
					Shift = this._keyboardModifierShift,
					Control = this._keyboardModifierControl
				});
			}
			this._keyboardShortcuts = new Settings.KeyboardShortcut[0];
			this._newKeyboardShortcuts = list.ToArray();
		}

		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		public bool AutoLoad
		{
			get
			{
				return this._autoLoad;
			}
		}

		public DefaultTabs DefaultTab
		{
			get
			{
				return this._defaultTab;
			}
		}

		public Settings.TriggerEnableModes EnableTrigger
		{
			get
			{
				return this._triggerEnableMode;
			}
		}

		public Settings.TriggerBehaviours TriggerBehaviour
		{
			get
			{
				return this._triggerBehaviour;
			}
		}

		public bool EnableKeyboardShortcuts
		{
			get
			{
				return this._enableKeyboardShortcuts;
			}
		}

		public IList<Settings.KeyboardShortcut> KeyboardShortcuts
		{
			get
			{
				return this._newKeyboardShortcuts;
			}
		}

		public bool KeyboardEscapeClose
		{
			get
			{
				return this._keyboardEscapeClose;
			}
		}

		public bool EnableBackgroundTransparency
		{
			get
			{
				return this._enableBackgroundTransparency;
			}
		}

		public bool RequireCode
		{
			get
			{
				return this._requireEntryCode;
			}
		}

		public bool RequireEntryCodeEveryTime
		{
			get
			{
				return this._requireEntryCodeEveryTime;
			}
		}

		public IList<int> EntryCode
		{
			get
			{
				return new ReadOnlyCollection<int>(this._entryCode);
			}
			set
			{
				if (value.Count != 4)
				{
					throw new Exception("Entry code must be length 4");
				}
				if (value.Any((int p) => p > 9 || p < 0))
				{
					throw new Exception("All digits in entry code must be >= 0 and <= 9");
				}
				this._entryCode = value.ToArray<int>();
			}
		}

		public bool UseDebugCamera
		{
			get
			{
				return this._useDebugCamera;
			}
		}

		public int DebugLayer
		{
			get
			{
				return this._debugLayer;
			}
		}

		public float DebugCameraDepth
		{
			get
			{
				return this._debugCameraDepth;
			}
		}

		public bool CollapseDuplicateLogEntries
		{
			get
			{
				return this._collapseDuplicateLogEntries;
			}
		}

		public bool RichTextInConsole
		{
			get
			{
				return this._richTextInConsole;
			}
		}

		public string ApiKey
		{
			get
			{
				return this._apiKey;
			}
		}

		public bool EnableBugReporter
		{
			get
			{
				return this._enableBugReporter;
			}
		}

		public IList<DefaultTabs> DisabledTabs
		{
			get
			{
				return this._disabledTabs;
			}
		}

		public PinAlignment TriggerPosition
		{
			get
			{
				return this._triggerPosition;
			}
		}

		public PinAlignment ProfilerAlignment
		{
			get
			{
				return this._profilerAlignment;
			}
		}

		public PinAlignment OptionsAlignment
		{
			get
			{
				return this._optionsAlignment;
			}
		}

		public ConsoleAlignment ConsoleAlignment
		{
			get
			{
				return this._consoleAlignment;
			}
			set
			{
				this._consoleAlignment = value;
			}
		}

		public int MaximumConsoleEntries
		{
			get
			{
				return this._maximumConsoleEntries;
			}
			set
			{
				this._maximumConsoleEntries = value;
			}
		}

		public bool EnableEventSystemGeneration
		{
			get
			{
				return this._enableEventSystemCreation;
			}
			set
			{
				this._enableEventSystemCreation = value;
			}
		}

		public bool AutomaticallyShowCursor
		{
			get
			{
				return this._automaticShowCursor;
			}
		}

		private static Settings GetOrCreateInstance()
		{
			Settings settings = Resources.Load<Settings>("SRDebugger/Settings");
			if (settings == null)
			{
				settings = ScriptableObject.CreateInstance<Settings>();
			}
			return settings;
		}

		private const string ResourcesPath = "/usr/Resources/SRDebugger";

		private const string ResourcesName = "Settings";

		private static Settings _instance;

		[SerializeField]
		private bool _isEnabled = true;

		[SerializeField]
		private bool _autoLoad = true;

		[SerializeField]
		private DefaultTabs _defaultTab;

		[SerializeField]
		private Settings.TriggerEnableModes _triggerEnableMode;

		[SerializeField]
		private Settings.TriggerBehaviours _triggerBehaviour;

		[SerializeField]
		private bool _enableKeyboardShortcuts = true;

		[SerializeField]
		private Settings.KeyboardShortcut[] _keyboardShortcuts;

		[SerializeField]
		private Settings.KeyboardShortcut[] _newKeyboardShortcuts = Settings.GetDefaultKeyboardShortcuts();

		[SerializeField]
		private bool _keyboardModifierControl = true;

		[SerializeField]
		private bool _keyboardModifierAlt;

		[SerializeField]
		private bool _keyboardModifierShift = true;

		[SerializeField]
		private bool _keyboardEscapeClose = true;

		[SerializeField]
		private bool _enableBackgroundTransparency = true;

		[SerializeField]
		private bool _collapseDuplicateLogEntries = true;

		[SerializeField]
		private bool _richTextInConsole = true;

		[SerializeField]
		private bool _requireEntryCode;

		[SerializeField]
		private bool _requireEntryCodeEveryTime;

		[SerializeField]
		private int[] _entryCode = new int[4];

		[SerializeField]
		private bool _useDebugCamera;

		[SerializeField]
		private int _debugLayer = 5;

		[Range(-100f, 100f)]
		[SerializeField]
		private float _debugCameraDepth = 100f;

		[SerializeField]
		private string _apiKey = string.Empty;

		[SerializeField]
		private bool _enableBugReporter;

		[SerializeField]
		private DefaultTabs[] _disabledTabs = new DefaultTabs[0];

		[SerializeField]
		private PinAlignment _profilerAlignment = PinAlignment.BottomLeft;

		[SerializeField]
		private PinAlignment _optionsAlignment = PinAlignment.BottomRight;

		[SerializeField]
		private ConsoleAlignment _consoleAlignment;

		[SerializeField]
		private PinAlignment _triggerPosition;

		[SerializeField]
		private int _maximumConsoleEntries = 1500;

		[SerializeField]
		private bool _enableEventSystemCreation = true;

		[SerializeField]
		private bool _automaticShowCursor = true;

		public enum ShortcutActions
		{
			None,
			OpenSystemInfoTab,
			OpenConsoleTab,
			OpenOptionsTab,
			OpenProfilerTab,
			OpenBugReporterTab,
			ClosePanel,
			OpenPanel,
			TogglePanel,
			ShowBugReportPopover,
			ToggleDockedConsole,
			ToggleDockedProfiler
		}

		public enum TriggerBehaviours
		{
			TripleTap,
			TapAndHold,
			DoubleTap
		}

		public enum TriggerEnableModes
		{
			Enabled,
			MobileOnly,
			Off,
			DevelopmentBuildsOnly
		}

		[Serializable]
		public sealed class KeyboardShortcut
		{
			[SerializeField]
			public Settings.ShortcutActions Action;

			[SerializeField]
			public bool Alt;

			[SerializeField]
			public bool Control;

			[SerializeField]
			public KeyCode Key;

			[SerializeField]
			public bool Shift;
		}
	}
}
