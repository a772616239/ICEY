using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/SRNumberSpinner")]
	public class SRNumberSpinner : InputField
	{
		protected override void Awake()
		{
			base.Awake();
			if (base.contentType != InputField.ContentType.IntegerNumber && base.contentType != InputField.ContentType.DecimalNumber)
			{
				UnityEngine.Debug.LogError("[SRNumberSpinner] contentType must be integer or decimal. Defaulting to integer");
				base.contentType = InputField.ContentType.DecimalNumber;
			}
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				return;
			}
			if (eventData.dragging)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			base.OnPointerClick(eventData);
			if (this.m_Keyboard == null || !this.m_Keyboard.active)
			{
				this.OnSelect(eventData);
			}
			else
			{
				base.UpdateLabel();
				eventData.Use();
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				return;
			}
			if (Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x))
			{
				Transform parent = base.transform.parent;
				if (parent != null)
				{
					eventData.pointerDrag = ExecuteEvents.GetEventHandler<IBeginDragHandler>(parent.gameObject);
					if (eventData.pointerDrag != null)
					{
						ExecuteEvents.Execute<IBeginDragHandler>(eventData.pointerDrag, eventData, ExecuteEvents.beginDragHandler);
					}
				}
				return;
			}
			eventData.Use();
			this._dragStartAmount = double.Parse(base.text);
			this._currentValue = this._dragStartAmount;
			float num = 1f;
			if (base.contentType == InputField.ContentType.IntegerNumber)
			{
				num *= 10f;
			}
			this._dragStep = Math.Max((double)num, this._dragStartAmount * 0.05000000074505806);
			if (base.isFocused)
			{
				base.DeactivateInputField();
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				return;
			}
			float x = eventData.delta.x;
			this._currentValue += Math.Abs(this._dragStep) * (double)x * (double)this.DragSensitivity;
			this._currentValue = Math.Round(this._currentValue, 2);
			if (this._currentValue > this.MaxValue)
			{
				this._currentValue = this.MaxValue;
			}
			if (this._currentValue < this.MinValue)
			{
				this._currentValue = this.MinValue;
			}
			if (base.contentType == InputField.ContentType.IntegerNumber)
			{
				base.text = ((int)Math.Round(this._currentValue)).ToString();
			}
			else
			{
				base.text = this._currentValue.ToString();
			}
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				return;
			}
			eventData.Use();
			if (this._dragStartAmount != this._currentValue)
			{
				base.DeactivateInputField();
				base.SendOnSubmit();
			}
		}

		private double _currentValue;

		private double _dragStartAmount;

		private double _dragStep;

		public float DragSensitivity = 0.01f;

		public double MaxValue = double.MaxValue;

		public double MinValue = double.MinValue;
	}
}
