using System;
using SRF.Helpers;

namespace SRDebugger.Internal
{
	public class OptionDefinition
	{
		private OptionDefinition(string name, string category, int sortPriority)
		{
			this.Name = name;
			this.Category = category;
			this.SortPriority = sortPriority;
		}

		public OptionDefinition(string name, string category, int sortPriority, MethodReference method) : this(name, category, sortPriority)
		{
			this.Method = method;
		}

		public OptionDefinition(string name, string category, int sortPriority, SRF.Helpers.PropertyReference property) : this(name, category, sortPriority)
		{
			this.Property = property;
		}

		public string Name { get; private set; }

		public string Category { get; private set; }

		public int SortPriority { get; private set; }

		public MethodReference Method { get; private set; }

		public SRF.Helpers.PropertyReference Property { get; private set; }
	}
}
