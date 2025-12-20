using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TaskNameAttribute : Attribute
	{
		public TaskNameAttribute(string name)
		{
			this.mName = name;
		}

		public string Name
		{
			get
			{
				return this.mName;
			}
		}

		public readonly string mName;
	}
}
