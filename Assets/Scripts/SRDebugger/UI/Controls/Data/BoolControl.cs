using System;
using SRF;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class BoolControl : DataBoundControl
	{
		protected override void Start()
		{
			base.Start();
			this.Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleOnValueChanged));
		}

		private void ToggleOnValueChanged(bool isOn)
		{
			base.UpdateValue(isOn);
		}

		protected override void OnBind(string propertyName, Type t)
		{
			base.OnBind(propertyName, t);
			this.Title.text = propertyName;
			this.Toggle.interactable = !base.IsReadOnly;
		}

		protected override void OnValueUpdated(object newValue)
		{
			bool isOn = (bool)newValue;
			this.Toggle.isOn = isOn;
		}

		public override bool CanBind(Type type, bool isReadOnly)
		{
			return type == typeof(bool);
		}

		[RequiredField]
		public Text Title;

		[RequiredField]
		public Toggle Toggle;
	}
}
