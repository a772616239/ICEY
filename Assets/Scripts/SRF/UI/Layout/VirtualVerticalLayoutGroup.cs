using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI.Layout
{
	[AddComponentMenu("SRF/UI/Layout/VerticalLayoutGroup (Virtualizing)")]
	public class VirtualVerticalLayoutGroup : LayoutGroup, IEventSystemHandler, IPointerClickHandler
	{
		public VirtualVerticalLayoutGroup.SelectedItemChangedEvent SelectedItemChanged
		{
			get
			{
				return this._selectedItemChanged;
			}
			set
			{
				this._selectedItemChanged = value;
			}
		}

		public object SelectedItem
		{
			get
			{
				return this._selectedItem;
			}
			set
			{
				if (this._selectedItem == value || !this.EnableSelection)
				{
					return;
				}
				int num = (value != null) ? this._itemList.IndexOf(value) : -1;
				if (value != null && num < 0)
				{
					throw new InvalidOperationException("Cannot select item not present in layout");
				}
				if (this._selectedItem != null)
				{
					this.InvalidateItem(this._selectedIndex);
				}
				this._selectedItem = value;
				this._selectedIndex = num;
				if (this._selectedItem != null)
				{
					this.InvalidateItem(this._selectedIndex);
				}
				this.SetDirty();
				if (this._selectedItemChanged != null)
				{
					this._selectedItemChanged.Invoke(this._selectedItem);
				}
			}
		}

		public override float minHeight
		{
			get
			{
				return (float)this._itemList.Count * this.ItemHeight + (float)base.padding.top + (float)base.padding.bottom + this.Spacing * (float)this._itemList.Count;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.EnableSelection)
			{
				return;
			}
			GameObject gameObject = eventData.pointerPressRaycast.gameObject;
			if (gameObject == null)
			{
				return;
			}
			Vector3 position = gameObject.transform.position;
			int num = Mathf.FloorToInt(Mathf.Abs(base.rectTransform.InverseTransformPoint(position).y) / this.ItemHeight);
			if (num >= 0 && num < this._itemList.Count)
			{
				this.SelectedItem = this._itemList[num];
			}
			else
			{
				this.SelectedItem = null;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollRectValueChanged));
			Component component = this.ItemPrefab.GetComponent(typeof(IVirtualView));
			if (component == null)
			{
				UnityEngine.Debug.LogWarning("[VirtualVerticalLayoutGroup] ItemPrefab does not have a component inheriting from IVirtualView, so no data binding can occur");
			}
		}

		private void OnScrollRectValueChanged(Vector2 d)
		{
			if (d.y < 0f || d.y > 1f)
			{
				this._scrollRect.verticalNormalizedPosition = Mathf.Clamp01(d.y);
			}
			this.SetDirty();
		}

		protected override void Start()
		{
			base.Start();
			this.ScrollUpdate();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		protected void Update()
		{
			if (!this.AlignBottom && !this.AlignTop)
			{
				UnityEngine.Debug.LogWarning("[VirtualVerticalLayoutGroup] Only Lower or Upper alignment is supported.", this);
				base.childAlignment = TextAnchor.UpperLeft;
			}
			if (this.SelectedItem != null && !this._itemList.Contains(this.SelectedItem))
			{
				this.SelectedItem = null;
			}
			if (this._isDirty)
			{
				this._isDirty = false;
				this.ScrollUpdate();
			}
		}

		protected void InvalidateItem(int itemIndex)
		{
			if (!this._visibleItemList.Contains(itemIndex))
			{
				return;
			}
			this._visibleItemList.Remove(itemIndex);
			for (int i = 0; i < this._visibleRows.Count; i++)
			{
				if (this._visibleRows[i].Index == itemIndex)
				{
					this.RecycleRow(this._visibleRows[i]);
					this._visibleRows.RemoveAt(i);
					break;
				}
			}
		}

		protected void RefreshIndexCache()
		{
			for (int i = 0; i < this._visibleRows.Count; i++)
			{
				this._visibleRows[i].Index = this._itemList.IndexOf(this._visibleRows[i].Data);
			}
		}

		protected void ScrollUpdate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			float y = base.rectTransform.anchoredPosition.y;
			float height = ((RectTransform)this.ScrollRect.transform).rect.height;
			int num = Mathf.FloorToInt(y / (this.ItemHeight + this.Spacing));
			int num2 = Mathf.CeilToInt((y + height) / (this.ItemHeight + this.Spacing));
			num -= this.RowPadding;
			num2 += this.RowPadding;
			num = Mathf.Max(0, num);
			num2 = Mathf.Min(this._itemList.Count, num2);
			bool flag = false;
			for (int i = 0; i < this._visibleRows.Count; i++)
			{
				VirtualVerticalLayoutGroup.Row row = this._visibleRows[i];
				if (row.Index < num || row.Index > num2)
				{
					this._visibleItemList.Remove(row.Index);
					this._visibleRows.Remove(row);
					this.RecycleRow(row);
					flag = true;
				}
			}
			for (int j = num; j < num2; j++)
			{
				if (j >= this._itemList.Count)
				{
					break;
				}
				if (!this._visibleItemList.Contains(j))
				{
					VirtualVerticalLayoutGroup.Row row2 = this.GetRow(j);
					this._visibleRows.Add(row2);
					this._visibleItemList.Add(j);
					flag = true;
				}
			}
			if (flag || this._visibleItemCount != this._visibleRows.Count)
			{
				LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			}
			this._visibleItemCount = this._visibleRows.Count;
		}

		public override void CalculateLayoutInputVertical()
		{
			base.SetLayoutInputForAxis(this.minHeight, this.minHeight, -1f, 1);
		}

		public override void SetLayoutHorizontal()
		{
			float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
			for (int i = 0; i < this._visibleRows.Count; i++)
			{
				VirtualVerticalLayoutGroup.Row row = this._visibleRows[i];
				base.SetChildAlongAxis(row.Rect, 0, (float)base.padding.left, num);
			}
			for (int j = 0; j < this._rowCache.Count; j++)
			{
				VirtualVerticalLayoutGroup.Row row2 = this._rowCache[j];
				base.SetChildAlongAxis(row2.Rect, 0, -num - (float)base.padding.left, num);
			}
		}

		public override void SetLayoutVertical()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			for (int i = 0; i < this._visibleRows.Count; i++)
			{
				VirtualVerticalLayoutGroup.Row row = this._visibleRows[i];
				base.SetChildAlongAxis(row.Rect, 1, (float)row.Index * this.ItemHeight + (float)base.padding.top + this.Spacing * (float)row.Index, this.ItemHeight);
			}
		}

		private new void SetDirty()
		{
			base.SetDirty();
			if (!this.IsActive())
			{
				return;
			}
			this._isDirty = true;
		}

		public void AddItem(object item)
		{
			this._itemList.Add(item);
			this.SetDirty();
			if (this.StickToBottom && Mathf.Approximately(this.ScrollRect.verticalNormalizedPosition, 0f))
			{
				this.ScrollRect.normalizedPosition = new Vector2(0f, 0f);
			}
		}

		public void RemoveItem(object item)
		{
			if (this.SelectedItem == item)
			{
				this.SelectedItem = null;
			}
			int itemIndex = this._itemList.IndexOf(item);
			this.InvalidateItem(itemIndex);
			this._itemList.Remove(item);
			this.RefreshIndexCache();
			this.SetDirty();
		}

		public void ClearItems()
		{
			for (int i = this._visibleRows.Count - 1; i >= 0; i--)
			{
				this.InvalidateItem(this._visibleRows[i].Index);
			}
			this._itemList.Clear();
			this.SetDirty();
		}

		private ScrollRect ScrollRect
		{
			get
			{
				if (this._scrollRect == null)
				{
					this._scrollRect = base.GetComponentInParent<ScrollRect>();
				}
				return this._scrollRect;
			}
		}

		private bool AlignBottom
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.LowerLeft;
			}
		}

		private bool AlignTop
		{
			get
			{
				return base.childAlignment == TextAnchor.UpperLeft || base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.UpperRight;
			}
		}

		private float ItemHeight
		{
			get
			{
				if (this._itemHeight <= 0f)
				{
					ILayoutElement layoutElement = this.ItemPrefab.GetComponent(typeof(ILayoutElement)) as ILayoutElement;
					if (layoutElement != null)
					{
						this._itemHeight = layoutElement.preferredHeight;
					}
					else
					{
						this._itemHeight = this.ItemPrefab.rect.height;
					}
					if (this._itemHeight.ApproxZero())
					{
						UnityEngine.Debug.LogWarning("[VirtualVerticalLayoutGroup] ItemPrefab must have a preferred size greater than 0");
						this._itemHeight = 10f;
					}
				}
				return this._itemHeight;
			}
		}

		private VirtualVerticalLayoutGroup.Row GetRow(int forIndex)
		{
			if (this._rowCache.Count == 0)
			{
				VirtualVerticalLayoutGroup.Row row = this.CreateRow();
				this.PopulateRow(forIndex, row);
				return row;
			}
			object obj = this._itemList[forIndex];
			VirtualVerticalLayoutGroup.Row row2 = null;
			VirtualVerticalLayoutGroup.Row row3 = null;
			int num = forIndex % 2;
			for (int i = 0; i < this._rowCache.Count; i++)
			{
				row2 = this._rowCache[i];
				if (row2.Data == obj)
				{
					this._rowCache.RemoveAt(i);
					this.PopulateRow(forIndex, row2);
					break;
				}
				if (row2.Index % 2 == num)
				{
					row3 = row2;
				}
				row2 = null;
			}
			if (row2 == null && row3 != null)
			{
				this._rowCache.Remove(row3);
				row2 = row3;
				this.PopulateRow(forIndex, row2);
			}
			else if (row2 == null)
			{
				row2 = this._rowCache.PopLast<VirtualVerticalLayoutGroup.Row>();
				this.PopulateRow(forIndex, row2);
			}
			return row2;
		}

		private void RecycleRow(VirtualVerticalLayoutGroup.Row row)
		{
			this._rowCache.Add(row);
		}

		private void PopulateRow(int index, VirtualVerticalLayoutGroup.Row row)
		{
			row.Index = index;
			row.Data = this._itemList[index];
			row.View.SetDataContext(this._itemList[index]);
			if (this.RowStyleSheet != null || this.AltRowStyleSheet != null || this.SelectedRowStyleSheet != null)
			{
				if (this.SelectedRowStyleSheet != null && this.SelectedItem == row.Data)
				{
					row.Root.StyleSheet = this.SelectedRowStyleSheet;
				}
				else
				{
					row.Root.StyleSheet = ((index % 2 != 0) ? this.AltRowStyleSheet : this.RowStyleSheet);
				}
			}
		}

		private VirtualVerticalLayoutGroup.Row CreateRow()
		{
			VirtualVerticalLayoutGroup.Row row = new VirtualVerticalLayoutGroup.Row();
			RectTransform rectTransform = SRInstantiate.Instantiate<RectTransform>(this.ItemPrefab);
			row.Rect = rectTransform;
			row.View = (rectTransform.GetComponent(typeof(IVirtualView)) as IVirtualView);
			if (this.RowStyleSheet != null || this.AltRowStyleSheet != null || this.SelectedRowStyleSheet != null)
			{
				row.Root = rectTransform.gameObject.GetComponentOrAdd<StyleRoot>();
				row.Root.StyleSheet = this.RowStyleSheet;
			}
			rectTransform.SetParent(base.rectTransform, false);
			return row;
		}

		private readonly SRList<object> _itemList = new SRList<object>();

		private readonly SRList<int> _visibleItemList = new SRList<int>();

		private bool _isDirty;

		private SRList<VirtualVerticalLayoutGroup.Row> _rowCache = new SRList<VirtualVerticalLayoutGroup.Row>();

		private ScrollRect _scrollRect;

		private int _selectedIndex;

		private object _selectedItem;

		[SerializeField]
		private VirtualVerticalLayoutGroup.SelectedItemChangedEvent _selectedItemChanged;

		private int _visibleItemCount;

		private SRList<VirtualVerticalLayoutGroup.Row> _visibleRows = new SRList<VirtualVerticalLayoutGroup.Row>();

		public StyleSheet AltRowStyleSheet;

		public bool EnableSelection = true;

		public RectTransform ItemPrefab;

		public int RowPadding = 2;

		public StyleSheet RowStyleSheet;

		public StyleSheet SelectedRowStyleSheet;

		public float Spacing;

		public bool StickToBottom = true;

		private float _itemHeight = -1f;

		[Serializable]
		public class SelectedItemChangedEvent : UnityEvent<object>
		{
		}

		[Serializable]
		private class Row
		{
			public object Data;

			public int Index;

			public RectTransform Rect;

			public StyleRoot Root;

			public IVirtualView View;
		}
	}
}
