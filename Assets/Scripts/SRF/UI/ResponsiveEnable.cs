using System;
using UnityEngine;

namespace SRF.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	[AddComponentMenu("SRF/UI/Responsive (Enable)")]
	public class ResponsiveEnable : ResponsiveBase
	{
		protected override void Refresh()
		{
			Rect rect = base.RectTransform.rect;
			for (int i = 0; i < this.Entries.Length; i++)
			{
				ResponsiveEnable.Entry entry = this.Entries[i];
				bool flag = true;
				ResponsiveEnable.Modes mode = entry.Mode;
				if (mode != ResponsiveEnable.Modes.EnableAbove)
				{
					if (mode != ResponsiveEnable.Modes.EnableBelow)
					{
						throw new IndexOutOfRangeException();
					}
					if (entry.ThresholdHeight > 0f)
					{
						flag = (rect.height <= entry.ThresholdHeight && flag);
					}
					if (entry.ThresholdWidth > 0f)
					{
						flag = (rect.width <= entry.ThresholdWidth && flag);
					}
				}
				else
				{
					if (entry.ThresholdHeight > 0f)
					{
						flag = (rect.height >= entry.ThresholdHeight && flag);
					}
					if (entry.ThresholdWidth > 0f)
					{
						flag = (rect.width >= entry.ThresholdWidth && flag);
					}
				}
				if (entry.GameObjects != null)
				{
					for (int j = 0; j < entry.GameObjects.Length; j++)
					{
						GameObject gameObject = entry.GameObjects[j];
						if (gameObject != null)
						{
							gameObject.SetActive(flag);
						}
					}
				}
				if (entry.Components != null)
				{
					for (int k = 0; k < entry.Components.Length; k++)
					{
						Behaviour behaviour = entry.Components[k];
						if (behaviour != null)
						{
							behaviour.enabled = flag;
						}
					}
				}
			}
		}

		public ResponsiveEnable.Entry[] Entries = new ResponsiveEnable.Entry[0];

		public enum Modes
		{
			EnableAbove,
			EnableBelow
		}

		[Serializable]
		public struct Entry
		{
			public Behaviour[] Components;

			public GameObject[] GameObjects;

			public ResponsiveEnable.Modes Mode;

			public float ThresholdHeight;

			public float ThresholdWidth;
		}
	}
}
