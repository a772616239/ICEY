using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class RequiredComponentAttribute : Attribute
	{
		public RequiredComponentAttribute(Type componentType)
		{
			this.mComponentType = componentType;
		}

		public Type ComponentType
		{
			get
			{
				return this.mComponentType;
			}
		}

		public readonly Type mComponentType;
	}
}
