using System;
using SRDebugger.Internal;
using SRDebugger.UI.Other;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IDockConsoleService))]
	public class DockConsoleServiceImpl : IDockConsoleService
	{
		public DockConsoleServiceImpl()
		{
			this._alignment = Settings.Instance.ConsoleAlignment;
		}

		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value == this._isVisible)
				{
					return;
				}
				this._isVisible = value;
				if (this._consoleRoot == null && value)
				{
					this.Load();
				}
				else
				{
					this._consoleRoot.CachedGameObject.SetActive(value);
				}
				this.CheckTrigger();
			}
		}

		public bool IsExpanded
		{
			get
			{
				return this._isExpanded;
			}
			set
			{
				if (value == this._isExpanded)
				{
					return;
				}
				this._isExpanded = value;
				if (this._consoleRoot == null && value)
				{
					this.Load();
				}
				else
				{
					this._consoleRoot.SetDropdownVisibility(value);
				}
				this.CheckTrigger();
			}
		}

		public ConsoleAlignment Alignment
		{
			get
			{
				return this._alignment;
			}
			set
			{
				this._alignment = value;
				if (this._consoleRoot != null)
				{
					this._consoleRoot.SetAlignmentMode(value);
				}
				this.CheckTrigger();
			}
		}

		private void Load()
		{
			IPinnedUIService service = SRServiceManager.GetService<IPinnedUIService>();
			if (service == null)
			{
				UnityEngine.Debug.LogError("[DockConsoleService] PinnedUIService not found");
				return;
			}
			PinnedUIServiceImpl pinnedUIServiceImpl = service as PinnedUIServiceImpl;
			if (pinnedUIServiceImpl == null)
			{
				UnityEngine.Debug.LogError("[DockConsoleService] Expected IPinnedUIService to be PinnedUIServiceImpl");
				return;
			}
			this._consoleRoot = pinnedUIServiceImpl.DockConsoleController;
			this._consoleRoot.SetDropdownVisibility(this._isExpanded);
			this._consoleRoot.IsVisible = this._isVisible;
			this._consoleRoot.SetAlignmentMode(this._alignment);
			this.CheckTrigger();
		}

		private void CheckTrigger()
		{
			ConsoleAlignment? consoleAlignment = null;
			PinAlignment position = Service.Trigger.Position;
			if (position == PinAlignment.TopLeft || position == PinAlignment.TopRight || position == PinAlignment.TopCenter)
			{
				consoleAlignment = new ConsoleAlignment?(ConsoleAlignment.Top);
			}
			else if (position == PinAlignment.BottomLeft || position == PinAlignment.BottomRight || position == PinAlignment.BottomCenter)
			{
				consoleAlignment = new ConsoleAlignment?(ConsoleAlignment.Bottom);
			}
			bool flag = consoleAlignment != null && this.IsVisible && this.Alignment == consoleAlignment.Value;
			if (this._didSuspendTrigger && !flag)
			{
				Service.Trigger.IsEnabled = true;
				this._didSuspendTrigger = false;
			}
			else if (Service.Trigger.IsEnabled && flag)
			{
				Service.Trigger.IsEnabled = false;
				this._didSuspendTrigger = true;
			}
		}

		private ConsoleAlignment _alignment;

		private DockConsoleController _consoleRoot;

		private bool _didSuspendTrigger;

		private bool _isExpanded = true;

		private bool _isVisible;
	}
}
