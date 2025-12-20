using System;

namespace SRF.Service
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServiceConstructorAttribute : Attribute
	{
		public ServiceConstructorAttribute(Type serviceType)
		{
			this.ServiceType = serviceType;
		}

		public Type ServiceType { get; private set; }
	}
}
