using System;
using System.Linq;
using SRDebugger.UI.Other;
using SRF;
using UnityEngine;

namespace SRDebugger.Scripts
{
	public class DebuggerTabController : SRMonoBehaviourEx
	{
		public DefaultTabs? ActiveTab
		{
			get
			{
				string key = this.TabController.ActiveTab.Key;
				if (string.IsNullOrEmpty(key))
				{
					return null;
				}
				object obj = Enum.Parse(typeof(DefaultTabs), key);
				if (!Enum.IsDefined(typeof(DefaultTabs), obj))
				{
					return null;
				}
				return new DefaultTabs?((DefaultTabs)obj);
			}
		}

		protected override void Start()
		{
			base.Start();
			this._hasStarted = true;
			SRTab[] array = Resources.LoadAll<SRTab>("SRDebugger/UI/Prefabs/Tabs");
			string[] names = Enum.GetNames(typeof(DefaultTabs));
			foreach (SRTab srtab in array)
			{
				IEnableTab enableTab = srtab.GetComponent(typeof(IEnableTab)) as IEnableTab;
				if (enableTab == null || enableTab.IsEnabled)
				{
					if (names.Contains(srtab.Key))
					{
						object obj = Enum.Parse(typeof(DefaultTabs), srtab.Key);
						if (Enum.IsDefined(typeof(DefaultTabs), obj) && Settings.Instance.DisabledTabs.Contains((DefaultTabs)obj))
						{
							goto IL_D6;
						}
					}
					this.TabController.AddTab(SRInstantiate.Instantiate<SRTab>(srtab), true);
				}
				IL_D6:;
			}
			if (this.AboutTab != null)
			{
				this._aboutTabInstance = SRInstantiate.Instantiate<SRTab>(this.AboutTab);
				this.TabController.AddTab(this._aboutTabInstance, false);
			}
			DefaultTabs? activeTab = this._activeTab;
			DefaultTabs tab = (activeTab == null) ? Settings.Instance.DefaultTab : activeTab.Value;
			if (!this.OpenTab(tab))
			{
				this.TabController.ActiveTab = this.TabController.Tabs.FirstOrDefault<SRTab>();
			}
		}

		public bool OpenTab(DefaultTabs tab)
		{
			if (!this._hasStarted)
			{
				this._activeTab = new DefaultTabs?(tab);
				return true;
			}
			string b = tab.ToString();
			foreach (SRTab srtab in this.TabController.Tabs)
			{
				if (srtab.Key == b)
				{
					this.TabController.ActiveTab = srtab;
					return true;
				}
			}
			return false;
		}

		public void ShowAboutTab()
		{
			if (this._aboutTabInstance != null)
			{
				this.TabController.ActiveTab = this._aboutTabInstance;
			}
		}

		private SRTab _aboutTabInstance;

		private DefaultTabs? _activeTab;

		private bool _hasStarted;

		public SRTab AboutTab;

		[RequiredField]
		public SRTabController TabController;
	}
}
