using System;
using SRF;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class ReadOnlyControl : DataBoundControl
	{
		protected override void Start()
		{
			base.Start();
		}

		protected override void OnBind(string propertyName, Type t)
		{
			base.OnBind(propertyName, t);
			this.Title.text = propertyName;
		}

		protected override void OnValueUpdated(object newValue)
		{
			this.ValueText.text = Convert.ToString(newValue);
		}

		public override bool CanBind(Type type, bool isReadOnly)
		{
			return type == typeof(string) && isReadOnly;
		}

		[RequiredField]
		public Text ValueText;

		[RequiredField]
		public Text Title;
	}
}
