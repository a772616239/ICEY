using System;
using System.Collections;
using UnityEngine;

namespace SRF
{
	public static class SRFGameObjectExtensions
	{
		public static T GetIComponent<T>(this GameObject t) where T : class
		{
			return t.GetComponent(typeof(T)) as T;
		}

		public static T GetComponentOrAdd<T>(this GameObject obj) where T : Component
		{
			T t = obj.GetComponent<T>();
			if (t == null)
			{
				t = obj.AddComponent<T>();
			}
			return t;
		}

		public static void RemoveComponentIfExists<T>(this GameObject obj) where T : Component
		{
			T component = obj.GetComponent<T>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		public static void RemoveComponentsIfExists<T>(this GameObject obj) where T : Component
		{
			T[] components = obj.GetComponents<T>();
			for (int i = 0; i < components.Length; i++)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}

		public static bool EnableComponentIfExists<T>(this GameObject obj, bool enable = true) where T : MonoBehaviour
		{
			T component = obj.GetComponent<T>();
			if (component == null)
			{
				return false;
			}
			component.enabled = enable;
			return true;
		}

		public static void SetLayerRecursive(this GameObject o, int layer)
		{
			SRFGameObjectExtensions.SetLayerInternal(o.transform, layer);
		}

		private static void SetLayerInternal(Transform t, int layer)
		{
			t.gameObject.layer = layer;
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform t2 = (Transform)obj;
					SRFGameObjectExtensions.SetLayerInternal(t2, layer);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
