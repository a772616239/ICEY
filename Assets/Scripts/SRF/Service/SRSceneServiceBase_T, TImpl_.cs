using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRF.Service
{
	public abstract class SRSceneServiceBase<T, TImpl> : SRServiceBase<T>, IAsyncService where T : class where TImpl : Component
	{
		protected abstract string SceneName { get; }

		protected TImpl RootObject
		{
			get
			{
				return this._rootObject;
			}
		}

		public bool IsLoaded
		{
			get
			{
				return this._rootObject != null;
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
			base.StartCoroutine(this.LoadCoroutine());
		}

		protected override void OnDestroy()
		{
			if (this.IsLoaded)
			{
				UnityEngine.Object.Destroy(this._rootObject.gameObject);
			}
			base.OnDestroy();
		}

		protected virtual void OnLoaded()
		{
		}

		private IEnumerator LoadCoroutine()
		{
			if (this._rootObject != null)
			{
				yield break;
			}
			SRServiceManager.LoadingCount++;
			if (!SceneManager.GetSceneByName(this.SceneName).isLoaded)
			{
				yield return SceneManager.LoadSceneAsync(this.SceneName, LoadSceneMode.Additive);
			}
			GameObject go = GameObject.Find(this.SceneName);
			if (!(go == null))
			{
				TImpl timpl = go.GetComponent<TImpl>();
				if (!(timpl == null))
				{
					this._rootObject = timpl;
					this._rootObject.transform.parent = base.CachedTransform;
					UnityEngine.Object.DontDestroyOnLoad(go);
					UnityEngine.Debug.Log("[Service] Loading {0} complete. (Scene: {1})".Fmt(new object[]
					{
						base.GetType().Name,
						this.SceneName
					}), this);
					SRServiceManager.LoadingCount--;
					this.OnLoaded();
					yield break;
				}
			}
			SRServiceManager.LoadingCount--;
			UnityEngine.Debug.LogError("[Service] Root object ({0}) not found".Fmt(new object[]
			{
				this.SceneName
			}), this);
			base.enabled = false;
			yield break;
		}

		private TImpl _rootObject;
	}
}
