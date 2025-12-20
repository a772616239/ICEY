using System;
using SRDebugger.Internal;
using SRF;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public abstract class OptionsControlBase : SRMonoBehaviourEx
	{
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
				this.SelectionModeToggle.gameObject.SetActive(this._selectionModeEnabled);
				if (this.SelectionModeToggle.graphic != null)
				{
					this.SelectionModeToggle.graphic.CrossFadeAlpha((!this.IsSelected) ? 0f : ((!this._selectionModeEnabled) ? 0.2f : 1f), 0f, true);
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return this.SelectionModeToggle.isOn;
			}
			set
			{
				this.SelectionModeToggle.isOn = value;
				if (this.SelectionModeToggle.graphic != null)
				{
					this.SelectionModeToggle.graphic.CrossFadeAlpha((!value) ? 0f : ((!this._selectionModeEnabled) ? 0.2f : 1f), 0f, true);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this.IsSelected = false;
			this.SelectionModeToggle.gameObject.SetActive(false);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.SelectionModeToggle.graphic != null)
			{
				this.SelectionModeToggle.graphic.CrossFadeAlpha((!this.IsSelected) ? 0f : ((!this._selectionModeEnabled) ? 0.2f : 1f), 0f, true);
			}
		}

		public virtual void Refresh()
		{
		}

		private bool _selectionModeEnabled;

		[RequiredField]
		public Toggle SelectionModeToggle;

		public OptionDefinition Option;
	}
}
