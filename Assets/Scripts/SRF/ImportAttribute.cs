using System;

namespace SRF
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ImportAttribute : Attribute
	{
		public ImportAttribute()
		{
		}

		public ImportAttribute(Type serviceType)
		{
			this.Service = serviceType;
		}

		public readonly Type Service;
	}
}
