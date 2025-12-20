using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	[AddComponentMenu("SRF/UI/Copy Size Into Layout Element")]
	public class CopySizeIntoLayoutElement : LayoutElement
	{
		public override float preferredWidth
		{
			get
			{
				if (!this.SetPreferredSize || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return this.CopySource.rect.width + this.PaddingWidth;
			}
		}

		public override float preferredHeight
		{
			get
			{
				if (!this.SetPreferredSize || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return this.CopySource.rect.height + this.PaddingHeight;
			}
		}

		public override float minWidth
		{
			get
			{
				if (!this.SetMinimumSize || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return this.CopySource.rect.width + this.PaddingWidth;
			}
		}

		public override float minHeight
		{
			get
			{
				if (!this.SetMinimumSize || this.CopySource == null || !this.IsActive())
				{
					return -1f;
				}
				return this.CopySource.rect.height + this.PaddingHeight;
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

		public bool SetPreferredSize;

		public bool SetMinimumSize;
	}
}
