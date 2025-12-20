using System;
using System.Diagnostics;
using SRDebugger.Internal;
using SRDebugger.UI;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IDebugPanelService))]
	public class DebugPanelServiceImpl : ScriptableObject, IDebugPanelService
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IDebugPanelService, bool> VisibilityChanged;

		public DebugPanelRoot RootObject
		{
			get
			{
				return this._debugPanelRootObject;
			}
		}

		public bool IsLoaded
		{
			get
			{
				return this._debugPanelRootObject != null;
			}
		}

		public bool IsVisible
		{
			get
			{
				return this.IsLoaded && this._isVisible;
			}
			set
			{
				if (this._isVisible == value)
				{
					return;
				}
				if (value)
				{
					if (!this.IsLoaded)
					{
						this.Load();
					}
					SRDebuggerUtil.EnsureEventSystemExists();
					this._debugPanelRootObject.CanvasGroup.alpha = 1f;
					this._debugPanelRootObject.CanvasGroup.interactable = true;
					this._debugPanelRootObject.CanvasGroup.blocksRaycasts = true;
					this._cursorWasVisible = new bool?(Cursor.visible);
					this._cursorLockMode = new CursorLockMode?(Cursor.lockState);
					if (Settings.Instance.AutomaticallyShowCursor)
					{
						Cursor.visible = true;
						Cursor.lockState = CursorLockMode.None;
					}
				}
				else
				{
					if (this.IsLoaded)
					{
						this._debugPanelRootObject.CanvasGroup.alpha = 0f;
						this._debugPanelRootObject.CanvasGroup.interactable = false;
						this._debugPanelRootObject.CanvasGroup.blocksRaycasts = false;
					}
					if (this._cursorWasVisible != null)
					{
						Cursor.visible = this._cursorWasVisible.Value;
						this._cursorWasVisible = null;
					}
					if (this._cursorLockMode != null)
					{
						Cursor.lockState = this._cursorLockMode.Value;
						this._cursorLockMode = null;
					}
				}
				this._isVisible = value;
				if (this.VisibilityChanged != null)
				{
					this.VisibilityChanged(this, this._isVisible);
				}
			}
		}

		public DefaultTabs? ActiveTab
		{
			get
			{
				if (this._debugPanelRootObject == null)
				{
					return null;
				}
				return this._debugPanelRootObject.TabController.ActiveTab;
			}
		}

		public void OpenTab(DefaultTabs tab)
		{
			if (!this.IsVisible)
			{
				this.IsVisible = true;
			}
			this._debugPanelRootObject.TabController.OpenTab(tab);
		}

		public void Unload()
		{
			if (this._debugPanelRootObject == null)
			{
				return;
			}
			this.IsVisible = false;
			this._debugPanelRootObject.CachedGameObject.SetActive(false);
			UnityEngine.Object.Destroy(this._debugPanelRootObject.CachedGameObject);
			this._debugPanelRootObject = null;
		}

		private void Load()
		{
			DebugPanelRoot debugPanelRoot = Resources.Load<DebugPanelRoot>("SRDebugger/UI/Prefabs/DebugPanel");
			if (debugPanelRoot == null)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Error loading debug panel prefab");
				return;
			}
			this._debugPanelRootObject = SRInstantiate.Instantiate<DebugPanelRoot>(debugPanelRoot);
			this._debugPanelRootObject.name = "Panel";
			UnityEngine.Object.DontDestroyOnLoad(this._debugPanelRootObject);
			this._debugPanelRootObject.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"), true);
			SRDebuggerUtil.EnsureEventSystemExists();
		}

		private DebugPanelRoot _debugPanelRootObject;

		private bool _isVisible;

		private bool? _cursorWasVisible;

		private CursorLockMode? _cursorLockMode;
	}
}
