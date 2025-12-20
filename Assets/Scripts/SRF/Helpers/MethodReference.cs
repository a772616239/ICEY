using System;
using System.Reflection;

namespace SRF.Helpers
{
	public class MethodReference
	{
		public MethodReference(object target, MethodInfo method)
		{
			SRDebugUtil.AssertNotNull(target, null, null);
			this._target = target;
			this._method = method;
		}

		public string MethodName
		{
			get
			{
				return this._method.Name;
			}
		}

		public object Invoke(object[] parameters)
		{
			return this._method.Invoke(this._target, parameters);
		}

		private MethodInfo _method;

		private object _target;
	}
}
