using System;
using UnityEngine;

namespace ExtensionMethods
{
	public static class GameObjectUtils
	{
		public static bool IsBiologicalChildOf(this GameObject gameObject, GameObject parent)
		{
			return gameObject.transform.parent == parent.transform;
		}

		public static bool IsBiologicalChildOf(this GameObject gameObject, Transform parent)
		{
			return gameObject.transform.parent == parent;
		}

		public static bool IsBiologicalParentOf(this GameObject gameObject, GameObject child)
		{
			return gameObject.transform == child.transform.parent;
		}

		public static bool IsBiologicalParentOf(this GameObject gameObject, Transform child)
		{
			return gameObject.transform == child.parent;
		}
	}
}
