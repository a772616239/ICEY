using System;
using UnityEngine;

namespace SRF.UI
{
	[Serializable]
	public class Style
	{
		public Style Copy()
		{
			Style style = new Style();
			style.CopyFrom(this);
			return style;
		}

		public void CopyFrom(Style style)
		{
			this.Image = style.Image;
			this.NormalColor = style.NormalColor;
			this.HoverColor = style.HoverColor;
			this.ActiveColor = style.ActiveColor;
			this.DisabledColor = style.DisabledColor;
		}

		public Color ActiveColor = Color.white;

		public Color DisabledColor = Color.white;

		public Color HoverColor = Color.white;

		public Sprite Image;

		public Color NormalColor = Color.white;
	}
}
