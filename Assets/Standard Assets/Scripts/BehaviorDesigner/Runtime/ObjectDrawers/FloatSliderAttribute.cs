using System;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.ObjectDrawers
{
	public class FloatSliderAttribute : ObjectDrawerAttribute
	{
		public FloatSliderAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float min;

		public float max;
	}
}
