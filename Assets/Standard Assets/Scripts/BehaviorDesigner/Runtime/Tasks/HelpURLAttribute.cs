using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class HelpURLAttribute : Attribute
	{
		public HelpURLAttribute(string url)
		{
			this.mURL = url;
		}

		public string URL
		{
			get
			{
				return this.mURL;
			}
		}

		private readonly string mURL;
	}
}
