using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("SRF/UI/Copy Layout Element")]
	[ExecuteInEditMode]
	public class CopyLayoutElement : UIBehaviour, ILayoutElement
	{
		public float preferredWidth
		{
			get
			{
				if (!this.CopyPreferredWidth || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetPreferredWidth(this.CopySource) + this.PaddingPreferredWidth;
			}
		}

		public float preferredHeight
		{
			get
			{
				if (!this.CopyPreferredHeight || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetPreferredHeight(this.CopySource) + this.PaddingPreferredHeight;
			}
		}

		public float minWidth
		{
			get
			{
				if (!this.CopyMinWidth || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetMinWidth(this.CopySource) + this.PaddingMinWidth;
			}
		}

		public float minHeight
		{
			get
			{
				if (!this.CopyMinHeight || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetMinHeight(this.CopySource) + this.PaddingMinHeight;
			}
		}

		public int layoutPriority
		{
			get
			{
				return 2;
			}
		}

		public float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		public float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
		}

		public void CalculateLayoutInputVertical()
		{
		}

		public bool CopyMinHeight;

		public bool CopyMinWidth;

		public bool CopyPreferredHeight;

		public bool CopyPreferredWidth;

		public RectTransform CopySource;

		public float PaddingMinHeight;

		public float PaddingMinWidth;

		public float PaddingPreferredHeight;

		public float PaddingPreferredWidth;
	}
}
