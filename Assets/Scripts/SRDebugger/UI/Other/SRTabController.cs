using System;
using System.Collections.Generic;
using System.Diagnostics;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class SRTabController : SRMonoBehaviourEx
	{
		public SRTab ActiveTab
		{
			get
			{
				return this._activeTab;
			}
			set
			{
				this.MakeActive(value);
			}
		}

		public IList<SRTab> Tabs
		{
			get
			{
				return this._tabs.AsReadOnly();
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<SRTabController, SRTab> ActiveTabChanged;

		public void AddTab(SRTab tab, bool visibleInSidebar = true)
		{
			tab.CachedTransform.SetParent(this.TabContentsContainer, false);
			tab.CachedGameObject.SetActive(false);
			if (visibleInSidebar)
			{
				SRTabButton srtabButton = SRInstantiate.Instantiate<SRTabButton>(this.TabButtonPrefab);
				srtabButton.CachedTransform.SetParent(this.TabButtonContainer, false);
				srtabButton.TitleText.text = tab.Title.ToUpper();
				if (tab.IconExtraContent != null)
				{
					RectTransform rectTransform = SRInstantiate.Instantiate<RectTransform>(tab.IconExtraContent);
					rectTransform.SetParent(srtabButton.ExtraContentContainer, false);
				}
				srtabButton.IconStyleComponent.StyleKey = tab.IconStyleKey;
				srtabButton.IsActive = false;
				srtabButton.Button.onClick.AddListener(delegate()
				{
					this.MakeActive(tab);
				});
				tab.TabButton = srtabButton;
			}
			this._tabs.Add(tab);
			this.SortTabs();
			if (this._tabs.Count == 1)
			{
				this.ActiveTab = tab;
			}
		}

		private void MakeActive(SRTab tab)
		{
			if (!this._tabs.Contains(tab))
			{
				throw new ArgumentException("tab is not a member of this tab controller", "tab");
			}
			if (this._activeTab != null)
			{
				this._activeTab.CachedGameObject.SetActive(false);
				if (this._activeTab.TabButton != null)
				{
					this._activeTab.TabButton.IsActive = false;
				}
				if (this._activeTab.HeaderExtraContent != null)
				{
					this._activeTab.HeaderExtraContent.gameObject.SetActive(false);
				}
			}
			this._activeTab = tab;
			if (this._activeTab != null)
			{
				this._activeTab.CachedGameObject.SetActive(true);
				this.TabHeaderText.text = this._activeTab.LongTitle;
				if (this._activeTab.TabButton != null)
				{
					this._activeTab.TabButton.IsActive = true;
				}
				if (this._activeTab.HeaderExtraContent != null)
				{
					this._activeTab.HeaderExtraContent.SetParent(this.TabHeaderContentContainer, false);
					this._activeTab.HeaderExtraContent.gameObject.SetActive(true);
				}
			}
			if (this.ActiveTabChanged != null)
			{
				this.ActiveTabChanged(this, this._activeTab);
			}
		}

		private void SortTabs()
		{
			this._tabs.Sort((SRTab t1, SRTab t2) => t1.SortIndex.CompareTo(t2.SortIndex));
			for (int i = 0; i < this._tabs.Count; i++)
			{
				if (this._tabs[i].TabButton != null)
				{
					this._tabs[i].TabButton.CachedTransform.SetSiblingIndex(i);
				}
			}
		}

		private readonly SRList<SRTab> _tabs = new SRList<SRTab>();

		private SRTab _activeTab;

		[RequiredField]
		public RectTransform TabButtonContainer;

		[RequiredField]
		public SRTabButton TabButtonPrefab;

		[RequiredField]
		public RectTransform TabContentsContainer;

		[RequiredField]
		public RectTransform TabHeaderContentContainer;

		[RequiredField]
		public Text TabHeaderText;
	}
}
