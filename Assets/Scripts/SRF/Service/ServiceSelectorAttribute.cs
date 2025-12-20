using System;

namespace SRF.Service
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServiceSelectorAttribute : Attribute
	{
		public ServiceSelectorAttribute(Type serviceType)
		{
			this.ServiceType = serviceType;
		}

		public Type ServiceType { get; private set; }
	}
}
