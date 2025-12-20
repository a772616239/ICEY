using System;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.ObjectDrawers
{
	public class IntSliderAttribute : ObjectDrawerAttribute
	{
		public IntSliderAttribute(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public int min;

		public int max;
	}
}
