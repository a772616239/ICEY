using System;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.Serialization;

namespace SRDebugger.UI.Other
{
	public class SRTab : SRMonoBehaviourEx
	{
		public string Title
		{
			get
			{
				return this._title;
			}
		}

		public string LongTitle
		{
			get
			{
				return string.IsNullOrEmpty(this._longTitle) ? this._title : this._longTitle;
			}
		}

		public string Key
		{
			get
			{
				return this._key;
			}
		}

		public RectTransform HeaderExtraContent;

		[Obsolete]
		[HideInInspector]
		public Sprite Icon;

		public RectTransform IconExtraContent;

		public string IconStyleKey = "Icon_Stompy";

		public int SortIndex;

		[HideInInspector]
		public SRTabButton TabButton;

		[FormerlySerializedAs("Title")]
		[SerializeField]
		private string _title;

		[SerializeField]
		private string _longTitle;

		[SerializeField]
		private string _key;
	}
}
