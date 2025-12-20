using System;
using System.Diagnostics;
using UnityEngine;

namespace SRF.Components
{
	public abstract class SRAutoSingleton<T> : SRMonoBehaviour where T : SRAutoSingleton<T>
	{
		public static T Instance
		{
			[DebuggerStepThrough]
			get
			{
				if (SRAutoSingleton<T>._instance == null && Application.isPlaying)
				{
					GameObject gameObject = new GameObject("_" + typeof(T).Name);
					gameObject.AddComponent<T>();
				}
				return SRAutoSingleton<T>._instance;
			}
		}

		public static bool HasInstance
		{
			get
			{
				return SRAutoSingleton<T>._instance != null;
			}
		}

		protected virtual void Awake()
		{
			if (SRAutoSingleton<T>._instance != null)
			{
				UnityEngine.Debug.LogWarning("More than one singleton object of type {0} exists.".Fmt(new object[]
				{
					typeof(T).Name
				}));
				return;
			}
			SRAutoSingleton<T>._instance = (T)((object)this);
		}

		private void OnApplicationQuit()
		{
			SRAutoSingleton<T>._instance = (T)((object)null);
		}

		private static T _instance;
	}
}
