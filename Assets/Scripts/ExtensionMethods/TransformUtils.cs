using System;
using UnityEngine;

namespace ExtensionMethods
{
	public static class TransformUtils
	{
		public static Transform[] GetChildren(this Transform transform)
		{
			Transform[] array = new Transform[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				array[i] = child;
			}
			return array;
		}

		public static bool IsBiologicalChildOf(this Transform transform, GameObject parent)
		{
			return transform.parent == parent.transform;
		}

		public static bool IsBiologicalChildOf(this Transform transform, Transform parent)
		{
			return transform.parent == parent;
		}

		public static bool IsBiologicalParentOf(this Transform transform, GameObject child)
		{
			return transform == child.transform.parent;
		}

		public static bool IsBiologicalParentOf(this Transform transform, Transform child)
		{
			return transform == child.parent;
		}
	}
}
