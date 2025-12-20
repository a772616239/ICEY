using System;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class CategoryGroup : SRMonoBehaviourEx
	{
		public bool IsSelected
		{
			get
			{
				return this.SelectionToggle.isOn;
			}
			set
			{
				this.SelectionToggle.isOn = value;
				if (this.SelectionToggle.graphic != null)
				{
					this.SelectionToggle.graphic.CrossFadeAlpha((!value) ? 0f : ((!this._selectionModeEnabled) ? 0.2f : 1f), 0f, true);
				}
			}
		}

		public bool SelectionModeEnabled
		{
			get
			{
				return this._selectionModeEnabled;
			}
			set
			{
				if (value == this._selectionModeEnabled)
				{
					return;
				}
				this._selectionModeEnabled = value;
				for (int i = 0; i < this.EnabledDuringSelectionMode.Length; i++)
				{
					this.EnabledDuringSelectionMode[i].SetActive(this._selectionModeEnabled);
				}
			}
		}

		[RequiredField]
		public RectTransform Container;

		[RequiredField]
		public Text Header;

		[RequiredField]
		public GameObject Background;

		[RequiredField]
		public Toggle SelectionToggle;

		public GameObject[] EnabledDuringSelectionMode = new GameObject[0];

		private bool _selectionModeEnabled = true;
	}
}
