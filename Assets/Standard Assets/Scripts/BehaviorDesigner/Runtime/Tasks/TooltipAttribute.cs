using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class TooltipAttribute : Attribute
	{
		public TooltipAttribute(string tooltip)
		{
			this.mTooltip = tooltip;
		}

		public string Tooltip
		{
			get
			{
				return this.mTooltip;
			}
		}

		public readonly string mTooltip;
	}
}
