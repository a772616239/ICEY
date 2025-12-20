using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/Long Press Button")]
	public class LongPressButton : Button
	{
		public Button.ButtonClickedEvent onLongPress
		{
			get
			{
				return this._onLongPress;
			}
			set
			{
				this._onLongPress = value;
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this._pressed = false;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this._pressed = true;
			this._handled = false;
			this._pressedTime = Time.realtimeSinceStartup;
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (!this._handled)
			{
				base.OnPointerUp(eventData);
			}
			this._pressed = false;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this._handled)
			{
				base.OnPointerClick(eventData);
			}
		}

		private void Update()
		{
			if (!this._pressed)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this._pressedTime >= this.LongPressDuration)
			{
				this._pressed = false;
				this._handled = true;
				this.onLongPress.Invoke();
			}
		}

		private bool _handled;

		[SerializeField]
		private Button.ButtonClickedEvent _onLongPress = new Button.ButtonClickedEvent();

		private bool _pressed;

		private float _pressedTime;

		public float LongPressDuration = 0.9f;
	}
}
