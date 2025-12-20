using System;
using UnityEngine;

namespace I2.Loc
{
	public class LanguagePopup : MonoBehaviour
	{
		private void Start()
		{
			UIPopupList component = base.GetComponent<UIPopupList>();
			component.items = this.Source.GetLanguages(true);
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnValueChange));
			int num = component.items.IndexOf(LocalizationManager.CurrentLanguage);
			component.value = component.items[(num < 0) ? 0 : num];
		}

		public void OnValueChange()
		{
			LocalizationManager.CurrentLanguage = UIPopupList.current.value;
		}

		public LanguageSource Source;
	}
}
