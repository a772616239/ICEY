using System;
using SRF;

namespace SRDebugger
{
	public sealed class InfoEntry
	{
		public string Title { get; set; }

		public object Value
		{
			get
			{
				object result;
				try
				{
					result = this._valueGetter();
				}
				catch (Exception ex)
				{
					result = "Error ({0})".Fmt(new object[]
					{
						ex.GetType().Name
					});
				}
				return result;
			}
		}

		public bool IsPrivate { get; private set; }

		public static InfoEntry Create(string name, Func<object> getter, bool isPrivate = false)
		{
			return new InfoEntry
			{
				Title = name,
				_valueGetter = getter,
				IsPrivate = isPrivate
			};
		}

		public static InfoEntry Create(string name, object value, bool isPrivate = false)
		{
			return new InfoEntry
			{
				Title = name,
				_valueGetter = (() => value),
				IsPrivate = isPrivate
			};
		}

		private Func<object> _valueGetter;
	}
}
