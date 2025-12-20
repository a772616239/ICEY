using System;
using SRF;
using SRF.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class EnumControl : DataBoundControl
	{
		protected override void Start()
		{
			base.Start();
		}

		protected override void OnBind(string propertyName, Type t)
		{
			base.OnBind(propertyName, t);
			this.Title.text = propertyName;
			this.Spinner.interactable = !base.IsReadOnly;
			if (this.DisableOnReadOnly != null)
			{
				foreach (GameObject gameObject in this.DisableOnReadOnly)
				{
					gameObject.SetActive(!base.IsReadOnly);
				}
			}
			this._names = Enum.GetNames(t);
			this._values = Enum.GetValues(t);
			string text = string.Empty;
			for (int j = 0; j < this._names.Length; j++)
			{
				if (this._names[j].Length > text.Length)
				{
					text = this._names[j];
				}
			}
			if (this._names.Length == 0)
			{
				return;
			}
			float preferredWidth = this.Value.cachedTextGeneratorForLayout.GetPreferredWidth(text, this.Value.GetGenerationSettings(new Vector2(float.MaxValue, this.Value.preferredHeight)));
			this.ContentLayoutElement.preferredWidth = preferredWidth;
		}

		protected override void OnValueUpdated(object newValue)
		{
			this._lastValue = newValue;
			this.Value.text = newValue.ToString();
		}

		public override bool CanBind(Type type, bool isReadOnly)
		{
			return type.IsEnum;
		}

		private void SetIndex(int i)
		{
			base.UpdateValue(this._values.GetValue(i));
			this.Refresh();
		}

		public void GoToNext()
		{
			int num = Array.IndexOf(this._values, this._lastValue);
			this.SetIndex(SRMath.Wrap(this._values.Length, num + 1));
		}

		public void GoToPrevious()
		{
			int num = Array.IndexOf(this._values, this._lastValue);
			this.SetIndex(SRMath.Wrap(this._values.Length, num - 1));
		}

		private object _lastValue;

		private string[] _names;

		private Array _values;

		[RequiredField]
		public LayoutElement ContentLayoutElement;

		public GameObject[] DisableOnReadOnly;

		[RequiredField]
		public SRSpinner Spinner;

		[RequiredField]
		public Text Title;

		[RequiredField]
		public Text Value;
	}
}
