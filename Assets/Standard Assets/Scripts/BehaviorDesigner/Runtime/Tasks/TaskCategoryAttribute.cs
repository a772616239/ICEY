using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TaskCategoryAttribute : Attribute
	{
		public TaskCategoryAttribute(string category)
		{
			this.mCategory = category;
		}

		public string Category
		{
			get
			{
				return this.mCategory;
			}
		}

		public readonly string mCategory;
	}
}
