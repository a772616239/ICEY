using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TaskDescriptionAttribute : Attribute
	{
		public TaskDescriptionAttribute(string description)
		{
			this.mDescription = description;
		}

		public string Description
		{
			get
			{
				return this.mDescription;
			}
		}

		public readonly string mDescription;
	}
}
