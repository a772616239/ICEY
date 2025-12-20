using System;
using System.Collections.Generic;
using UnityEngine;

namespace SRF.UI
{
	[Serializable]
	public class StyleSheet : ScriptableObject
	{
		public Style GetStyle(string key, bool searchParent = true)
		{
			int num = this._keys.IndexOf(key);
			if (num >= 0)
			{
				return this._styles[num];
			}
			if (searchParent && this.Parent != null)
			{
				return this.Parent.GetStyle(key, true);
			}
			return null;
		}

		[SerializeField]
		private List<string> _keys = new List<string>();

		[SerializeField]
		private List<Style> _styles = new List<Style>();

		[SerializeField]
		public StyleSheet Parent;
	}
}
