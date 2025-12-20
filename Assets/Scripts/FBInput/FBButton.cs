using System;
using UnityEngine;

namespace FBInput
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(UIButton))]
	public class FBButton : BaseBehaviour
	{
		public bool IsPressed
		{
			get
			{
				return this._uiButton.state == UIButtonColor.State.Pressed;
			}
		}

		public bool IsActive
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public string Text
		{
			get
			{
				return (!(this._uiLabel != null)) ? string.Empty : this._uiLabel.text;
			}
			set
			{
				if (this._uiLabel != null)
				{
					this._uiLabel.text = value;
				}
			}
		}

		[SerializeField]
		private UIButton _uiButton;

		[SerializeField]
		private UILabel _uiLabel;
	}
}
