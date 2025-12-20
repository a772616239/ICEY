using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRF
{
	public static class SRFTransformExtensions
	{
		public static IEnumerable<Transform> GetChildren(this Transform t)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				yield return t.GetChild(i);
			}
			yield break;
		}

		public static void ResetLocal(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}

		public static GameObject CreateChild(this Transform t, string name)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.parent = t;
			gameObject.transform.ResetLocal();
			gameObject.gameObject.layer = t.gameObject.layer;
			return gameObject;
		}

		public static void SetParentMaintainLocals(this Transform t, Transform parent)
		{
			t.SetParent(parent, false);
		}

		public static void SetLocals(this Transform t, Transform from)
		{
			t.localPosition = from.localPosition;
			t.localRotation = from.localRotation;
			t.localScale = from.localScale;
		}

		public static void Match(this Transform t, Transform from)
		{
			t.position = from.position;
			t.rotation = from.rotation;
		}

		public static void DestroyChildren(this Transform t)
		{
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					UnityEngine.Object.Destroy(((Transform)obj).gameObject);
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
