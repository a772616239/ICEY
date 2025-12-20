using System;
using UnityEngine;

namespace ExtensionMethods
{
	public static class Vector2Utils
	{
		public static float Slope(this Vector2 vector2)
		{
			return vector2.y / vector2.x;
		}
	}
}
