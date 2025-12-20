using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/Spinner")]
	public class SRSpinner : Selectable, IEventSystemHandler, IBeginDragHandler, IDragHandler
	{
		public SRSpinner.SpinEvent OnSpinIncrement
		{
			get
			{
				return this._onSpinIncrement;
			}
			set
			{
				this._onSpinIncrement = value;
			}
		}

		public SRSpinner.SpinEvent OnSpinDecrement
		{
			get
			{
				return this._onSpinDecrement;
			}
			set
			{
				this._onSpinDecrement = value;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			this._dragDelta = 0f;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				return;
			}
			this._dragDelta += eventData.delta.x;
			if (Mathf.Abs(this._dragDelta) > this.DragThreshold)
			{
				float num = Mathf.Sign(this._dragDelta);
				int num2 = Mathf.FloorToInt(Mathf.Abs(this._dragDelta) / this.DragThreshold);
				if (num > 0f)
				{
					this.OnIncrement(num2);
				}
				else
				{
					this.OnDecrement(num2);
				}
				this._dragDelta -= (float)num2 * this.DragThreshold * num;
			}
		}

		private void OnIncrement(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.OnSpinIncrement.Invoke();
			}
		}

		private void OnDecrement(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.OnSpinDecrement.Invoke();
			}
		}

		private float _dragDelta;

		[SerializeField]
		private SRSpinner.SpinEvent _onSpinDecrement = new SRSpinner.SpinEvent();

		[SerializeField]
		private SRSpinner.SpinEvent _onSpinIncrement = new SRSpinner.SpinEvent();

		public float DragThreshold = 20f;

		[Serializable]
		public class SpinEvent : UnityEvent
		{
		}
	}
}
