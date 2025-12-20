using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	public abstract class SharedVariable<T> : SharedVariable
	{
		public override void InitializePropertyMapping(BehaviorSource behaviorSource)
		{
			if (!Application.isPlaying || !(behaviorSource.Owner.GetObject() is Behavior))
			{
				return;
			}
			if (!string.IsNullOrEmpty(base.PropertyMapping))
			{
				string[] array = base.PropertyMapping.Split(new char[]
				{
					'/'
				});
				GameObject gameObject;
				if (!object.Equals(base.PropertyMappingOwner, null))
				{
					gameObject = base.PropertyMappingOwner;
				}
				else
				{
					gameObject = (behaviorSource.Owner.GetObject() as Behavior).gameObject;
				}
				if (gameObject == null)
				{
					UnityEngine.Debug.LogError("Error: Unable to find GameObject on " + behaviorSource.behaviorName + " for property mapping with variable " + base.Name);
					return;
				}
				Component component = gameObject.GetComponent(TaskUtility.GetTypeWithinAssembly(array[0]));
				if (component == null)
				{
					UnityEngine.Debug.LogError("Error: Unable to find component on " + behaviorSource.behaviorName + " for property mapping with variable " + base.Name);
					return;
				}
				Type type = component.GetType();
				PropertyInfo property = type.GetProperty(array[1]);
				if (property != null)
				{
					MethodInfo methodInfo = property.GetGetMethod();
					if (methodInfo != null)
					{
						this.mGetter = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, methodInfo);
					}
					methodInfo = property.GetSetMethod();
					if (methodInfo != null)
					{
						this.mSetter = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), component, methodInfo);
					}
				}
			}
		}

		public T Value
		{
			get
			{
				return (this.mGetter == null) ? this.mValue : this.mGetter();
			}
			set
			{
				bool flag = base.NetworkSync && !this.mValue.Equals(value);
				if (this.mSetter != null)
				{
					this.mSetter(value);
				}
				else
				{
					this.mValue = value;
				}
				if (flag)
				{
					base.ValueChanged();
				}
			}
		}

		public override object GetValue()
		{
			return this.Value;
		}

		public override void SetValue(object value)
		{
			if (this.mSetter != null)
			{
				this.mSetter((T)((object)value));
			}
			else
			{
				this.mValue = (T)((object)value);
			}
		}

		public override string ToString()
		{
			string result;
			if (this.Value == null)
			{
				result = "(null)";
			}
			else
			{
				T value = this.Value;
				result = value.ToString();
			}
			return result;
		}

		private Func<T> mGetter;

		private Action<T> mSetter;

		[SerializeField]
		protected T mValue;
	}
}
