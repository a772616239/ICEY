using System;
using UnityEngine;

namespace Xft
{
	public class XftTools
	{
		public static void TopLeftUVToLowerLeft(ref Vector2 tl, ref Vector2 dimension)
		{
			tl.y = 1f - tl.y;
			dimension.y = -dimension.y;
		}
	}
}
