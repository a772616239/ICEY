using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI.Layout
{
	[AddComponentMenu("SRF/UI/Layout/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		protected bool IsCenterAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.UpperCenter;
			}
		}

		protected bool IsRightAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.UpperRight;
			}
		}

		protected bool IsMiddleAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.MiddleCenter;
			}
		}

		protected bool IsLowerAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerLeft || base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.LowerCenter;
			}
		}

		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float totalMin = this.GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
			base.SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
		}

		public override void SetLayoutHorizontal()
		{
			this.SetLayout(base.rectTransform.rect.width, 0, false);
		}

		public override void SetLayoutVertical()
		{
			this.SetLayout(base.rectTransform.rect.width, 1, false);
		}

		public override void CalculateLayoutInputVertical()
		{
			this._layoutHeight = this.SetLayout(base.rectTransform.rect.width, 1, true);
		}

		public float SetLayout(float width, int axis, bool layoutInput)
		{
			float height = base.rectTransform.rect.height;
			float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
			float num2 = (!this.IsLowerAlign) ? ((float)base.padding.top) : ((float)base.padding.bottom);
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int index = (!this.IsLowerAlign) ? i : (base.rectChildren.Count - 1 - i);
				RectTransform rectTransform = base.rectChildren[index];
				float num5 = LayoutUtility.GetPreferredSize(rectTransform, 0);
				float preferredSize = LayoutUtility.GetPreferredSize(rectTransform, 1);
				num5 = Mathf.Min(num5, num);
				if (this._rowList.Count > 0)
				{
					num3 += this.Spacing;
				}
				if (num3 + num5 > num)
				{
					num3 -= this.Spacing;
					if (!layoutInput)
					{
						float yOffset = this.CalculateRowVerticalOffset(height, num2, num4);
						this.LayoutRow(this._rowList, num3, num4, num, (float)base.padding.left, yOffset, axis);
					}
					this._rowList.Clear();
					num2 += num4;
					num2 += this.Spacing;
					num4 = 0f;
					num3 = 0f;
				}
				num3 += num5;
				this._rowList.Add(rectTransform);
				if (preferredSize > num4)
				{
					num4 = preferredSize;
				}
			}
			if (!layoutInput)
			{
				float yOffset2 = this.CalculateRowVerticalOffset(height, num2, num4);
				this.LayoutRow(this._rowList, num3, num4, num, (float)base.padding.left, yOffset2, axis);
			}
			this._rowList.Clear();
			num2 += num4;
			num2 += (float)((!this.IsLowerAlign) ? base.padding.bottom : base.padding.top);
			if (layoutInput && axis == 1)
			{
				base.SetLayoutInputForAxis(num2, num2, -1f, axis);
			}
			return num2;
		}

		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			float result;
			if (this.IsLowerAlign)
			{
				result = groupHeight - yOffset - currentRowHeight;
			}
			else if (this.IsMiddleAlign)
			{
				result = groupHeight * 0.5f - this._layoutHeight * 0.5f + yOffset;
			}
			else
			{
				result = yOffset;
			}
			return result;
		}

		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			float num = xOffset;
			if (!this.ChildForceExpandWidth && this.IsCenterAlign)
			{
				num += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!this.ChildForceExpandWidth && this.IsRightAlign)
			{
				num += maxWidth - rowWidth;
			}
			float num2 = 0f;
			if (this.ChildForceExpandWidth)
			{
				int num3 = 0;
				for (int i = 0; i < this._rowList.Count; i++)
				{
					if (LayoutUtility.GetFlexibleWidth(this._rowList[i]) > 0f)
					{
						num3++;
					}
				}
				if (num3 > 0)
				{
					num2 = (maxWidth - rowWidth) / (float)num3;
				}
			}
			for (int j = 0; j < this._rowList.Count; j++)
			{
				int index = (!this.IsLowerAlign) ? j : (this._rowList.Count - 1 - j);
				RectTransform rect = this._rowList[index];
				float num4 = LayoutUtility.GetPreferredSize(rect, 0);
				if (LayoutUtility.GetFlexibleWidth(rect) > 0f)
				{
					num4 += num2;
				}
				float num5 = LayoutUtility.GetPreferredSize(rect, 1);
				if (this.ChildForceExpandHeight)
				{
					num5 = rowHeight;
				}
				num4 = Mathf.Min(num4, maxWidth);
				float num6 = yOffset;
				if (this.IsMiddleAlign)
				{
					num6 += (rowHeight - num5) * 0.5f;
				}
				else if (this.IsLowerAlign)
				{
					num6 += rowHeight - num5;
				}
				if (axis == 0)
				{
					base.SetChildAlongAxis(rect, 0, num, num4);
				}
				else
				{
					base.SetChildAlongAxis(rect, 1, num6, num5);
				}
				num += num4 + this.Spacing;
			}
		}

		public float GetGreatestMinimumChildWidth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				float minWidth = LayoutUtility.GetMinWidth(base.rectChildren[i]);
				num = Mathf.Max(minWidth, num);
			}
			return num;
		}

		private readonly IList<RectTransform> _rowList = new List<RectTransform>();

		private float _layoutHeight;

		public bool ChildForceExpandHeight;

		public bool ChildForceExpandWidth;

		public float Spacing;
	}
}
