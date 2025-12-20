using System;
using System.Collections.Generic;
using SRF;
using SRF.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class NumberControl : DataBoundControl
	{
		protected override void Start()
		{
			base.Start();
			this.NumberSpinner.onEndEdit.AddListener(new UnityAction<string>(this.OnValueChanged));
		}

		private void OnValueChanged(string newValue)
		{
			try
			{
				object newValue2 = Convert.ChangeType(newValue, this._type);
				base.UpdateValue(newValue2);
			}
			catch (Exception)
			{
				this.NumberSpinner.text = this._lastValue;
			}
		}

		protected override void OnBind(string propertyName, Type t)
		{
			base.OnBind(propertyName, t);
			this.Title.text = propertyName;
			if (NumberControl.IsIntegerType(t))
			{
				this.NumberSpinner.contentType = InputField.ContentType.IntegerNumber;
			}
			else
			{
				if (!NumberControl.IsDecimalType(t))
				{
					throw new ArgumentException("Type must be one of expected types", "t");
				}
				this.NumberSpinner.contentType = InputField.ContentType.DecimalNumber;
			}
			SROptions.NumberRangeAttribute attribute = base.Property.GetAttribute<SROptions.NumberRangeAttribute>();
			this.NumberSpinner.MaxValue = this.GetMaxValue(t);
			this.NumberSpinner.MinValue = this.GetMinValue(t);
			if (attribute != null)
			{
				this.NumberSpinner.MaxValue = Math.Min(attribute.Max, this.NumberSpinner.MaxValue);
				this.NumberSpinner.MinValue = Math.Max(attribute.Min, this.NumberSpinner.MinValue);
			}
			SROptions.IncrementAttribute attribute2 = base.Property.GetAttribute<SROptions.IncrementAttribute>();
			if (attribute2 != null)
			{
				if (this.UpNumberButton != null)
				{
					this.UpNumberButton.Amount = attribute2.Increment;
				}
				if (this.DownNumberButton != null)
				{
					this.DownNumberButton.Amount = -attribute2.Increment;
				}
			}
			this._type = t;
			this.NumberSpinner.interactable = !base.IsReadOnly;
			if (this.DisableOnReadOnly != null)
			{
				foreach (GameObject gameObject in this.DisableOnReadOnly)
				{
					gameObject.SetActive(!base.IsReadOnly);
				}
			}
		}

		protected override void OnValueUpdated(object newValue)
		{
			string text = Convert.ToString(newValue);
			if (text != this._lastValue)
			{
				this.NumberSpinner.text = text;
			}
			this._lastValue = text;
		}

		public override bool CanBind(Type type, bool isReadOnly)
		{
			return NumberControl.IsDecimalType(type) || NumberControl.IsIntegerType(type);
		}

		protected static bool IsIntegerType(Type t)
		{
			for (int i = 0; i < NumberControl.IntegerTypes.Length; i++)
			{
				if (NumberControl.IntegerTypes[i] == t)
				{
					return true;
				}
			}
			return false;
		}

		protected static bool IsDecimalType(Type t)
		{
			for (int i = 0; i < NumberControl.DecimalTypes.Length; i++)
			{
				if (NumberControl.DecimalTypes[i] == t)
				{
					return true;
				}
			}
			return false;
		}

		protected double GetMaxValue(Type t)
		{
			NumberControl.ValueRange valueRange;
			if (NumberControl.ValueRanges.TryGetValue(t, out valueRange))
			{
				return valueRange.MaxValue;
			}
			UnityEngine.Debug.LogWarning("[NumberControl] No MaxValue stored for type {0}".Fmt(new object[]
			{
				t
			}));
			return double.MaxValue;
		}

		protected double GetMinValue(Type t)
		{
			NumberControl.ValueRange valueRange;
			if (NumberControl.ValueRanges.TryGetValue(t, out valueRange))
			{
				return valueRange.MinValue;
			}
			UnityEngine.Debug.LogWarning("[NumberControl] No MinValue stored for type {0}".Fmt(new object[]
			{
				t
			}));
			return double.MinValue;
		}

		private static readonly Type[] IntegerTypes = new Type[]
		{
			typeof(int),
			typeof(short),
			typeof(byte),
			typeof(sbyte),
			typeof(uint),
			typeof(ushort)
		};

		private static readonly Type[] DecimalTypes = new Type[]
		{
			typeof(float),
			typeof(double)
		};

		public static readonly Dictionary<Type, NumberControl.ValueRange> ValueRanges = new Dictionary<Type, NumberControl.ValueRange>
		{
			{
				typeof(int),
				new NumberControl.ValueRange
				{
					MaxValue = 2147483647.0,
					MinValue = -2147483648.0
				}
			},
			{
				typeof(short),
				new NumberControl.ValueRange
				{
					MaxValue = 32767.0,
					MinValue = -32768.0
				}
			},
			{
				typeof(byte),
				new NumberControl.ValueRange
				{
					MaxValue = 255.0,
					MinValue = 0.0
				}
			},
			{
				typeof(sbyte),
				new NumberControl.ValueRange
				{
					MaxValue = 127.0,
					MinValue = -128.0
				}
			},
			{
				typeof(uint),
				new NumberControl.ValueRange
				{
					MaxValue = 4294967295.0,
					MinValue = 0.0
				}
			},
			{
				typeof(ushort),
				new NumberControl.ValueRange
				{
					MaxValue = 65535.0,
					MinValue = 0.0
				}
			},
			{
				typeof(float),
				new NumberControl.ValueRange
				{
					MaxValue = 3.4028234663852886E+38,
					MinValue = -3.4028234663852886E+38
				}
			},
			{
				typeof(double),
				new NumberControl.ValueRange
				{
					MaxValue = double.MaxValue,
					MinValue = double.MinValue
				}
			}
		};

		private string _lastValue;

		private Type _type;

		public GameObject[] DisableOnReadOnly;

		public SRNumberButton DownNumberButton;

		[RequiredField]
		public SRNumberSpinner NumberSpinner;

		[RequiredField]
		public Text Title;

		public SRNumberButton UpNumberButton;

		public struct ValueRange
		{
			public double MaxValue;

			public double MinValue;
		}
	}
}
