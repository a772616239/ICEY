using System;
using UnityEngine;

public class SingletonMono<T> : BaseBehaviour where T : MonoBehaviour
{
	protected SingletonMono()
	{
	}

	public static T Instance
	{
		get
		{
			if (SingletonMono<T>.ApplicationIsQuitting)
			{
				UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				return (T)((object)null);
			}
			object @lock = SingletonMono<T>.Lock;
			lock (@lock)
			{
				if (SingletonMono<T>._instance == null)
				{
					SingletonMono<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
				}
			}
			return SingletonMono<T>._instance;
		}
	}

	protected virtual void OnDestroy()
	{
		SingletonMono<T>.ApplicationIsQuitting = true;
	}

	public static bool IsValid()
	{
		return SingletonMono<T>._instance != null;
	}

	private static T _instance;

	private static readonly object Lock = new object();

	public static bool ApplicationIsQuitting = false;
}
