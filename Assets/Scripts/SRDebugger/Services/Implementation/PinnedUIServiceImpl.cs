using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.UI.Controls;
using SRDebugger.UI.Other;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IPinnedUIService))]
	public class PinnedUIServiceImpl : SRServiceBase<IPinnedUIService>, IPinnedUIService
	{
		public DockConsoleController DockConsoleController
		{
			get
			{
				if (this._uiRoot == null)
				{
					this.Load();
				}
				return this._uiRoot.DockConsoleController;
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<OptionDefinition, bool> OptionPinStateChanged;

		public bool IsProfilerPinned
		{
			get
			{
				return !(this._uiRoot == null) && this._uiRoot.Profiler.activeSelf;
			}
			set
			{
				if (this._uiRoot == null)
				{
					this.Load();
				}
				this._uiRoot.Profiler.SetActive(value);
			}
		}

		public void Pin(OptionDefinition obj, int order = -1)
		{
			if (this._uiRoot == null)
			{
				this.Load();
			}
			if (this._pinnedObjects.ContainsKey(obj))
			{
				return;
			}
			OptionsControlBase optionsControlBase = OptionControlFactory.CreateControl(obj, null);
			optionsControlBase.CachedTransform.SetParent(this._uiRoot.Container, false);
			if (order >= 0)
			{
				optionsControlBase.CachedTransform.SetSiblingIndex(order);
			}
			this._pinnedObjects.Add(obj, optionsControlBase);
			this._controlList.Add(optionsControlBase);
			this.OnPinnedStateChanged(obj, true);
		}

		public void Unpin(OptionDefinition obj)
		{
			if (!this._pinnedObjects.ContainsKey(obj))
			{
				return;
			}
			OptionsControlBase optionsControlBase = this._pinnedObjects[obj];
			this._pinnedObjects.Remove(obj);
			this._controlList.Remove(optionsControlBase);
			UnityEngine.Object.Destroy(optionsControlBase.CachedGameObject);
			this.OnPinnedStateChanged(obj, false);
		}

		private void OnPinnedStateChanged(OptionDefinition option, bool isPinned)
		{
			if (this.OptionPinStateChanged != null)
			{
				this.OptionPinStateChanged(option, isPinned);
			}
		}

		public void UnpinAll()
		{
			foreach (KeyValuePair<OptionDefinition, OptionsControlBase> keyValuePair in this._pinnedObjects)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value.CachedGameObject);
			}
			this._pinnedObjects.Clear();
			this._controlList.Clear();
		}

		public bool HasPinned(OptionDefinition option)
		{
			return this._pinnedObjects.ContainsKey(option);
		}

		protected override void Awake()
		{
			base.Awake();
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));
		}

		private void Load()
		{
			PinnedUIRoot pinnedUIRoot = Resources.Load<PinnedUIRoot>("SRDebugger/UI/Prefabs/PinnedUI");
			if (pinnedUIRoot == null)
			{
				UnityEngine.Debug.LogError("[SRDebugger.PinnedUI] Error loading ui prefab");
				return;
			}
			PinnedUIRoot pinnedUIRoot2 = SRInstantiate.Instantiate<PinnedUIRoot>(pinnedUIRoot);
			pinnedUIRoot2.CachedTransform.SetParent(base.CachedTransform, false);
			this._uiRoot = pinnedUIRoot2;
			this.UpdateAnchors();
			SRDebug.Instance.PanelVisibilityChanged += this.OnDebugPanelVisibilityChanged;
			Service.Options.OptionsUpdated += this.OnOptionsUpdated;
			Service.Options.OptionsValueUpdated += this.OptionsOnPropertyChanged;
		}

		private void UpdateAnchors()
		{
			switch (Settings.Instance.ProfilerAlignment)
			{
			case PinAlignment.TopLeft:
			case PinAlignment.BottomLeft:
			case PinAlignment.CenterLeft:
				this._uiRoot.Profiler.transform.SetSiblingIndex(0);
				break;
			case PinAlignment.TopRight:
			case PinAlignment.BottomRight:
			case PinAlignment.CenterRight:
				this._uiRoot.Profiler.transform.SetSiblingIndex(1);
				break;
			}
			switch (Settings.Instance.ProfilerAlignment)
			{
			case PinAlignment.TopLeft:
			case PinAlignment.TopRight:
				this._uiRoot.ProfilerVerticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
				break;
			case PinAlignment.BottomLeft:
			case PinAlignment.BottomRight:
				this._uiRoot.ProfilerVerticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
				break;
			case PinAlignment.CenterLeft:
			case PinAlignment.CenterRight:
				this._uiRoot.ProfilerVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
				break;
			}
			this._uiRoot.ProfilerHandleManager.SetAlignment(Settings.Instance.ProfilerAlignment);
			switch (Settings.Instance.OptionsAlignment)
			{
			case PinAlignment.TopLeft:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.UpperLeft;
				break;
			case PinAlignment.TopRight:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.UpperRight;
				break;
			case PinAlignment.BottomLeft:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.LowerLeft;
				break;
			case PinAlignment.BottomRight:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.LowerRight;
				break;
			case PinAlignment.CenterLeft:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
				break;
			case PinAlignment.CenterRight:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.MiddleRight;
				break;
			case PinAlignment.TopCenter:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.UpperCenter;
				break;
			case PinAlignment.BottomCenter:
				this._uiRoot.OptionsLayoutGroup.childAlignment = TextAnchor.LowerCenter;
				break;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this._queueRefresh)
			{
				this._queueRefresh = false;
				this.Refresh();
			}
		}

		private void OnOptionsUpdated(object sender, EventArgs eventArgs)
		{
			List<OptionDefinition> list = this._pinnedObjects.Keys.ToList<OptionDefinition>();
			foreach (OptionDefinition optionDefinition in list)
			{
				if (!Service.Options.Options.Contains(optionDefinition))
				{
					this.Unpin(optionDefinition);
				}
			}
		}

		private void OptionsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			this._queueRefresh = true;
		}

		private void OnDebugPanelVisibilityChanged(bool isVisible)
		{
			if (!isVisible)
			{
				this._queueRefresh = true;
			}
		}

		private void Refresh()
		{
			for (int i = 0; i < this._controlList.Count; i++)
			{
				this._controlList[i].Refresh();
			}
		}

		private readonly List<OptionsControlBase> _controlList = new List<OptionsControlBase>();

		private readonly Dictionary<OptionDefinition, OptionsControlBase> _pinnedObjects = new Dictionary<OptionDefinition, OptionsControlBase>();

		private bool _queueRefresh;

		private PinnedUIRoot _uiRoot;
	}
}
