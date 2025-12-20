using System;
using SRDebugger.Services;
using SRF.Service;

namespace SRDebugger.Internal
{
	public static class Service
	{
		public static IConsoleService Console
		{
			get
			{
				if (Service._consoleService == null)
				{
					Service._consoleService = SRServiceManager.GetService<IConsoleService>();
				}
				return Service._consoleService;
			}
		}

		public static IDockConsoleService DockConsole
		{
			get
			{
				if (Service._dockConsoleService == null)
				{
					Service._dockConsoleService = SRServiceManager.GetService<IDockConsoleService>();
				}
				return Service._dockConsoleService;
			}
		}

		public static IDebugPanelService Panel
		{
			get
			{
				if (Service._debugPanelService == null)
				{
					Service._debugPanelService = SRServiceManager.GetService<IDebugPanelService>();
				}
				return Service._debugPanelService;
			}
		}

		public static IDebugTriggerService Trigger
		{
			get
			{
				if (Service._debugTriggerService == null)
				{
					Service._debugTriggerService = SRServiceManager.GetService<IDebugTriggerService>();
				}
				return Service._debugTriggerService;
			}
		}

		public static IPinnedUIService PinnedUI
		{
			get
			{
				if (Service._pinnedUiService == null)
				{
					Service._pinnedUiService = SRServiceManager.GetService<IPinnedUIService>();
				}
				return Service._pinnedUiService;
			}
		}

		public static IDebugCameraService DebugCamera
		{
			get
			{
				if (Service._debugCameraService == null)
				{
					Service._debugCameraService = SRServiceManager.GetService<IDebugCameraService>();
				}
				return Service._debugCameraService;
			}
		}

		public static IOptionsService Options
		{
			get
			{
				if (Service._optionsService == null)
				{
					Service._optionsService = SRServiceManager.GetService<IOptionsService>();
				}
				return Service._optionsService;
			}
		}

		private static IConsoleService _consoleService;

		private static IDebugPanelService _debugPanelService;

		private static IDebugTriggerService _debugTriggerService;

		private static IPinnedUIService _pinnedUiService;

		private static IDebugCameraService _debugCameraService;

		private static IOptionsService _optionsService;

		private static IDockConsoleService _dockConsoleService;
	}
}
