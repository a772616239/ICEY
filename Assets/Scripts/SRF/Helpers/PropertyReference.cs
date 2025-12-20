using System;
using System.Linq;
using System.Reflection;

namespace SRF.Helpers
{
	public class PropertyReference
	{
		public PropertyReference(object target, PropertyInfo property)
		{
			SRDebugUtil.AssertNotNull(target, null, null);
			this._target = target;
			this._property = property;
		}

		public string PropertyName
		{
			get
			{
				return this._property.Name;
			}
		}

		public Type PropertyType
		{
			get
			{
				return this._property.PropertyType;
			}
		}

		public bool CanRead
		{
			get
			{
				return this._property.GetGetMethod() != null;
			}
		}

		public bool CanWrite
		{
			get
			{
				return this._property.GetSetMethod() != null;
			}
		}

		public object GetValue()
		{
			if (this._property.CanRead)
			{
				return SRReflection.GetPropertyValue(this._target, this._property);
			}
			return null;
		}

		public void SetValue(object value)
		{
			if (this._property.CanWrite)
			{
				SRReflection.SetPropertyValue(this._target, this._property, value);
				return;
			}
			throw new InvalidOperationException("Can not write to property");
		}

		public T GetAttribute<T>() where T : Attribute
		{
			object obj = this._property.GetCustomAttributes(typeof(T), true).FirstOrDefault<object>();
			return obj as T;
		}

		private readonly PropertyInfo _property;

		private readonly object _target;
	}
}
