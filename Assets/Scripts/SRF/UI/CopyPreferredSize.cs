using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	[AddComponentMenu("SRF/UI/Copy Preferred Size")]
	public class CopyPreferredSize : LayoutElement
	{
		public override float preferredWidth
		{
			get
			{
				if (this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetPreferredWidth(this.CopySource) + this.PaddingWidth;
			}
		}

		public override float preferredHeight
		{
			get
			{
				if (this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return LayoutUtility.GetPreferredHeight(this.CopySource) + this.PaddingHeight;
			}
		}

		public override int layoutPriority
		{
			get
			{
				return 2;
			}
		}

		public RectTransform CopySource;

		public float PaddingHeight;

		public float PaddingWidth;
	}
}
