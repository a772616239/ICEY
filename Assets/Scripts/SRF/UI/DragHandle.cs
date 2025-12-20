using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	public class DragHandle : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		private float Mult
		{
			get
			{
				return (float)((!this.Invert) ? 1 : -1);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.Verify())
			{
				return;
			}
			this._startValue = this.GetCurrentValue();
			this._delta = 0f;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!this.Verify())
			{
				return;
			}
			float num = 0f;
			if (this.Axis == RectTransform.Axis.Horizontal)
			{
				num += eventData.delta.x;
			}
			else
			{
				num += eventData.delta.y;
			}
			if (this._canvasScaler != null)
			{
				num /= this._canvasScaler.scaleFactor;
			}
			num *= this.Mult;
			this._delta += num;
			this.SetCurrentValue(Mathf.Clamp(this._startValue + this._delta, this.GetMinSize(), this.GetMaxSize()));
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this.Verify())
			{
				return;
			}
			this.SetCurrentValue(Mathf.Max(this._startValue + this._delta, this.GetMinSize()));
			this._delta = 0f;
			this.CommitCurrentValue();
		}

		private void Start()
		{
			this.Verify();
			this._canvasScaler = base.GetComponentInParent<CanvasScaler>();
		}

		private bool Verify()
		{
			if (this.TargetLayoutElement == null && this.TargetRectTransform == null)
			{
				UnityEngine.Debug.LogWarning("DragHandle: TargetLayoutElement and TargetRectTransform are both null. Disabling behaviour.");
				base.enabled = false;
				return false;
			}
			return true;
		}

		private float GetCurrentValue()
		{
			if (this.TargetLayoutElement != null)
			{
				return (this.Axis != RectTransform.Axis.Horizontal) ? this.TargetLayoutElement.preferredHeight : this.TargetLayoutElement.preferredWidth;
			}
			if (this.TargetRectTransform != null)
			{
				return (this.Axis != RectTransform.Axis.Horizontal) ? this.TargetRectTransform.sizeDelta.y : this.TargetRectTransform.sizeDelta.x;
			}
			throw new InvalidOperationException();
		}

		private void SetCurrentValue(float value)
		{
			if (this.TargetLayoutElement != null)
			{
				if (this.Axis == RectTransform.Axis.Horizontal)
				{
					this.TargetLayoutElement.preferredWidth = value;
				}
				else
				{
					this.TargetLayoutElement.preferredHeight = value;
				}
				return;
			}
			if (this.TargetRectTransform != null)
			{
				Vector2 sizeDelta = this.TargetRectTransform.sizeDelta;
				if (this.Axis == RectTransform.Axis.Horizontal)
				{
					sizeDelta.x = value;
				}
				else
				{
					sizeDelta.y = value;
				}
				this.TargetRectTransform.sizeDelta = sizeDelta;
				return;
			}
			throw new InvalidOperationException();
		}

		private void CommitCurrentValue()
		{
			if (this.TargetLayoutElement != null)
			{
				if (this.Axis == RectTransform.Axis.Horizontal)
				{
					this.TargetLayoutElement.preferredWidth = ((RectTransform)this.TargetLayoutElement.transform).sizeDelta.x;
				}
				else
				{
					this.TargetLayoutElement.preferredHeight = ((RectTransform)this.TargetLayoutElement.transform).sizeDelta.y;
				}
			}
		}

		private float GetMinSize()
		{
			if (this.TargetLayoutElement == null)
			{
				return 0f;
			}
			return (this.Axis != RectTransform.Axis.Horizontal) ? this.TargetLayoutElement.minHeight : this.TargetLayoutElement.minWidth;
		}

		private float GetMaxSize()
		{
			if (this.MaxSize > 0f)
			{
				return this.MaxSize;
			}
			return float.MaxValue;
		}

		private CanvasScaler _canvasScaler;

		private float _delta;

		private float _startValue;

		public RectTransform.Axis Axis;

		public bool Invert;

		public float MaxSize = -1f;

		public LayoutElement TargetLayoutElement;

		public RectTransform TargetRectTransform;
	}
}
