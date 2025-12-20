using System;
using UnityEngine;

namespace CIS
{
	public class SingletonMonoBehaviourClass<T> : MonoBehaviour where T : UnityEngine.Object, new()
	{
		public static T instance
		{
			get
			{
				if (SingletonMonoBehaviourClass<T>.s_Instance == null)
				{
					SingletonMonoBehaviourClass<T>.s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
				}
				if (SingletonMonoBehaviourClass<T>.s_Instance == null)
				{
					GameObject gameObject = new GameObject(typeof(T).ToString());
					SingletonMonoBehaviourClass<T>.s_Instance = (gameObject.AddComponent(typeof(T)) as T);
				}
				return SingletonMonoBehaviourClass<T>.s_Instance;
			}
		}

		public static void Release()
		{
			if (SingletonMonoBehaviourClass<T>.s_Instance != null)
			{
				MonoBehaviour monoBehaviour = SingletonMonoBehaviourClass<T>.s_Instance as MonoBehaviour;
				UnityEngine.Object.Destroy(monoBehaviour.gameObject);
				SingletonMonoBehaviourClass<T>.s_Instance = (T)((object)null);
			}
		}

		protected virtual void OnApplicationQuit()
		{
			SingletonMonoBehaviourClass<T>.s_Instance = (T)((object)null);
		}

		private static T s_Instance;
	}
}
