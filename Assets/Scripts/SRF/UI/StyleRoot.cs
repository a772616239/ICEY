using System;
using UnityEngine;

namespace SRF.UI
{
	[ExecuteInEditMode]
	[AddComponentMenu("SRF/UI/Style Root")]
	public sealed class StyleRoot : SRMonoBehaviour
	{
		public Style GetStyle(string key)
		{
			if (this.StyleSheet == null)
			{
				UnityEngine.Debug.LogWarning("[StyleRoot] StyleSheet is not set.", this);
				return null;
			}
			return this.StyleSheet.GetStyle(key, true);
		}

		private void OnEnable()
		{
			this._activeStyleSheet = null;
			if (this.StyleSheet != null)
			{
				this.OnStyleSheetChanged();
			}
		}

		private void OnDisable()
		{
			this.OnStyleSheetChanged();
		}

		private void Update()
		{
			if (this._activeStyleSheet != this.StyleSheet)
			{
				this.OnStyleSheetChanged();
			}
		}

		private void OnStyleSheetChanged()
		{
			this._activeStyleSheet = this.StyleSheet;
			base.BroadcastMessage("SRStyleDirty", SendMessageOptions.DontRequireReceiver);
		}

		public void SetDirty()
		{
			this._activeStyleSheet = null;
		}

		private StyleSheet _activeStyleSheet;

		public StyleSheet StyleSheet;
	}
}
