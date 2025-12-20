using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SRF.Service
{
	public abstract class SRDependencyServiceBase<T> : SRServiceBase<T>, IAsyncService where T : class
	{
		protected abstract Type[] Dependencies { get; }

		public bool IsLoaded
		{
			get
			{
				return this._isLoaded;
			}
		}

		[Conditional("ENABLE_LOGGING")]
		private void Log(string msg, UnityEngine.Object target)
		{
			UnityEngine.Debug.Log(msg, target);
		}

		protected override void Start()
		{
			base.Start();
			base.StartCoroutine(this.LoadDependencies());
		}

		protected virtual void OnLoaded()
		{
		}

		private IEnumerator LoadDependencies()
		{
			SRServiceManager.LoadingCount++;
			foreach (Type d in this.Dependencies)
			{
				bool hasService = SRServiceManager.HasService(d);
				if (!hasService)
				{
					object service = SRServiceManager.GetService(d);
					if (service == null)
					{
						UnityEngine.Debug.LogError("[Service] Could not resolve dependency ({0})".Fmt(new object[]
						{
							d.Name
						}));
						base.enabled = false;
						yield break;
					}
					IAsyncService a = service as IAsyncService;
					if (a != null)
					{
						while (!a.IsLoaded)
						{
							yield return new WaitForEndOfFrame();
						}
					}
				}
			}
			this._isLoaded = true;
			SRServiceManager.LoadingCount--;
			this.OnLoaded();
			yield break;
		}

		private bool _isLoaded;
	}
}
