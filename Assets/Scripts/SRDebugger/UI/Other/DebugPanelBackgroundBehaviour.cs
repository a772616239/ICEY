using System;
using SRF;
using SRF.UI;
using UnityEngine;

namespace SRDebugger.UI.Other
{
	[RequireComponent(typeof(StyleComponent))]
	public class DebugPanelBackgroundBehaviour : SRMonoBehaviour
	{
		private void Awake()
		{
			this._styleComponent = base.GetComponent<StyleComponent>();
			this._defaultKey = this._styleComponent.StyleKey;
			this.Update();
		}

		private void Update()
		{
			if (!this._isTransparent && Settings.Instance.EnableBackgroundTransparency)
			{
				this._styleComponent.StyleKey = this.TransparentStyleKey;
				this._isTransparent = true;
			}
			else if (this._isTransparent && !Settings.Instance.EnableBackgroundTransparency)
			{
				this._styleComponent.StyleKey = this._defaultKey;
				this._isTransparent = false;
			}
		}

		private string _defaultKey;

		private bool _isTransparent;

		private StyleComponent _styleComponent;

		public string TransparentStyleKey = string.Empty;
	}
}
