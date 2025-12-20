using System;
using System.Diagnostics;
using UnityEngine;

namespace SRF.Components
{
	public abstract class SRSingleton<T> : SRMonoBehaviour where T : SRSingleton<T>
	{
		public static T Instance
		{
			[DebuggerStepThrough]
			get
			{
				if (SRSingleton<T>._instance == null)
				{
					throw new InvalidOperationException("No instance of {0} present in scene".Fmt(new object[]
					{
						typeof(T).Name
					}));
				}
				return SRSingleton<T>._instance;
			}
		}

		public static bool HasInstance
		{
			[DebuggerStepThrough]
			get
			{
				return SRSingleton<T>._instance != null;
			}
		}

		private void Register()
		{
			if (SRSingleton<T>._instance != null)
			{
				UnityEngine.Debug.LogWarning("More than one singleton object of type {0} exists.".Fmt(new object[]
				{
					typeof(T).Name
				}));
				if (base.GetComponents<Component>().Length == 2)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(this);
				}
				return;
			}
			SRSingleton<T>._instance = (T)((object)this);
		}

		protected virtual void Awake()
		{
			this.Register();
		}

		protected virtual void OnEnable()
		{
			if (SRSingleton<T>._instance == null)
			{
				this.Register();
			}
		}

		private void OnApplicationQuit()
		{
			SRSingleton<T>._instance = (T)((object)null);
		}

		private static T _instance;
	}
}
