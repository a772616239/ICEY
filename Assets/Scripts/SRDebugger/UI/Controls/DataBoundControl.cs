using System;
using SRF.Helpers;
using UnityEngine;

namespace SRDebugger.UI.Controls
{
	public abstract class DataBoundControl : OptionsControlBase
	{
		public SRF.Helpers.PropertyReference Property
		{
			get
			{
				return this._prop;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
		}

		public string PropertyName { get; private set; }

		public void Bind(string propertyName, SRF.Helpers.PropertyReference prop)
		{
			this.PropertyName = propertyName;
			this._prop = prop;
			this._isReadOnly = !prop.CanWrite;
			this.OnBind(propertyName, prop.PropertyType);
			this.Refresh();
		}

		protected void UpdateValue(object newValue)
		{
			if (newValue == this._prevValue)
			{
				return;
			}
			if (this.IsReadOnly)
			{
				return;
			}
			this._prop.SetValue(newValue);
			this._prevValue = newValue;
		}

		public override void Refresh()
		{
			if (this._prop == null)
			{
				return;
			}
			object value = this._prop.GetValue();
			if (value != this._prevValue)
			{
				try
				{
					this.OnValueUpdated(value);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError("[SROptions] Error refreshing binding.");
					UnityEngine.Debug.LogException(exception);
				}
			}
			this._prevValue = value;
		}

		protected virtual void OnBind(string propertyName, Type t)
		{
		}

		protected abstract void OnValueUpdated(object newValue);

		public abstract bool CanBind(Type type, bool isReadOnly);

		protected override void Start()
		{
			base.Start();
			this.Refresh();
			this._hasStarted = true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this._hasStarted)
			{
				this.Refresh();
			}
		}

		private bool _hasStarted;

		private bool _isReadOnly;

		private object _prevValue;

		private SRF.Helpers.PropertyReference _prop;
	}
}
