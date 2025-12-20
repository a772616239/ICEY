using System;
using SRF;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class StringControl : DataBoundControl
	{
		protected override void Start()
		{
			base.Start();
			this.InputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		}

		private void OnValueChanged(string newValue)
		{
			base.UpdateValue(newValue);
		}

		protected override void OnBind(string propertyName, Type t)
		{
			base.OnBind(propertyName, t);
			this.Title.text = propertyName;
			this.InputField.text = string.Empty;
			this.InputField.interactable = !base.IsReadOnly;
		}

		protected override void OnValueUpdated(object newValue)
		{
			string text = (newValue != null) ? ((string)newValue) : string.Empty;
			this.InputField.text = text;
		}

		public override bool CanBind(Type type, bool isReadOnly)
		{
			return type == typeof(string) && !isReadOnly;
		}

		[RequiredField]
		public InputField InputField;

		[RequiredField]
		public Text Title;
	}
}
