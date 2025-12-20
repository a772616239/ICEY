using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/Unselectable")]
	public sealed class Unselectable : SRMonoBehaviour, IEventSystemHandler, ISelectHandler
	{
		public void OnSelect(BaseEventData eventData)
		{
			this._suspectedSelected = true;
		}

		private void Update()
		{
			if (!this._suspectedSelected)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject == base.CachedGameObject)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			this._suspectedSelected = false;
		}

		private bool _suspectedSelected;
	}
}
