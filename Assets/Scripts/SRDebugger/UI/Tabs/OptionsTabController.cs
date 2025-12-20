using System;
using System.Collections.Generic;
using System.ComponentModel;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRDebugger.UI.Controls.Data;
using SRDebugger.UI.Other;
using SRF;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Tabs
{
	public class OptionsTabController : SRMonoBehaviourEx
	{
		protected override void Start()
		{
			base.Start();
			this.PinButton.onValueChanged.AddListener(new UnityAction<bool>(this.SetSelectionModeEnabled));
			this.PinPromptText.SetActive(false);
			this.Populate();
			this._optionCanvas = base.GetComponent<Canvas>();
			Service.Options.OptionsUpdated += this.OnOptionsUpdated;
			Service.Options.OptionsValueUpdated += this.OnOptionsValueChanged;
			Service.PinnedUI.OptionPinStateChanged += this.OnOptionPinnedStateChanged;
		}

		private void OnOptionPinnedStateChanged(OptionDefinition optionDefinition, bool isPinned)
		{
			if (this._options.ContainsKey(optionDefinition))
			{
				this._options[optionDefinition].IsSelected = isPinned;
			}
		}

		private void OnOptionsUpdated(object sender, EventArgs eventArgs)
		{
			this.Clear();
			this.Populate();
		}

		private void OnOptionsValueChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			this._queueRefresh = true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			Service.Panel.VisibilityChanged += this.PanelOnVisibilityChanged;
		}

		protected override void OnDisable()
		{
			this.SetSelectionModeEnabled(false);
			if (Service.Panel != null)
			{
				Service.Panel.VisibilityChanged -= this.PanelOnVisibilityChanged;
			}
			base.OnDisable();
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

		private void PanelOnVisibilityChanged(IDebugPanelService debugPanelService, bool b)
		{
			if (!b)
			{
				this.SetSelectionModeEnabled(false);
				this.Refresh();
			}
			else if (b && base.CachedGameObject.activeInHierarchy)
			{
				this.Refresh();
			}
			if (this._optionCanvas != null)
			{
				this._optionCanvas.enabled = b;
			}
		}

		public void SetSelectionModeEnabled(bool isEnabled)
		{
			if (this._selectionModeEnabled == isEnabled)
			{
				return;
			}
			this._selectionModeEnabled = isEnabled;
			this.PinButton.isOn = isEnabled;
			this.PinPromptText.SetActive(isEnabled);
			foreach (KeyValuePair<OptionDefinition, OptionsControlBase> keyValuePair in this._options)
			{
				keyValuePair.Value.SelectionModeEnabled = isEnabled;
				if (isEnabled)
				{
					keyValuePair.Value.IsSelected = Service.PinnedUI.HasPinned(keyValuePair.Key);
				}
			}
			foreach (OptionsTabController.CategoryInstance categoryInstance in this._categories)
			{
				categoryInstance.CategoryGroup.SelectionModeEnabled = isEnabled;
			}
			this.RefreshCategorySelection();
			if (isEnabled)
			{
				return;
			}
		}

		private void Refresh()
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				this._controls[i].Refresh();
				this._controls[i].IsSelected = Service.PinnedUI.HasPinned(this._controls[i].Option);
			}
		}

		private void CommitPinnedOptions()
		{
			foreach (KeyValuePair<OptionDefinition, OptionsControlBase> keyValuePair in this._options)
			{
				OptionsControlBase value = keyValuePair.Value;
				if (value.IsSelected && !Service.PinnedUI.HasPinned(keyValuePair.Key))
				{
					Service.PinnedUI.Pin(keyValuePair.Key, -1);
				}
				else if (!value.IsSelected && Service.PinnedUI.HasPinned(keyValuePair.Key))
				{
					Service.PinnedUI.Unpin(keyValuePair.Key);
				}
			}
		}

		private void RefreshCategorySelection()
		{
			this._isTogglingCategory = true;
			foreach (OptionsTabController.CategoryInstance categoryInstance in this._categories)
			{
				bool isSelected = true;
				for (int i = 0; i < categoryInstance.Options.Count; i++)
				{
					if (!categoryInstance.Options[i].IsSelected)
					{
						isSelected = false;
						break;
					}
				}
				categoryInstance.CategoryGroup.IsSelected = isSelected;
			}
			this._isTogglingCategory = false;
		}

		private void OnOptionSelectionToggle(bool selected)
		{
			if (!this._isTogglingCategory)
			{
				this.RefreshCategorySelection();
				this.CommitPinnedOptions();
			}
		}

		private void OnCategorySelectionToggle(OptionsTabController.CategoryInstance category, bool selected)
		{
			this._isTogglingCategory = true;
			for (int i = 0; i < category.Options.Count; i++)
			{
				category.Options[i].IsSelected = selected;
			}
			this._isTogglingCategory = false;
			this.CommitPinnedOptions();
		}

		protected void Populate()
		{
			Dictionary<string, List<OptionDefinition>> dictionary = new Dictionary<string, List<OptionDefinition>>();
			foreach (OptionDefinition optionDefinition in Service.Options.Options)
			{
				List<OptionDefinition> list;
				if (!dictionary.TryGetValue(optionDefinition.Category, out list))
				{
					list = new List<OptionDefinition>();
					dictionary.Add(optionDefinition.Category, list);
				}
				list.Add(optionDefinition);
			}
			bool flag = false;
			foreach (KeyValuePair<string, List<OptionDefinition>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Count != 0)
				{
					flag = true;
					this.CreateCategory(keyValuePair.Key, keyValuePair.Value);
				}
			}
			if (flag)
			{
				this.NoOptionsNotice.SetActive(false);
			}
		}

		protected void CreateCategory(string title, List<OptionDefinition> options)
		{
			options.Sort((OptionDefinition d1, OptionDefinition d2) => d1.SortPriority.CompareTo(d2.SortPriority));
			CategoryGroup categoryGroup = SRInstantiate.Instantiate<CategoryGroup>(this.CategoryGroupPrefab);
			OptionsTabController.CategoryInstance categoryInstance = new OptionsTabController.CategoryInstance(categoryGroup);
			this._categories.Add(categoryInstance);
			categoryGroup.CachedTransform.SetParent(this.ContentContainer, false);
			categoryGroup.Header.text = title;
			categoryGroup.SelectionModeEnabled = false;
			categoryInstance.CategoryGroup.SelectionToggle.onValueChanged.AddListener(delegate(bool b)
			{
				this.OnCategorySelectionToggle(categoryInstance, b);
			});
			foreach (OptionDefinition optionDefinition in options)
			{
				OptionsControlBase optionsControlBase = OptionControlFactory.CreateControl(optionDefinition, title);
				if (optionsControlBase == null)
				{
					UnityEngine.Debug.LogError("[SRDebugger.OptionsTab] Failed to create option control for {0}".Fmt(new object[]
					{
						optionDefinition.Name
					}));
				}
				else
				{
					categoryInstance.Options.Add(optionsControlBase);
					optionsControlBase.CachedTransform.SetParent(categoryGroup.Container, false);
					optionsControlBase.IsSelected = Service.PinnedUI.HasPinned(optionDefinition);
					optionsControlBase.SelectionModeEnabled = false;
					optionsControlBase.SelectionModeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnOptionSelectionToggle));
					this._options.Add(optionDefinition, optionsControlBase);
					this._controls.Add(optionsControlBase);
				}
			}
		}

		private void Clear()
		{
			foreach (OptionsTabController.CategoryInstance categoryInstance in this._categories)
			{
				UnityEngine.Object.Destroy(categoryInstance.CategoryGroup.gameObject);
			}
			this._categories.Clear();
			this._controls.Clear();
			this._options.Clear();
		}

		private readonly List<OptionsControlBase> _controls = new List<OptionsControlBase>();

		private readonly List<OptionsTabController.CategoryInstance> _categories = new List<OptionsTabController.CategoryInstance>();

		private readonly Dictionary<OptionDefinition, OptionsControlBase> _options = new Dictionary<OptionDefinition, OptionsControlBase>();

		private bool _queueRefresh;

		private bool _selectionModeEnabled;

		private Canvas _optionCanvas;

		[RequiredField]
		public ActionControl ActionControlPrefab;

		[RequiredField]
		public CategoryGroup CategoryGroupPrefab;

		[RequiredField]
		public RectTransform ContentContainer;

		[RequiredField]
		public GameObject NoOptionsNotice;

		[RequiredField]
		public Toggle PinButton;

		[RequiredField]
		public GameObject PinPromptSpacer;

		[RequiredField]
		public GameObject PinPromptText;

		private bool _isTogglingCategory;

		private class CategoryInstance
		{
			public CategoryInstance(CategoryGroup group)
			{
				this.CategoryGroup = group;
			}

			public CategoryGroup CategoryGroup { get; private set; }

			public readonly List<OptionsControlBase> Options = new List<OptionsControlBase>();
		}
	}
}
